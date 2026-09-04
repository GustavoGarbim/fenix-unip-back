using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class TryoutService : ITryoutService
{
    private readonly ITryoutRepository _repository;

    public TryoutService(ITryoutRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<TryoutResponseDto>> GetAllAsync()
    {
        var tryouts = await _repository.GetAllWithModalidadeAsync();
        return tryouts.Select(ToResponseDto);
    }

    public async Task<TryoutResponseDto?> GetByIdAsync(int id)
    {
        var tryout = await _repository.GetByIdWithModalidadeAsync(id);
        return tryout is null ? null : ToResponseDto(tryout);
    }

    public async Task<TryoutResponseDto> CreateAsync(TryoutCreateDto dto)
    {
        var tryout = new Tryout
        {
            ModalidadeId = dto.ModalidadeId,
            NomeCandidato = dto.NomeCandidato,
            Email = dto.Email,
            Telefone = dto.Telefone,
            RA = dto.RA,
            Curso = dto.Curso,
            Mensagem = dto.Mensagem,
            DataInscricao = DateTime.UtcNow,
            Status = "Pendente"
        };

        await _repository.AddAsync(tryout);
        await _repository.SaveChangesAsync();

        var criado = await _repository.GetByIdWithModalidadeAsync(tryout.Id);
        return ToResponseDto(criado!);
    }

    public async Task<bool> UpdateStatusAsync(int id, TryoutStatusUpdateDto dto)
    {
        var tryout = await _repository.GetByIdAsync(id);
        if (tryout is null) return false;

        tryout.Status = dto.Status;

        _repository.Update(tryout);
        return await _repository.SaveChangesAsync();
    }

    private static TryoutResponseDto ToResponseDto(Tryout t) => new()
    {
        Id = t.Id,
        ModalidadeId = t.ModalidadeId,
        ModalidadeNome = t.Modalidade?.Nome,
        NomeCandidato = t.NomeCandidato,
        Email = t.Email,
        Telefone = t.Telefone,
        RA = t.RA,
        Curso = t.Curso,
        Mensagem = t.Mensagem,
        DataInscricao = t.DataInscricao,
        Status = t.Status
    };
}
