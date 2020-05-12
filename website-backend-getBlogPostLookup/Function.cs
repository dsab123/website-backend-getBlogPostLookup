using System;
using System.Runtime.InteropServices;
using Microsoft.Extensions.DependencyInjection;

using Newtonsoft.Json;
using Amazon.Lambda.Core;
using Newtonsoft.Json.Serialization;
using website_backend_getBlogPostLookup.Utility;
using website_backend_getBlogPostLookup.DataAccess;
using website_backend_getBlogPostLookup.Configuration;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace website_backend_getBlogPostLookup
{
  public class Function
  {
    public ISqlDataContext DataContext;

    private readonly IExceptionLogFormatter _exceptionLogFormatter;

    public Function()
    {
      var serviceCollection = new ServiceCollection();
      ConfigureServices(serviceCollection);
      var serviceProvider = serviceCollection.BuildServiceProvider();

      DataContext = serviceProvider.GetService<ISqlDataContext>();
      _exceptionLogFormatter = serviceProvider.GetService<IExceptionLogFormatter>();
    }

    /// <summary>
    /// Entry point to retrieve json-formatted lookup table mapping BlogPost ids to slugs, from
    /// cross-reference table in RDS
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public string FunctionHandler(string input, ILambdaContext context)
    {
      LambdaLogger.Log("getBlogPostLookup Lambda Started");

      var blogPostLookup = DataContext.GetBlogPostLookup();

      try
      {
        return JsonConvert.SerializeObject(blogPostLookup);
      }
      catch (Exception ex)
      {
        LambdaLogger.Log(blogPostLookup.ToString());
        LambdaLogger.Log(_exceptionLogFormatter.FormatExceptionLogMessage(ex));
        throw;
      }
    }

    private void ConfigureServices(IServiceCollection serviceCollection)
    {
      serviceCollection.AddTransient<ILambdaConfiguration, LambdaConfiguration>();
      serviceCollection.AddTransient<ISqlDataContext, SqlDataContext>();
      serviceCollection.AddTransient<IExceptionLogFormatter, ExceptionLogFormatter>();
    }

    // used in local testing
    public static void Main()
    {
      var ret =  new Function();
      ret.FunctionHandler("something", null);
    }
  }
}
