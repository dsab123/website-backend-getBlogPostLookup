using Microsoft.Extensions.Configuration;

namespace website_backend_getBlogPostLookup.Configuration
{
  public interface ILambdaConfiguration
  {
    IConfiguration Configuration { get; }
  }
}