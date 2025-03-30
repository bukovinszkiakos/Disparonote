using System.ComponentModel.DataAnnotations;

public class Note
{
    [Key]
    public int Id { get; set; }

    [Required]
    public string Content { get; set; }

    [Required]
    [MaxLength(128)]
    public string AccessKey { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    public DateTime? ExpiresAt { get; set; }

    public bool IsRead { get; set; } = false;


}
