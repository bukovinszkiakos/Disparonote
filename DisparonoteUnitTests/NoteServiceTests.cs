using NUnit.Framework;
using Moq;
using System;
using System.Threading.Tasks;

[TestFixture]
public class NoteServiceTests
{
    private Mock<INoteRepository> _noteRepositoryMock;
    private NoteService _noteService;

    [SetUp]
    public void Setup()
    {
        _noteRepositoryMock = new Mock<INoteRepository>();
        _noteService = new NoteService(_noteRepositoryMock.Object);
    }

    [Test]
    public async Task CreateNoteAsync_ShouldReturnNoteDto_WithAccessKey()
    {
        var dto = new CreateNoteDto { Content = "test content", ExpiresAt = DateTime.UtcNow.AddDays(1) };

        _noteRepositoryMock.Setup(repo => repo.CreateNoteAsync(It.IsAny<Note>()))
            .ReturnsAsync((Note note) => note);

        var result = await _noteService.CreateNoteAsync(dto);

        Assert.That(result, Is.Not.Null);
        Assert.That(result.AccessKey, Is.Not.Empty);
        Assert.That(result.CreatedAt, Is.Not.EqualTo(default(DateTime)));
        Assert.That(result.ExpiresAt, Is.EqualTo(dto.ExpiresAt).Within(TimeSpan.FromSeconds(1)));
    }

    [Test]
    public async Task GetNoteContentByAccessKeyAsync_ReturnsContent_AndDeletesNote()
    {
        var note = new Note { AccessKey = "123", Content = "secret", CreatedAt = DateTime.UtcNow };
        _noteRepositoryMock.Setup(r => r.GetNoteByAccessKeyAsync("123")).ReturnsAsync(note);

        var content = await _noteService.GetNoteContentByAccessKeyAsync("123");

        Assert.That(content, Is.EqualTo("secret"));
        _noteRepositoryMock.Verify(r => r.DeleteNoteAsync(note), Times.Once);
    }

    [Test]
    public async Task GetNoteContentByAccessKeyAsync_ReturnsNull_WhenNoteNotFound()
    {
        _noteRepositoryMock.Setup(r => r.GetNoteByAccessKeyAsync("notfound")).ReturnsAsync((Note)null);

        var result = await _noteService.GetNoteContentByAccessKeyAsync("notfound");

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task GetNoteContentByAccessKeyAsync_DeletesExpiredNote_AndReturnsNull()
    {
        var expiredNote = new Note
        {
            AccessKey = "expired",
            Content = "expired content",
            ExpiresAt = DateTime.UtcNow.AddMinutes(-10)
        };

        _noteRepositoryMock.Setup(r => r.GetNoteByAccessKeyAsync("expired")).ReturnsAsync(expiredNote);

        var result = await _noteService.GetNoteContentByAccessKeyAsync("expired");

        Assert.That(result, Is.Null);
        _noteRepositoryMock.Verify(r => r.DeleteNoteAsync(expiredNote), Times.Once);
    }
}
