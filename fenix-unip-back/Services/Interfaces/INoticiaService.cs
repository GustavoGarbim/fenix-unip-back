using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface INoticiaService
{
    Task<IEnumerable<NoticiaResponseDto>> GetAllAsync();
    Task<NoticiaResponseDto?> GetByIdAsync(int id);
    Task<NoticiaResponseDto> CreateAsync(NoticiaCreateDto dto);
    Task<bool> UpdateAsync(int id, NoticiaUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
