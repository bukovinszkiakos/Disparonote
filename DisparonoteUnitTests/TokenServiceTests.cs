using NUnit.Framework;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using Microsoft.AspNetCore.Identity;
using System.Linq;
using System.Security.Claims;

[TestFixture]
public class TokenServiceTests
{
    private TokenService _tokenService;

    [SetUp]
    public void Setup()
    {
        var inMemorySettings = new Dictionary<string, string> {
            {"Jwt:ValidIssuer", "test-issuer"},
            {"Jwt:ValidAudience", "test-audience"},
            {"Jwt:IssuerSigningKey", "supersecretkey_supersecretkey_1234567890"} 
        };

        IConfiguration configuration = new ConfigurationBuilder()
            .AddInMemoryCollection(inMemorySettings)
            .Build();

        _tokenService = new TokenService(configuration);
    }

    [Test]
    public void CreateToken_ValidUser_ReturnsJwt()
    {
        var user = new IdentityUser
        {
            Id = "123",
            Email = "test@example.com",
            UserName = "testuser"
        };

        var token = _tokenService.CreateToken(user, "User", rememberMe: false);

        Assert.IsNotNull(token);

        var handler = new JwtSecurityTokenHandler();
        var jwtToken = handler.ReadJwtToken(token);


        Assert.AreEqual("test-issuer", jwtToken.Issuer);
        Assert.AreEqual("test-audience", jwtToken.Audiences.First());
        Assert.IsTrue(jwtToken.Claims.Any(c => c.Type == "exp"));
        Assert.IsTrue(jwtToken.Claims.Any(c => c.Type == ClaimTypes.Email && c.Value == "test@example.com"));
        Assert.IsTrue(jwtToken.Claims.Any(c => c.Type == ClaimTypes.Role && c.Value == "User"));
    }
}
