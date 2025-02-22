public class NoteService : INoteService
{
    private readonly INoteRepository _noteRepository;

    public NoteService(INoteRepository noteRepository)
    {
        _noteRepository = noteRepository;
    }
    public async Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto)
    {
        var note = new Note
        {
            Content = createNoteDto.Content,
            AccessKey = Guid.NewGuid().ToString("N"),
            CreatedAt = DateTime.UtcNow,
            ExpiresAt = createNoteDto.ExpiresAt?.ToUniversalTime(),
        };
        await _noteRepository.CreateNoteAsync(note);

        return new NoteDto
        {
            AccessKey = note.AccessKey,
            CreatedAt = note.CreatedAt,
            ExpiresAt = note.ExpiresAt
        };
    }
    public async Task<string?> GetNoteContentByAccessKeyAsync(string accessKey)
    {
        var note = await _noteRepository.GetNoteByAccessKeyAsync(accessKey);
        if (note == null)
        {
            return null;
        }

        if (note.ExpiresAt.HasValue && note.ExpiresAt < DateTime.UtcNow)
        {
            await _noteRepository.DeleteNoteAsync(note);
            return null;
        }

        await _noteRepository.DeleteNoteAsync(note);
        return note.Content;
    }

    public async Task DeleteNoteByAccessKeyAsync(string accessKey)
    {
        var note = await _noteRepository.GetNoteByAccessKeyAsync(accessKey);
        if (note != null)
        {
            await _noteRepository.DeleteNoteAsync(note);
        }
    }

    
}
