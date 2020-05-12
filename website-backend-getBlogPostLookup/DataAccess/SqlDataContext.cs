using System;
using System.Data.SqlClient;
using System.Collections.Generic;
using System.Text;
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
    public List<BlogPostIdXSlug> GetBlogPostLookup()
    {
      var blogPostLookup = new List<BlogPostIdXSlug>();

      try
      {
        using (var command = new NpgsqlCommand($"SELECT * from public.blogpostid_slug", Connection))
        {
          Connection.Open();
          var reader = command.ExecuteReader();

          while (reader.Read())
          {
            var id = int.Parse(reader["blogpost_id"].ToString());
            var slug = reader["slug"].ToString();
            blogPostLookup.Add(new BlogPostIdXSlug(id, slug));
          }
        }
      }
      catch (Exception ex)
      {
        LambdaLogger.Log(_exceptionLogFormatter.FormatExceptionLogMessage(ex));
      }

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
