using System.Collections.Generic;

using website_backend_getBlogPostLookup.Models;

namespace website_backend_getBlogPostLookup.DataAccess
{
  public interface ISqlDataContext
  {
    List<BlogPostIdXSlug> GetBlogPostLookup();
  }
}
