using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;

[TestFixture]
public class AuthServiceTests
{
    private Mock<UserManager<IdentityUser>> _userManagerMock;
    private Mock<ITokenService> _tokenServiceMock;
    private AuthService _authService;

    [SetUp]
    public void Setup()
    {
        var store = new Mock<IUserStore<IdentityUser>>();
        _userManagerMock = new Mock<UserManager<IdentityUser>>(
            store.Object, null, null, null, null, null, null, null, null
        );
        _tokenServiceMock = new Mock<ITokenService>();

        _authService = new AuthService(_userManagerMock.Object, _tokenServiceMock.Object);
    }

    [Test]
    public async Task RegisterAsync_Success_ReturnsSuccessAuthResult()
    {
        var email = "test@test.com";
        var username = "testuser";
        var password = "Password123";

        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), password))
            .ReturnsAsync(IdentityResult.Success);

        var result = await _authService.RegisterAsync(email, username, password);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Email, Is.EqualTo(email));
        Assert.That(result.Username, Is.EqualTo(username));
    }

    [Test]
    public async Task RegisterAsync_Fails_ReturnsErrorMessages()
    {
        var identityError = new IdentityError { Code = "DuplicateEmail", Description = "Email already taken" };
        var failedResult = IdentityResult.Failed(identityError);

        _userManagerMock
            .Setup(m => m.CreateAsync(It.IsAny<IdentityUser>(), It.IsAny<string>()))
            .ReturnsAsync(failedResult);

        var result = await _authService.RegisterAsync("email", "user", "pass");

        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessages.ContainsKey("DuplicateEmail"), Is.True);
    }

    [Test]
    public async Task LoginAsync_ValidCredentials_ReturnsToken()
    {
        var email = "test@test.com";
        var password = "Password123";
        var user = new IdentityUser { UserName = "testuser", Email = email };

        _userManagerMock.Setup(m => m.FindByEmailAsync(email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, password)).ReturnsAsync(true);
        _tokenServiceMock.Setup(t => t.CreateToken(user, null, false)).Returns("mocked-token");

        var result = await _authService.LoginAsync(email, password);

        Assert.That(result.Success, Is.True);
        Assert.That(result.Token, Is.EqualTo("mocked-token"));
    }

    [Test]
    public async Task LoginAsync_InvalidEmail_ReturnsError()
    {
        _userManagerMock.Setup(m => m.FindByEmailAsync("bad@email.com")).ReturnsAsync((IdentityUser)null);

        var result = await _authService.LoginAsync("bad@email.com", "pass");

        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessages.ContainsKey("BadCredentials"), Is.True);
    }

    [Test]
    public async Task LoginAsync_InvalidPassword_ReturnsError()
    {
        var user = new IdentityUser { UserName = "testuser", Email = "test@test.com" };

        _userManagerMock.Setup(m => m.FindByEmailAsync(user.Email)).ReturnsAsync(user);
        _userManagerMock.Setup(m => m.CheckPasswordAsync(user, "wrongpass")).ReturnsAsync(false);

        var result = await _authService.LoginAsync(user.Email, "wrongpass");

        Assert.That(result.Success, Is.False);
        Assert.That(result.ErrorMessages.ContainsKey("BadCredentials"), Is.True);
    }
}
