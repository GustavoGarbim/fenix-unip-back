using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace fenix_unip_back.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAdministradorRepository _administradorRepository;
    private readonly ICarteirinhaDigitalRepository _carteirinhaDigitalRepository;
    private readonly JwtTokenGenerator _tokenGenerator;
    private readonly ILogger<AuthService> _logger;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IAdministradorRepository administradorRepository,
        ICarteirinhaDigitalRepository carteirinhaDigitalRepository,
        JwtTokenGenerator tokenGenerator,
        ILogger<AuthService> logger)
    {
        _usuarioRepository = usuarioRepository;
        _administradorRepository = administradorRepository;
        _carteirinhaDigitalRepository = carteirinhaDigitalRepository;
        _tokenGenerator = tokenGenerator;
        _logger = logger;
    }

    public async Task<AuthResponseDto?> LoginUsuarioAsync(LoginRequestDto dto)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuario is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, usuario.SenhaHash))
        {
            return null;
        }

        var (token, expiraEm) = _tokenGenerator.GenerateToken(usuario.Id, usuario.Email, "Usuario", usuario.Nome);

        return new AuthResponseDto
        {
            Id = usuario.Id,
            Token = token,
            ExpiraEm = expiraEm,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Role = "Usuario"
        };
    }

    public async Task<AuthResponseDto> RegisterUsuarioAsync(RegisterRequestDto dto)
    {
        var existente = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (existente is not null)
        {
            throw new InvalidOperationException("Já existe um usuário cadastrado com este e-mail.");
        }

        var usuario = new Usuario
        {
            Nome = dto.Nome,
            Email = dto.Email,
            SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.Senha),
            RA = dto.RA,
            Curso = dto.Curso,
            Telefone = dto.Telefone,
            DataNascimento = dto.DataNascimento,
            DataCadastro = DateTime.UtcNow,
            Ativo = true
        };

        await _usuarioRepository.AddAsync(usuario);
        await _usuarioRepository.SaveChangesAsync();

        var carteirinha = new CarteirinhaDigital
        {
            UsuarioId = usuario.Id,
            NumeroCarteirinha = $"FNX{usuario.Id:D6}",
            DataEmissao = DateTime.UtcNow,
            DataValidade = DateTime.UtcNow.AddYears(1),
            Status = "Ativa"
        };

        await _carteirinhaDigitalRepository.AddAsync(carteirinha);
        await _carteirinhaDigitalRepository.SaveChangesAsync();

        var (token, expiraEm) = _tokenGenerator.GenerateToken(usuario.Id, usuario.Email, "Usuario", usuario.Nome);

        return new AuthResponseDto
        {
            Id = usuario.Id,
            Token = token,
            ExpiraEm = expiraEm,
            Nome = usuario.Nome,
            Email = usuario.Email,
            Role = "Usuario"
        };
    }

    public async Task<AuthResponseDto?> LoginAdministradorAsync(LoginRequestDto dto)
    {
        var admin = await _administradorRepository.GetByEmailAsync(dto.Email);
        if (admin is null || !BCrypt.Net.BCrypt.Verify(dto.Senha, admin.SenhaHash))
        {
            return null;
        }

        var (token, expiraEm) = _tokenGenerator.GenerateToken(admin.Id, admin.Email, admin.Role, admin.Nome);

        return new AuthResponseDto
        {
            Id = admin.Id,
            Token = token,
            ExpiraEm = expiraEm,
            Nome = admin.Nome,
            Email = admin.Email,
            Role = admin.Role
        };
    }

    public async Task ForgotPasswordAsync(ForgotPasswordRequestDto dto)
    {
        var usuario = await _usuarioRepository.GetByEmailAsync(dto.Email);
        if (usuario is null)
        {
            // Não revela se o e-mail existe, para evitar enumeração de usuários.
            return;
        }

        var token = Guid.NewGuid().ToString("N");
        var expiraEm = DateTime.UtcNow.AddHours(1);

        usuario.SenhaResetToken = token;
        usuario.SenhaResetExpiraEm = expiraEm;

        _usuarioRepository.Update(usuario);
        await _usuarioRepository.SaveChangesAsync();

        _logger.LogInformation(
            "Link de redefinição de senha para {Email}: token={Token} (válido até {ExpiraEm})",
            usuario.Email, token, expiraEm);
    }

    public async Task<bool> ResetPasswordAsync(ResetPasswordRequestDto dto)
    {
        var usuarios = await _usuarioRepository.FindAsync(u =>
            u.SenhaResetToken == dto.Token && u.SenhaResetExpiraEm > DateTime.UtcNow);
        var usuario = usuarios.FirstOrDefault();

        if (usuario is null)
        {
            return false;
        }

        usuario.SenhaHash = BCrypt.Net.BCrypt.HashPassword(dto.NovaSenha);
        usuario.SenhaResetToken = null;
        usuario.SenhaResetExpiraEm = null;

        _usuarioRepository.Update(usuario);
        await _usuarioRepository.SaveChangesAsync();

        return true;
    }
}
