using Microsoft.EntityFrameworkCore;

public class NoteRepository : INoteRepository
{
    private readonly DisparoNoteDbContext _context;

    public NoteRepository(DisparoNoteDbContext context)
    {
        _context = context;
    }
    public async Task<Note> CreateNoteAsync(Note note)
    {
        _context.Notes.Add(note);
        await _context.SaveChangesAsync();
        return note;
    }
    public async Task<Note?> GetNoteByAccessKeyAsync(string accessKey)
    {
        return await _context.Notes.FirstOrDefaultAsync(n => n.AccessKey == accessKey);
    }
    public async Task DeleteNoteAsync(Note note)
    {
        _context.Notes.Remove(note);
        await _context.SaveChangesAsync();
    }

}
