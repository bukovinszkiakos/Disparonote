using NUnit.Framework;
using Moq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using System.Threading.Tasks;

[TestFixture]
public class AuthControllerTests
{
    private Mock<IAuthService> _authServiceMock;
    private AuthController _controller;

    [SetUp]
    public void Setup()
    {
        _authServiceMock = new Mock<IAuthService>();
        _controller = new AuthController(_authServiceMock.Object);
    }

    [Test]
    public async Task Register_ValidRequest_ReturnsCreated()
    {
        var request = new RegistrationRequest("test@example.com", "testuser", "password123");
        _authServiceMock.Setup(x => x.RegisterAsync(request.Email, request.Username, request.Password, "User"))
            .ReturnsAsync(new AuthResult(true, request.Email, request.Username, ""));

        var result = await _controller.Register(request);

        Assert.IsInstanceOf<CreatedAtActionResult>(result.Result);
        var createdResult = result.Result as CreatedAtActionResult;
        var response = createdResult.Value as RegistrationResponse;
        Assert.AreEqual(request.Email, response.Email);
        Assert.AreEqual(request.Username, response.Username);
    }

    [Test]
    public async Task Register_InvalidModelState_ReturnsBadRequest()
    {
        _controller.ModelState.AddModelError("Email", "Required");
        var request = new RegistrationRequest("", "user", "pass");

        var result = await _controller.Register(request);

        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task Login_InvalidCredentials_ReturnsBadRequest()
    {
        var request = new AuthRequest("test@example.com", "wrongpassword", false);
        var authResult = new AuthResult(false, request.Email, "", "")
        {
            ErrorMessages = { { "BadCredentials", "Invalid email or password" } }
        };

        _authServiceMock.Setup(x => x.LoginAsync(request.Email, request.Password, request.RememberMe))
            .ReturnsAsync(authResult);

        var result = await _controller.Login(request);

        Assert.IsInstanceOf<BadRequestObjectResult>(result.Result);
    }

    [Test]
    public async Task Login_ValidCredentials_ReturnsOkWithToken()
    {
        var request = new AuthRequest("test@example.com", "correctpassword", true);
        var authResult = new AuthResult(true, request.Email, "testuser", "sampletoken");

        _authServiceMock.Setup(x => x.LoginAsync(request.Email, request.Password, request.RememberMe))
            .ReturnsAsync(authResult);

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = await _controller.Login(request);

        Assert.IsInstanceOf<OkObjectResult>(result.Result);
        var okResult = result.Result as OkObjectResult;
        var response = okResult.Value as AuthResponse;
        Assert.AreEqual("sampletoken", response.Token);
    }

    [Test]
    public void Logout_ReturnsOk()
    {
        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = new DefaultHttpContext()
        };

        var result = _controller.Logout();

        Assert.IsInstanceOf<OkObjectResult>(result);
    }

    [Test]
    public void Me_UnauthorizedUser_ReturnsUnauthorized()
    {
        var mockAuthService = new Mock<IAuthService>();
        var controller = new AuthController(mockAuthService.Object);

        var httpContext = new DefaultHttpContext();

        controller.ControllerContext = new ControllerContext
        {
            HttpContext = httpContext
        };

        var result = controller.Me();

        Assert.IsInstanceOf<UnauthorizedResult>(result);
    }

    [Test]
    public void Me_AuthorizedUser_ReturnsOkWithUserData()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Email, "test@example.com"),
            new Claim(ClaimTypes.Name, "testuser"),
            new Claim("exp", ((DateTimeOffset)DateTime.UtcNow.AddMinutes(30)).ToUnixTimeSeconds().ToString())
        };
        var identity = new ClaimsIdentity(claims, "TestAuth");
        var principal = new ClaimsPrincipal(identity);

        var context = new DefaultHttpContext
        {
            User = principal
        };

        _controller.ControllerContext = new ControllerContext
        {
            HttpContext = context
        };

        var result = _controller.Me();

        Assert.IsInstanceOf<OkObjectResult>(result);
        var okResult = result as OkObjectResult;
        Assert.IsNotNull(okResult.Value);
    }
}
