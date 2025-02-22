using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

[Route("api/notes")]
[ApiController]
public class NotesController : ControllerBase
{
    private readonly INoteService _noteService;

    public NotesController(INoteService noteService)
    {
        _noteService = noteService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult> CreateNote([FromBody] CreateNoteDto request)
    {
        var note = await _noteService.CreateNoteAsync(request);
        return Ok(new { AccessLink = $"/api/notes/{note.AccessKey}" });
    }

    [HttpGet("{accessKey}")]
    public async Task<ActionResult> GetNote(string accessKey)
    {
        var content = await _noteService.GetNoteContentByAccessKeyAsync(accessKey);
        return content != null ? Ok(new { Content = content }) : NotFound("Note not found.");
    }
}
