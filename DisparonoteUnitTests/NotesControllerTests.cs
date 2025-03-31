using Moq;
using NUnit.Framework;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

[TestFixture]
public class NotesControllerTests
{
    private Mock<INoteService> _noteServiceMock;
    private NotesController _controller;

    [SetUp]
    public void SetUp()
    {
        _noteServiceMock = new Mock<INoteService>();
        _controller = new NotesController(_noteServiceMock.Object);
    }

    [Test]
    public async Task CreateNote_ReturnsAccessLink()
    {
        var createNoteDto = new CreateNoteDto { Content = "Secret", ExpiresAt = null };
        var noteDto = new NoteDto
        {
            AccessKey = "abc123",
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = null
        };

        _noteServiceMock
            .Setup(s => s.CreateNoteAsync(createNoteDto))
            .ReturnsAsync(noteDto);

        var result = await _controller.CreateNote(createNoteDto) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));

        Assert.That(result.Value!.ToString(), Does.Contain("/api/notes/abc123"));
    }


    [Test]
    public async Task GetNote_ValidAccessKey_ReturnsNoteContent()
    {
        var accessKey = "valid123";
        _noteServiceMock
            .Setup(s => s.GetNoteContentByAccessKeyAsync(accessKey))
            .ReturnsAsync("Hello World");

        var result = await _controller.GetNote(accessKey) as OkObjectResult;

        Assert.That(result, Is.Not.Null);
        Assert.That(result!.StatusCode, Is.EqualTo(200));
        Assert.That(result.Value!.ToString(), Does.Contain("Hello World"));
    }


    [Test]
    public async Task GetNote_InvalidAccessKey_ReturnsNotFound()
    {
        var accessKey = "invalid123";
        _noteServiceMock
            .Setup(s => s.GetNoteContentByAccessKeyAsync(accessKey))
            .ReturnsAsync((string?)null);

        var result = await _controller.GetNote(accessKey);

        var notFoundResult = result as NotFoundObjectResult;
        Assert.That(notFoundResult, Is.Not.Null);
        Assert.That(notFoundResult!.StatusCode, Is.EqualTo(404));
        Assert.That(notFoundResult.Value, Is.EqualTo("Note not found."));
    }
}
