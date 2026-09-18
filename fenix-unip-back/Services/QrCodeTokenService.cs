using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using fenix_unip_back.Services.Interfaces;
using Microsoft.IdentityModel.Tokens;

namespace fenix_unip_back.Services;

public class QrCodeTokenService : IQrCodeTokenService
{
    private const string CheckinAudience = "FenixUnipCheckin";

    private readonly IConfiguration _configuration;

    public QrCodeTokenService(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public string GenerateToken(int usuarioId, int eventoId, DateTime expiraEm)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"]!;
        var issuer = jwtSection["Issuer"];

        var claims = new[]
        {
            new Claim("usuarioId", usuarioId.ToString()),
            new Claim("eventoId", eventoId.ToString())
        };

        var credentials = new SigningCredentials(
            new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
            SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: issuer,
            audience: CheckinAudience,
            claims: claims,
            expires: expiraEm,
            signingCredentials: credentials);

        return new JwtSecurityTokenHandler().WriteToken(token);
    }

    public (bool Valido, int UsuarioId, int EventoId, string? Erro) ValidateToken(string token)
    {
        var jwtSection = _configuration.GetSection("Jwt");
        var key = jwtSection["Key"]!;
        var issuer = jwtSection["Issuer"];

        var validationParameters = new TokenValidationParameters
        {
            ValidateIssuer = true,
            ValidateAudience = true,
            ValidateLifetime = true,
            ValidateIssuerSigningKey = true,
            ValidIssuer = issuer,
            ValidAudience = CheckinAudience,
            IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key))
        };

        try
        {
            var principal = new JwtSecurityTokenHandler().ValidateToken(token, validationParameters, out _);

            var usuarioId = int.Parse(principal.FindFirst("usuarioId")!.Value);
            var eventoId = int.Parse(principal.FindFirst("eventoId")!.Value);

            return (true, usuarioId, eventoId, null);
        }
        catch
        {
            return (false, 0, 0, "QR code inválido ou expirado.");
        }
    }
}
