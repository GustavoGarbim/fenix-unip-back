using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class AuthService : IAuthService
{
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IAdministradorRepository _administradorRepository;
    private readonly JwtTokenGenerator _tokenGenerator;

    public AuthService(
        IUsuarioRepository usuarioRepository,
        IAdministradorRepository administradorRepository,
        JwtTokenGenerator tokenGenerator)
    {
        _usuarioRepository = usuarioRepository;
        _administradorRepository = administradorRepository;
        _tokenGenerator = tokenGenerator;
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

        var (token, expiraEm) = _tokenGenerator.GenerateToken(usuario.Id, usuario.Email, "Usuario", usuario.Nome);

        return new AuthResponseDto
        {
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
            Token = token,
            ExpiraEm = expiraEm,
            Nome = admin.Nome,
            Email = admin.Email,
            Role = admin.Role
        };
    }
}
