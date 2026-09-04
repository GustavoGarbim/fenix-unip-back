using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace fenix_unip_back.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class UsuariosController : ControllerBase
{
    private readonly IUsuarioRepository _repository;

    public UsuariosController(IUsuarioRepository repository)
    {
        _repository = repository;
    }

    [HttpGet]
    [Authorize(Roles = "Admin")]
    public async Task<ActionResult<IEnumerable<UsuarioResponseDto>>> GetAll()
    {
        var usuarios = await _repository.GetAllAsync();
        return Ok(usuarios.Select(ToResponseDto));
    }

    [HttpGet("{id:int}")]
    public async Task<ActionResult<UsuarioResponseDto>> GetById(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        return usuario is null ? NotFound() : Ok(ToResponseDto(usuario));
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] UsuarioUpdateDto dto)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario is null) return NotFound();

        usuario.Nome = dto.Nome;
        usuario.RA = dto.RA;
        usuario.Curso = dto.Curso;
        usuario.Telefone = dto.Telefone;
        usuario.DataNascimento = dto.DataNascimento;
        usuario.Ativo = dto.Ativo;

        _repository.Update(usuario);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    [HttpDelete("{id:int}")]
    [Authorize(Roles = "Admin")]
    public async Task<IActionResult> Delete(int id)
    {
        var usuario = await _repository.GetByIdAsync(id);
        if (usuario is null) return NotFound();

        _repository.Delete(usuario);
        await _repository.SaveChangesAsync();

        return NoContent();
    }

    private static UsuarioResponseDto ToResponseDto(Usuario u) => new()
    {
        Id = u.Id,
        Nome = u.Nome,
        Email = u.Email,
        RA = u.RA,
        Curso = u.Curso,
        Telefone = u.Telefone,
        DataNascimento = u.DataNascimento,
        DataCadastro = u.DataCadastro,
        Ativo = u.Ativo
    };
}
