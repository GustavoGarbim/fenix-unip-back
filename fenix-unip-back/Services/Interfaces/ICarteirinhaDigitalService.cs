using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface ICarteirinhaDigitalService
{
    Task<CarteirinhaDigitalResponseDto?> GetByUsuarioIdAsync(int usuarioId);
}
