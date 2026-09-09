using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface IPedidoService
{
    Task<IEnumerable<PedidoResponseDto>> GetAllAsync();
    Task<IEnumerable<PedidoResponseDto>> GetAllByUsuarioAsync(int usuarioId);
    Task<PedidoResponseDto?> GetByIdAsync(int id);
    Task<PedidoResponseDto> CreateAsync(PedidoCreateDto dto, int usuarioId);
}
