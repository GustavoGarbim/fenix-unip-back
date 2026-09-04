using fenix_unip_back.DTOs;
using fenix_unip_back.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fenix_unip_back.Controllers;

[ApiController]
[Route("api/auth")]
public class AuthController : ControllerBase
{
    private readonly IAuthService _authService;

    public AuthController(IAuthService authService)
    {
        _authService = authService;
    }

    [HttpPost("login")]
    public async Task<ActionResult<AuthResponseDto>> Login([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginUsuarioAsync(dto);
        if (result is null) return Unauthorized(new { message = "E-mail ou senha inválidos." });
        return Ok(result);
    }

    [HttpPost("register")]
    public async Task<ActionResult<AuthResponseDto>> Register([FromBody] RegisterRequestDto dto)
    {
        try
        {
            var result = await _authService.RegisterUsuarioAsync(dto);
            return CreatedAtAction(nameof(Login), result);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }

    [HttpPost("admin/login")]
    public async Task<ActionResult<AuthResponseDto>> AdminLogin([FromBody] LoginRequestDto dto)
    {
        var result = await _authService.LoginAdministradorAsync(dto);
        if (result is null) return Unauthorized(new { message = "E-mail ou senha inválidos." });
        return Ok(result);
    }
}
