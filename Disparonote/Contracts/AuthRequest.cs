using System.ComponentModel.DataAnnotations;

public record AuthRequest(
    [Required] string Email,
    [Required] string Password
);
