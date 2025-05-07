using FakeItEasy;
using Microsoft.Extensions.Configuration;
using UATP.Core.ApiModels;
using UATP.Core.Interfaces;
using UATP.Core.Services;

namespace UATP.Tests.Services;

[TestFixture]
public class AuthenticationServiceTests
{
    private IAuthenticationService _authenticationService;
    private IConfiguration _configuration;
    [SetUp]
    public void SetUp()
    {
        _configuration = A.Fake<IConfiguration>();
        A.CallTo(() => _configuration["Jwt:Key"]).Returns("some long string of text that should act as a key");
        A.CallTo(() => _configuration["Jwt:Username"]).Returns("user@example.com");
        A.CallTo(() => _configuration["Jwt:Password"]).Returns("Passw0rd!");
        A.CallTo(() => _configuration["Jwt:Expires"]).Returns("86400");
        _authenticationService = new AuthenticationService(_configuration);
    }

    [TearDown]
    public void TearDown()
    {
        _authenticationService = null;
    }

    [Test]
    public void AuthorizeShouldNotThrow()
    {
        Assert.That(
            () => _authenticationService.Authenticate(new AuthenticationModel()
                { Username = string.Empty, Password = string.Empty }), Throws.Nothing);
    }

    [Test]
    public void AuthorizeShouldReturnNullForInvalidUsernameAndPassword()
    {
        var result =
            _authenticationService.Authenticate(new AuthenticationModel { Username = string.Empty, Password = string.Empty });
        Assert.That(result, Is.Null);
    }

    [Test]
    public void AuthorizeShouldReturnNotNullForValidUsernameAndPassword()
    {
        var result = _authenticationService.Authenticate(new AuthenticationModel
            { Username = "user@example.com", Password = "Passw0rd!" });
        Assert.That(result, Is.Not.Null);
    }
}