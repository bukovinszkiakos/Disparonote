using NUnit.Framework;
using System.Net;
using System.Net.Http.Json;
using System.Threading.Tasks;
using System.Net.Http.Headers;
using System.Collections.Generic;
using System.Linq;

[TestFixture]
public class NotesControllerIntegrationTests
{
    private HttpClient _client;

    [SetUp]
    public void Setup()
    {
        var factory = new DisparoNoteWebApplicationFactory();
        _client = factory.CreateClient();
    }

    [TearDown]
    public void TearDown()
    {
        _client?.Dispose();
    }

    private async Task<string> AuthenticateAndGetToken()
    {
        var registerRequest = new RegistrationRequest("noteuser@example.com", "noteuser", "Test123!");
        await _client.PostAsJsonAsync("/Auth/Register", registerRequest);

        var loginRequest = new AuthRequest("noteuser@example.com", "Test123!", false);
        var loginResponse = await _client.PostAsJsonAsync("/Auth/Login", loginRequest);
        var result = await loginResponse.Content.ReadFromJsonAsync<AuthResponse>();

        return result.Token;
    }

    [Test]
    public async Task CreateNote_Authorized_ReturnsAccessLink()
    {
        var token = await AuthenticateAndGetToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var noteRequest = new CreateNoteDto
        {
            Content = "Test note content",
            ExpiresAt = null
        };

        var response = await _client.PostAsJsonAsync("/api/notes", noteRequest);

        Assert.That(response.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var result = await response.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        Assert.That(result, Contains.Key("accessLink"));
        Assert.That(result["accessLink"], Is.Not.Null.And.Not.Empty);
    }

    [Test]
    public async Task GetNote_ValidAccessKey_ReturnsNoteContent()
    {
        var token = await AuthenticateAndGetToken();
        _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var noteRequest = new CreateNoteDto
        {
            Content = "Secret note content",
            ExpiresAt = null
        };

        var createResponse = await _client.PostAsJsonAsync("/api/notes", noteRequest);
        var createResult = await createResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();
        var accessLink = createResult["accessLink"];
        var accessKey = accessLink.Split("/").Last();

        var getResponse = await _client.GetAsync($"/api/notes/{accessKey}");

        Assert.That(getResponse.StatusCode, Is.EqualTo(HttpStatusCode.OK));
        var getResult = await getResponse.Content.ReadFromJsonAsync<Dictionary<string, string>>();

        Assert.That(getResult, Contains.Key("content"));
        Assert.That(getResult["content"], Is.EqualTo("Secret note content"));
    }
}
