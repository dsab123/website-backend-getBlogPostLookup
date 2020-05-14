using System;
using System.Text;
using System.Collections.Generic;
using Amazon.Lambda.Core;

using Npgsql;
using website_backend_getBlogPostLookup.Configuration;
using website_backend_getBlogPostLookup.Models;
using website_backend_getBlogPostLookup.Utility;

namespace website_backend_getBlogPostLookup.DataAccess
{
  public class SqlDataContext : ISqlDataContext, IDisposable
  {
    private readonly ILambdaConfiguration _lambdaConfiguration;
    private readonly IExceptionLogFormatter _exceptionLogFormatter;
    private NpgsqlConnection Connection { get; }

    public SqlDataContext(ILambdaConfiguration lambdaConfiguration, IExceptionLogFormatter exceptionLogFormatter)
    {
      _lambdaConfiguration = lambdaConfiguration;
      _exceptionLogFormatter = exceptionLogFormatter;

      Connection = CreateConnection();
    }

    // TODO - genericize the input model so that this can be reused in a Lambda Layer
    public List<BlogPostInfo> GetBlogPostLookup()
    {
      var blogPostLookup = new List<BlogPostInfo>();

      try
      {
        using (var command = new NpgsqlCommand($"SELECT * from public.blogpostInfo", Connection))
        {
          LambdaLogger.Log("before connection open");
          Connection.Open();
          LambdaLogger.Log("after connection open");
          var reader = command.ExecuteReader();

          while (reader.Read())
          {
            var id = int.Parse(reader["blogpost_id"].ToString());
            var slug = reader["slug"].ToString();
            var title = reader["title"].ToString();
            var teaser = reader["teaser"].ToString();
            blogPostLookup.Add(new BlogPostInfo(id, slug, title, teaser));
          }
        }
      }
      catch (Exception ex)
      {
        LambdaLogger.Log(_exceptionLogFormatter.FormatExceptionLogMessage(ex));
      }

      Connection.Close();
      return blogPostLookup;
    }

    private NpgsqlConnection CreateConnection()
    {
      if (Connection != null)
      {
        return Connection;
      }

      try
      {
        var section = _lambdaConfiguration.Configuration.GetSection("AppSettings");
        
        var server = section["Server"];
        var username = section["Username"];
        var database = section["Database"];
        var password = section["Password"];

        return new NpgsqlConnection(string.Format($"Database={database};Host={server};User ID={username};Password={password}"));
      } 
      catch (Exception ex) {
        LambdaLogger.Log(_exceptionLogFormatter.FormatExceptionLogMessage(ex, new StringBuilder("ConnectionString was not retrieved from configuration, probably.")));
        throw;
      }
    }

    public void Dispose()
    {
      Connection.Dispose();
    }
  }
}
