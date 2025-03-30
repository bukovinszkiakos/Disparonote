
public interface INoteService
{
    Task<NoteDto> CreateNoteAsync(CreateNoteDto createNoteDto);
    Task<string?> GetNoteContentByAccessKeyAsync(string accessKey);
    Task DeleteNoteByAccessKeyAsync(string accessKey);
}
