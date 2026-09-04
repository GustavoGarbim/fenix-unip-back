using fenix_unip_back.DTOs;
using fenix_unip_back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fenix_unip_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class SugestoesController : ControllerBase
{
    private readonly ISugestaoService _service;

    public SugestoesController(ISugestaoService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<SugestaoResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<SugestaoResponseDto>> GetById(int id)
    {
        var sugestao = await _service.GetByIdAsync(id);
        return sugestao is null ? NotFound() : Ok(sugestao);
    }

    [HttpPost]
    public async Task<ActionResult<SugestaoResponseDto>> Create([FromBody] SugestaoCreateDto dto)
    {
        var criado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}/responder")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Responder(int id, [FromBody] SugestaoResponderDto dto)
    {
        var ok = await _service.ResponderAsync(id, dto);
        return ok ? NoContent() : NotFound();
    }
}
