using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using fenix_unip_back.DTOs;
using fenix_unip_back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fenix_unip_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class PedidosController : ControllerBase
{
    private readonly IPedidoService _service;

    public PedidosController(IPedidoService service)
    {
        _service = service;
    }

    private int UsuarioIdAtual =>
        int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private string? RoleAtual => User.FindFirst(ClaimTypes.Role)?.Value;

    [HttpGet]
    public async Task<ActionResult<IEnumerable<PedidoResponseDto>>> GetAll()
    {
        if (RoleAtual == "Admin")
        {
            return Ok(await _service.GetAllAsync());
        }

        return Ok(await _service.GetAllByUsuarioAsync(UsuarioIdAtual));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<PedidoResponseDto>> GetById(int id)
    {
        var pedido = await _service.GetByIdAsync(id);
        if (pedido is null) return NotFound();

        if (RoleAtual != "Admin" && pedido.UsuarioId != UsuarioIdAtual)
        {
            return Forbid();
        }

        return Ok(pedido);
    }

    [HttpPost]
    public async Task<ActionResult<PedidoResponseDto>> Create([FromBody] PedidoCreateDto dto)
    {
        try
        {
            var criado = await _service.CreateAsync(dto, UsuarioIdAtual);
            return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(new { message = ex.Message });
        }
    }
}
