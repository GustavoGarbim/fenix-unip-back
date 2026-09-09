using fenix_unip_back.DTOs;

namespace fenix_unip_back.Services.Interfaces;

public interface IAuthService
{
    Task<AuthResponseDto?> LoginUsuarioAsync(LoginRequestDto dto);
    Task<AuthResponseDto> RegisterUsuarioAsync(RegisterRequestDto dto);
    Task<AuthResponseDto?> LoginAdministradorAsync(LoginRequestDto dto);
    Task ForgotPasswordAsync(ForgotPasswordRequestDto dto);
    Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto);
}
