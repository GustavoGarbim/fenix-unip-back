using fenix_unip_back.DTOs;
using fenix_unip_back.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fenix_unip_back.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TryoutsController : ControllerBase
{
    private readonly ITryoutService _service;

    public TryoutsController(ITryoutService service)
    {
        _service = service;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<TryoutResponseDto>>> GetAll() =>
        Ok(await _service.GetAllAsync());

    [HttpGet("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<TryoutResponseDto>> GetById(int id)
    {
        var tryout = await _service.GetByIdAsync(id);
        return tryout is null ? NotFound() : Ok(tryout);
    }

    [HttpPost]
    public async Task<ActionResult<TryoutResponseDto>> Create([FromBody] TryoutCreateDto dto)
    {
        var criado = await _service.CreateAsync(dto);
        return CreatedAtAction(nameof(GetById), new { id = criado.Id }, criado);
    }

    [HttpPut("{id:int}/status")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> UpdateStatus(int id, [FromBody] TryoutStatusUpdateDto dto)
    {
        var ok = await _service.UpdateStatusAsync(id, dto);
        return ok ? NoContent() : NotFound();
    }
}
