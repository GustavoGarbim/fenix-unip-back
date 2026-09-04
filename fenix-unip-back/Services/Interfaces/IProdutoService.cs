using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface IProdutoService
{
    Task<IEnumerable<ProdutoResponseDto>> GetAllAsync();
    Task<ProdutoResponseDto?> GetByIdAsync(int id);
    Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto);
    Task<bool> UpdateAsync(int id, ProdutoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
