using NUnit.Framework;
using Microsoft.EntityFrameworkCore;
using System;
using System.Threading.Tasks;
using System.Linq;

[TestFixture]
public class NoteRepositoryTests
{
    private DisparoNoteDbContext _context;
    private NoteRepository _repository;

    [SetUp]
    public void Setup()
    {
        var options = new DbContextOptionsBuilder<DisparoNoteDbContext>()
            .UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString())
            .Options;

        _context = new DisparoNoteDbContext(options);
        _repository = new NoteRepository(_context);
    }

    [TearDown]
    public void TearDown()
    {
        _context.Database.EnsureDeleted();
        _context.Dispose();
    }

    [Test]
    public async Task CreateNoteAsync_SavesNoteToDatabase()
    {
        var note = new Note
        {
            Content = "Test note",
            AccessKey = "abc123",
            CreatedAt = DateTime.UtcNow
        };

        var result = await _repository.CreateNoteAsync(note);
        var savedNote = await _context.Notes.FindAsync(result.Id);

        Assert.That(savedNote, Is.Not.Null);
        Assert.That(savedNote.Content, Is.EqualTo("Test note"));
    }

    [Test]
    public async Task GetNoteByAccessKeyAsync_ValidKey_ReturnsNote()
    {
        var note = new Note
        {
            Content = "Get this",
            AccessKey = "key123"
        };
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();

        var result = await _repository.GetNoteByAccessKeyAsync("key123");

        Assert.That(result, Is.Not.Null);
        Assert.That(result.Content, Is.EqualTo("Get this"));
    }

    [Test]
    public async Task DeleteNoteAsync_RemovesNote()
    {
        var note = new Note
        {
            Content = "Delete this",
            AccessKey = "delete123"
        };
        await _context.Notes.AddAsync(note);
        await _context.SaveChangesAsync();

        await _repository.DeleteNoteAsync(note);
        var result = await _context.Notes.FindAsync(note.Id);

        Assert.That(result, Is.Null);
    }

    [Test]
    public async Task DeleteExpiredNotesAsync_RemovesOnlyExpired()
    {
        var expired = new Note
        {
            Content = "Old",
            AccessKey = "expired",
            ExpiresAt = DateTime.UtcNow.AddMinutes(-5)
        };
        var active = new Note
        {
            Content = "Fresh",
            AccessKey = "active",
            ExpiresAt = DateTime.UtcNow.AddMinutes(5)
        };

        await _context.Notes.AddRangeAsync(expired, active);
        await _context.SaveChangesAsync();

        await _repository.DeleteExpiredNotesAsync();

        var notes = await _context.Notes.ToListAsync();
        Assert.That(notes.Count, Is.EqualTo(1));
        Assert.That(notes.First().AccessKey, Is.EqualTo("active"));
    }
}
