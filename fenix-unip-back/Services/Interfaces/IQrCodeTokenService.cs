namespace fenix_unip_back.Services.Interfaces;

public interface IQrCodeTokenService
{
    string GenerateToken(int usuarioId, int eventoId, DateTime expiraEm);
    (bool Valido, int UsuarioId, int EventoId, string? Erro) ValidateToken(string token);
}
