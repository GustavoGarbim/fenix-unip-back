using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface ITryoutService
{
    Task<IEnumerable<TryoutResponseDto>> GetAllAsync();
    Task<TryoutResponseDto?> GetByIdAsync(int id);
    Task<TryoutResponseDto> CreateAsync(TryoutCreateDto dto);
    Task<bool> UpdateStatusAsync(int id, TryoutStatusUpdateDto dto);
}
