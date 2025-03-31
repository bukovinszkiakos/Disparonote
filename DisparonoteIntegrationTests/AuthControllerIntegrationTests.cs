using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using FluentAssertions;
using NUnit.Framework;

[TestFixture]
public class AuthControllerIntegrationTests
{
    private DisparoNoteWebApplicationFactory _factory;
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        _factory = new DisparoNoteWebApplicationFactory();
        _client = _factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
        _factory?.Dispose();
    }

    [Test]
    public async Task Register_Should_Return_Created()
    {
        var request = new RegistrationRequest("testuser@example.com", "testuser", "Test123!");

        var response = await _client.PostAsJsonAsync("/Auth/Register", request);

        response.StatusCode.Should().Be(HttpStatusCode.Created);
    }

    [Test]
    public async Task Login_Should_Return_ValidToken()
    {
        var register = new RegistrationRequest("loginuser@example.com", "loginuser", "Test123!");
        await _client.PostAsJsonAsync("/Auth/Register", register);

        var login = new AuthRequest("loginuser@example.com", "Test123!", false);

        var response = await _client.PostAsJsonAsync("/Auth/Login", login);
        var result = await response.Content.ReadFromJsonAsync<AuthResponse>();

        response.StatusCode.Should().Be(HttpStatusCode.OK);
        result.Should().NotBeNull();
        result.Token.Should().NotBeNullOrEmpty();
    }

    [Test]
    public async Task Me_WithoutToken_Should_Return_Unauthorized()
    {
        var response = await _client.GetAsync("/Auth/Me");

        response.StatusCode.Should().Be(HttpStatusCode.Unauthorized);
    }
}
