using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface ISugestaoService
{
    Task<IEnumerable<SugestaoResponseDto>> GetAllAsync();
    Task<SugestaoResponseDto?> GetByIdAsync(int id);
    Task<SugestaoResponseDto> CreateAsync(SugestaoCreateDto dto);
    Task<bool> ResponderAsync(int id, SugestaoResponderDto dto);
}
