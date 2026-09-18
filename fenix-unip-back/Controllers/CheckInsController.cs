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
public class CheckInsController : ControllerBase
{
    private readonly ICheckInService _service;

    public CheckInsController(ICheckInService service)
    {
        _service = service;
    }

    private int UsuarioIdAtual =>
        int.Parse(User.FindFirst(JwtRegisteredClaimNames.Sub)?.Value
            ?? User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

    private string? RoleAtual => User.FindFirst(ClaimTypes.Role)?.Value;

    [HttpGet("meu-qrcode/{eventoId:int}")]
    public async Task<ActionResult<QrCodeTokenResponseDto>> GetMeuQrCode(int eventoId)
    {
        var dto = await _service.GerarQrCodeAsync(UsuarioIdAtual, eventoId);
        return dto is null ? NotFound() : Ok(dto);
    }

    [HttpGet("meus-selos")]
    public async Task<ActionResult<MeusSelosResponseDto>> GetMeusSelos() =>
        Ok(await _service.GetMeusSelosAsync(UsuarioIdAtual));

    [HttpPost("validar")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<CheckInResultDto>> Validar([FromBody] CheckInValidarRequestDto dto) =>
        Ok(await _service.ValidarCheckInAsync(dto.Token, UsuarioIdAtual));

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<CheckInResponseDto>>> GetAll(
        [FromQuery] int? eventoId, [FromQuery] int? usuarioId) =>
        Ok(await _service.GetAllAsync(eventoId, usuarioId));

    [HttpGet("ranking")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<RankingSelosItemDto>>> GetRanking([FromQuery] int? minimo) =>
        Ok(await _service.GetRankingAsync(minimo));
}
