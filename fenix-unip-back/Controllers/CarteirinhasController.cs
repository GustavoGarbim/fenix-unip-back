using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using fenix_unip_back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fenix_unip_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class CarteirinhasController : ControllerBase
{
    private readonly ICarteirinhaDigitalService _service;

    public CarteirinhasController(ICarteirinhaDigitalService service)
    {
        _service = service;
    }

    private int UsuarioIdAtual =>
        int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private string? RoleAtual => User.FindFirst(ClaimTypes.Role)?.Value;

    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<IActionResult> GetByUsuarioId(int usuarioId)
    {
        if (RoleAtual != "Admin" && usuarioId != UsuarioIdAtual)
        {
            return Forbid();
        }

        var carteirinha = await _service.GetByUsuarioIdAsync(usuarioId);
        return carteirinha is null ? NotFound() : Ok(carteirinha);
    }
}
