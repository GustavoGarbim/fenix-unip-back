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

    [HttpPost("forgot-password")]
    public async Task<ActionResult> ForgotPassword([FromBody] ForgotPasswordRequestDto dto)
    {
        await _authService.ForgotPasswordAsync(dto);
        return Ok(new { message = "Se o e-mail existir, enviaremos instruções." });
    }

    [HttpPost("reset-password")]
    public async Task<ActionResult> ResetPassword([FromBody] ResetPasswordRequestDto dto)
    {
        var sucesso = await _authService.ResetPasswordAsync(dto);
        if (!sucesso) return BadRequest(new { message = "Token inválido ou expirado." });
        return Ok(new { message = "Senha redefinida com sucesso." });
    }
}
