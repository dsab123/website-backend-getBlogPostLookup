using System;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;

using Amazon.Lambda.Core;
using website_backend_getBlogPostLookup.Models;
using website_backend_getBlogPostLookup.Utility;
using website_backend_getBlogPostLookup.DataAccess;
using website_backend_getBlogPostLookup.Configuration;

[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.CamelCaseLambdaJsonSerializer))]

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
    /// Entry point to retrieve BlogPost information from database
    /// </summary>
    /// <param name="context"></param>
    /// <returns></returns>
    public List<BlogPostInfo> FunctionHandler(ILambdaContext context)
    {
      LambdaLogger.Log("getBlogPostLookup Lambda Started");

      try
      {
        LambdaLogger.Log("getBlogPostLookup Lambda finishing");
        return DataContext.GetBlogPostLookup();
      }
      catch (Exception ex)
      {
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
      ret.FunctionHandler(null);
    }
  }
}
