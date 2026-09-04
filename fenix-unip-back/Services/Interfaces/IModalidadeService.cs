using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface IModalidadeService
{
    Task<IEnumerable<ModalidadeResponseDto>> GetAllAsync();
    Task<ModalidadeResponseDto?> GetByIdAsync(int id);
    Task<ModalidadeResponseDto> CreateAsync(ModalidadeCreateDto dto);
    Task<bool> UpdateAsync(int id, ModalidadeUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
