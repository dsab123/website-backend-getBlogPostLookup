using System;

using Moq;
using Xunit;

using Amazon.Lambda.Core;

using website_backend_getBlogPostLookup.Utility;
using website_backend_getBlogPostLookup.DataAccess;
using website_backend_getBlogPostLookup.Configuration;
using Microsoft.Extensions.Configuration;
using Microsoft.VisualBasic.CompilerServices;

namespace website_backend_getBlogPostLookup.Tests
{ 
  public class SqlDataContextTests
  {
    private readonly Mock<ILambdaConfiguration> _lambdaConfiguration;
    private readonly Mock<IExceptionLogFormatter> _exceptionLogFormatter;

    // since IConfiguration isn't bare, but wrapped in ILambdaConfiguration, and is readonly, I need
    // this wrapper to duplicate it
    private class FakeCompleteLambdaConfiguration : ILambdaConfiguration
    {
      private readonly Mock<IConfiguration> MockConfiguration;
      public IConfiguration Configuration => MockConfiguration.Object;

      public FakeCompleteLambdaConfiguration()
      {
        MockConfiguration = new Mock<IConfiguration>();

        var mockConfigurationSection = new Mock<IConfigurationSection>();
        mockConfigurationSection.Setup(a => a.Value).Returns("fake");

        MockConfiguration.Setup(a => a.GetSection(It.IsAny<string>())).Returns(mockConfigurationSection.Object);
      }
    }

    private class FakeIncompleteLambdaConfiguration : ILambdaConfiguration
    {
      private readonly Mock<IConfiguration> _mockConfiguration;
      public IConfiguration Configuration => _mockConfiguration.Object;

      public FakeIncompleteLambdaConfiguration()
      {
        _mockConfiguration = new Mock<IConfiguration>();
      }
    }

    public SqlDataContextTests()
    {
      _exceptionLogFormatter = new Mock<IExceptionLogFormatter>();
    }

    [Fact]
    public void CreateConnection_ValidConfiguration_DoesNotThrow()
    {
      // Arrange, Act
      var dbContext = new SqlDataContext(
        new FakeCompleteLambdaConfiguration(),
        _exceptionLogFormatter.Object);

      // not sure of a way to validate that the SqlDataContext object is valid.
      // HOWEVER, I'm not going to write tests against GetBlogPostLookup, because it is a simple
      // database access method, and assuming the configuration is valid, the
      // method will be fine. Also, too much work to mock/wrap SqlConnection and SqlCommand,
      // with little payoff.
    }

    [Fact]
    public void CreateConnection_IncompleteOrInvalidConfiguration_ThrowsNullReferenceException()
    {

      var x = System.Reflection.Assembly.GetExecutingAssembly().GetName().Name;

      // Arrange, Act, Assert
      Assert.Throws<NullReferenceException>(() =>
        new SqlDataContext(
          new FakeIncompleteLambdaConfiguration(),
          _exceptionLogFormatter.Object));
    }
  }
}
