using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Moq;
using website_backend_getBlogPostLookup.DataAccess;
using website_backend_getBlogPostLookup.Models;

namespace website_backend_getBlogPostLookup.Tests
{
  public class FunctionTests
  {
    private readonly FunctionExtractAndOverride _function;

    // we need to override the constructor so that we don't do any of that snazzy configuration,
    // and also to set the dataContext in an easier manner
    private class FunctionExtractAndOverride : Function
    {
      private readonly Mock<ISqlDataContext> _mockContext;

      public FunctionExtractAndOverride()
      {
        _mockContext = new Mock<ISqlDataContext>();
        _mockContext.Setup(c => c.GetBlogPostLookup())
          .Returns(new List<BlogPostIdXSlug>
          {
            new BlogPostIdXSlug(1, "slugOne"),
            new BlogPostIdXSlug(2, "slugTwo"),
            new BlogPostIdXSlug(3, "slugThree"),
            new BlogPostIdXSlug(4, "slugFour")
          });

        DataContext = _mockContext.Object;
      }
    } 

    public FunctionTests()
    {
      _function = new FunctionExtractAndOverride();
    }

    [Fact]
    public void FunctionHandler_ContextReturnsValidList_JsonSerializationIsValid()
    {
      // Arrange
      var function = new FunctionExtractAndOverride();

      // Act
      var ret = function.FunctionHandler(string.Empty, null);

      // Assert
      Assert.Equal("[{\"Id\":1,\"Slug\":\"slugOne\"},{\"Id\":2,\"Slug\":\"slugTwo\"},{\"Id\":3,\"Slug\":\"slugThree\"},{\"Id\":4,\"Slug\":\"slugFour\"}]", ret);
    }
  }
}
