using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class SugestaoService : ISugestaoService
{
    private readonly IRepository<Sugestao> _repository;

    public SugestaoService(IRepository<Sugestao> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<SugestaoResponseDto>> GetAllAsync()
    {
        var sugestoes = await _repository.GetAllAsync();
        return sugestoes.Select(ToResponseDto);
    }

    public async Task<SugestaoResponseDto?> GetByIdAsync(int id)
    {
        var sugestao = await _repository.GetByIdAsync(id);
        return sugestao is null ? null : ToResponseDto(sugestao);
    }

    public async Task<SugestaoResponseDto> CreateAsync(SugestaoCreateDto dto)
    {
        var sugestao = new Sugestao
        {
            NomeAutor = dto.NomeAutor,
            Email = dto.Email,
            Categoria = dto.Categoria,
            Mensagem = dto.Mensagem,
            DataEnvio = DateTime.UtcNow,
            Status = "Recebida"
        };

        await _repository.AddAsync(sugestao);
        await _repository.SaveChangesAsync();

        return ToResponseDto(sugestao);
    }

    public async Task<bool> ResponderAsync(int id, SugestaoResponderDto dto)
    {
        var sugestao = await _repository.GetByIdAsync(id);
        if (sugestao is null) return false;

        sugestao.RespostaAdmin = dto.RespostaAdmin;
        sugestao.Status = "Respondida";

        _repository.Update(sugestao);
        return await _repository.SaveChangesAsync();
    }

    private static SugestaoResponseDto ToResponseDto(Sugestao s) => new()
    {
        Id = s.Id,
        NomeAutor = s.NomeAutor,
        Email = s.Email,
        Categoria = s.Categoria,
        Mensagem = s.Mensagem,
        DataEnvio = s.DataEnvio,
        Status = s.Status,
        RespostaAdmin = s.RespostaAdmin
    };
}
