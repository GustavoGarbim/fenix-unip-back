using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface ICheckInService
{
    Task<QrCodeTokenResponseDto?> GerarQrCodeAsync(int usuarioId, int eventoId);
    Task<CheckInResultDto> ValidarCheckInAsync(string token, int administradorId);
    Task<MeusSelosResponseDto> GetMeusSelosAsync(int usuarioId);
    Task<List<CheckInResponseDto>> GetAllAsync(int? eventoId, int? usuarioId);
    Task<List<RankingSelosItemDto>> GetRankingAsync(int? minimo);
}
