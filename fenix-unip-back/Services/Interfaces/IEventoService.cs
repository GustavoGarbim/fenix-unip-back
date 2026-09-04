using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface IEventoService
{
    Task<IEnumerable<EventoResponseDto>> GetAllAsync(int? modalidadeId = null);
    Task<EventoResponseDto?> GetByIdAsync(int id);
    Task<EventoResponseDto> CreateAsync(EventoCreateDto dto);
    Task<bool> UpdateAsync(int id, EventoUpdateDto dto);
    Task<bool> DeleteAsync(int id);
}
