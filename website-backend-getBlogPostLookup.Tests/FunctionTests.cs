using System.Collections.Generic;

using Moq;
using Xunit;
using website_backend_getBlogPostLookup.DataAccess;
using website_backend_getBlogPostLookup.Models;

namespace website_backend_getBlogPostLookup.Tests
{
  public class FunctionTests
  {
    // we need to override the constructor so that we don't do any of that snazzy configuration,
    // and also to set the dataContext in an easier manner
    private class FunctionExtractAndOverride : Function
    {
      private readonly Mock<ISqlDataContext> _mockContext;

      public FunctionExtractAndOverride()
      {
        _mockContext = new Mock<ISqlDataContext>();
        _mockContext.Setup(c => c.GetBlogPostLookup())
          .Returns(new List<BlogPostInfo>
          {
            new BlogPostInfo(1, "slugOne", "title", "teaser"),
            new BlogPostInfo(2, "slugTwo", "title", "teaser"),
            new BlogPostInfo(3, "slugThree", "title", "teaser"),
            new BlogPostInfo(4, "slugFour", "title", "teaser")
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
      var ret = function.FunctionHandler(null);

      // Assert
      Assert.Equal(4, ret.Count);
      Assert.Equal("slugOne", ret[0].Slug);
      Assert.Equal("slugTwo", ret[1].Slug);
      Assert.Equal("slugThree", ret[2].Slug);
      Assert.Equal("slugFour", ret[3].Slug);
    }
  }
}
