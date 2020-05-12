using System;
using System.Text;

namespace website_backend_getBlogPostLookup.Utility
{
  public interface IExceptionLogFormatter
  {
    string FormatExceptionLogMessage(Exception ex, StringBuilder builder = null);
  }
}
