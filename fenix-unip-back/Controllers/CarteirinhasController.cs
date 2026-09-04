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

    [HttpGet("usuario/{usuarioId:int}")]
    public async Task<IActionResult> GetByUsuarioId(int usuarioId)
    {
        var carteirinha = await _service.GetByUsuarioIdAsync(usuarioId);
        return carteirinha is null ? NotFound() : Ok(carteirinha);
    }
}
