using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class ModalidadeService : IModalidadeService
{
    private readonly IRepository<Modalidade> _repository;

    public ModalidadeService(IRepository<Modalidade> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ModalidadeResponseDto>> GetAllAsync()
    {
        var modalidades = await _repository.GetAllAsync();
        return modalidades.Select(ToResponseDto);
    }

    public async Task<ModalidadeResponseDto?> GetByIdAsync(int id)
    {
        var modalidade = await _repository.GetByIdAsync(id);
        return modalidade is null ? null : ToResponseDto(modalidade);
    }

    public async Task<ModalidadeResponseDto> CreateAsync(ModalidadeCreateDto dto)
    {
        var modalidade = new Modalidade
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Categoria = dto.Categoria,
            Tecnico = dto.Tecnico,
            ImagemUrl = dto.ImagemUrl,
            Ativo = dto.Ativo
        };

        await _repository.AddAsync(modalidade);
        await _repository.SaveChangesAsync();

        return ToResponseDto(modalidade);
    }

    public async Task<bool> UpdateAsync(int id, ModalidadeUpdateDto dto)
    {
        var modalidade = await _repository.GetByIdAsync(id);
        if (modalidade is null) return false;

        modalidade.Nome = dto.Nome;
        modalidade.Descricao = dto.Descricao;
        modalidade.Categoria = dto.Categoria;
        modalidade.Tecnico = dto.Tecnico;
        modalidade.ImagemUrl = dto.ImagemUrl;
        modalidade.Ativo = dto.Ativo;

        _repository.Update(modalidade);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var modalidade = await _repository.GetByIdAsync(id);
        if (modalidade is null) return false;

        _repository.Delete(modalidade);
        return await _repository.SaveChangesAsync();
    }

    private static ModalidadeResponseDto ToResponseDto(Modalidade m) => new()
    {
        Id = m.Id,
        Nome = m.Nome,
        Descricao = m.Descricao,
        Categoria = m.Categoria,
        Tecnico = m.Tecnico,
        ImagemUrl = m.ImagemUrl,
        Ativo = m.Ativo
    };
}
