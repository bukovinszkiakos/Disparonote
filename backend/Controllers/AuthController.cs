﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

[ApiController]
[Route("[controller]")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("Register")]
    public async Task<ActionResult<RegistrationResponse>> Register([FromBody] RegistrationRequest request)
    {
        if (!ModelState.IsValid)
            return BadRequest(ModelState);

        var result = await _authService.RegisterAsync(request.Email, request.Username, request.Password, "User");

        if (!result.Success)
        {
            foreach (var error in result.ErrorMessages)
            {
                ModelState.AddModelError(error.Key, error.Value);
            }
            return BadRequest(ModelState);
        }

        return CreatedAtAction(nameof(Register), new RegistrationResponse(result.Email, result.Username));
    }

    [HttpPost("Login")]
    public async Task<ActionResult<AuthResponse>> Login([FromBody] AuthRequest request)
{
    if (!ModelState.IsValid)
        return BadRequest(ModelState);

    var result = await _authService.LoginAsync(request.Email, request.Password, request.RememberMe);

    if (!result.Success)
    {
        foreach (var error in result.ErrorMessages)
        {
            ModelState.AddModelError(error.Key, error.Value);
        }
        return BadRequest(ModelState);
    }

        var rememberMe = request.RememberMe;

        Response.Cookies.Append("token", result.Token, new CookieOptions
    {
        HttpOnly = true,
        Secure = true,
        SameSite = SameSiteMode.Lax,
            Expires = rememberMe
            ? DateTime.UtcNow.AddDays(7)   // hosszabb élettartam
            : DateTime.UtcNow.AddMinutes(30) // rövid vagy session cookie
        });

    return Ok(new AuthResponse(result.Email, result.Username, result.Token));
}

    [HttpPost("Logout")]
    public IActionResult Logout()
    {
        Response.Cookies.Delete("token");
        return Ok(new { message = "Logout successful" });
    }

    [Authorize]
    [HttpGet("Me")]
    public IActionResult Me()
    {
        var user = HttpContext.User;
        if (user == null || !user.Identity.IsAuthenticated)
            return Unauthorized();

        var email = user.FindFirst(ClaimTypes.Email)?.Value;
        var username = user.FindFirst(ClaimTypes.Name)?.Value;
        var expClaim = user.FindFirst("exp")?.Value;
        long expirationUnix = 0;
        if (expClaim != null)
        {
            long.TryParse(expClaim, out expirationUnix);
        }
        return Ok(new { email, username, expirationUnix });
    }







}
