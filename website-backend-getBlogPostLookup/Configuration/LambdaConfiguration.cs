using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Microsoft.Extensions.Configuration;

namespace website_backend_getBlogPostLookup.Configuration
{
  public class LambdaConfiguration : ILambdaConfiguration
  {
    public IConfiguration Configuration => new ConfigurationBuilder()
        .SetBasePath(Directory.GetCurrentDirectory())
        .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
        .Build();
  }
}
