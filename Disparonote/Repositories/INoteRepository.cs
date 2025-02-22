using System.Collections.Generic;
using System.Threading.Tasks;

public interface INoteRepository
{
    Task<Note> CreateNoteAsync(Note note);
    Task<Note?> GetNoteByAccessKeyAsync(string accessKey);
    Task DeleteNoteAsync(Note note);
}
