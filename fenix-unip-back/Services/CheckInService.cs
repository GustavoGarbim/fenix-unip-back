using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class CheckInService : ICheckInService
{
    private readonly ICheckInRepository _repository;
    private readonly IUsuarioRepository _usuarioRepository;
    private readonly IEventoRepository _eventoRepository;
    private readonly IQrCodeTokenService _tokenService;

    public CheckInService(
        ICheckInRepository repository,
        IUsuarioRepository usuarioRepository,
        IEventoRepository eventoRepository,
        IQrCodeTokenService tokenService)
    {
        _repository = repository;
        _usuarioRepository = usuarioRepository;
        _eventoRepository = eventoRepository;
        _tokenService = tokenService;
    }

    public async Task<QrCodeTokenResponseDto?> GerarQrCodeAsync(int usuarioId, int eventoId)
    {
        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        if (usuario is null) return null;

        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        if (evento is null) return null;

        var expiraEm = evento.DataHora > DateTime.UtcNow
            ? evento.DataHora.AddHours(8)
            : DateTime.UtcNow.AddHours(2);

        var token = _tokenService.GenerateToken(usuarioId, eventoId, expiraEm);

        return new QrCodeTokenResponseDto
        {
            Token = token,
            ExpiraEm = expiraEm,
            EventoId = evento.Id,
            EventoTitulo = evento.Titulo,
            EventoData = evento.DataHora
        };
    }

    public async Task<CheckInResultDto> ValidarCheckInAsync(string token, int administradorId)
    {
        var (valido, usuarioId, eventoId, erro) = _tokenService.ValidateToken(token);
        if (!valido)
        {
            return new CheckInResultDto
            {
                Sucesso = false,
                Mensagem = erro ?? "QR code inválido ou expirado."
            };
        }

        var usuario = await _usuarioRepository.GetByIdAsync(usuarioId);
        var evento = await _eventoRepository.GetByIdAsync(eventoId);
        if (usuario is null || evento is null)
        {
            return new CheckInResultDto
            {
                Sucesso = false,
                Mensagem = "Usuário ou evento não encontrado.",
                UsuarioId = usuarioId,
                EventoId = eventoId
            };
        }

        var existente = await _repository.GetByUsuarioEventoAsync(usuarioId, eventoId);
        if (existente is not null)
        {
            var totalExistente = (await _repository.GetByUsuarioAsync(usuarioId)).Count;

            return new CheckInResultDto
            {
                Sucesso = false,
                Mensagem = "Check-in já registrado para este evento.",
                UsuarioId = usuario.Id,
                UsuarioNome = usuario.Nome,
                EventoId = evento.Id,
                EventoTitulo = evento.Titulo,
                DataHoraCheckIn = existente.DataHoraCheckIn,
                TotalSelos = totalExistente
            };
        }

        var checkIn = new CheckIn
        {
            UsuarioId = usuarioId,
            EventoId = eventoId,
            AdministradorId = administradorId,
            DataHoraCheckIn = DateTime.UtcNow
        };

        await _repository.AddAsync(checkIn);
        await _repository.SaveChangesAsync();

        var totalSelos = (await _repository.GetByUsuarioAsync(usuarioId)).Count;

        return new CheckInResultDto
        {
            Sucesso = true,
            Mensagem = "Check-in registrado com sucesso!",
            UsuarioId = usuario.Id,
            UsuarioNome = usuario.Nome,
            EventoId = evento.Id,
            EventoTitulo = evento.Titulo,
            DataHoraCheckIn = checkIn.DataHoraCheckIn,
            TotalSelos = totalSelos
        };
    }

    public async Task<MeusSelosResponseDto> GetMeusSelosAsync(int usuarioId)
    {
        var checkIns = await _repository.GetByUsuarioAsync(usuarioId);
        var historico = checkIns.Select(MapToResponseDto).ToList();

        return new MeusSelosResponseDto
        {
            TotalSelos = historico.Count,
            Historico = historico
        };
    }

    public async Task<List<CheckInResponseDto>> GetAllAsync(int? eventoId, int? usuarioId)
    {
        var checkIns = await _repository.GetFilteredAsync(eventoId, usuarioId);
        return checkIns.Select(MapToResponseDto).ToList();
    }

    public async Task<List<RankingSelosItemDto>> GetRankingAsync(int? minimo)
    {
        var checkIns = await _repository.GetAllWithUsuarioAsync();

        var ranking = checkIns
            .Where(c => c.Usuario is not null)
            .GroupBy(c => c.Usuario!)
            .Select(g => new RankingSelosItemDto
            {
                UsuarioId = g.Key.Id,
                UsuarioNome = g.Key.Nome,
                RA = g.Key.RA,
                TotalSelos = g.Count()
            })
            .OrderByDescending(r => r.TotalSelos)
            .ToList();

        if (minimo.HasValue)
        {
            ranking = ranking.Where(r => r.TotalSelos >= minimo.Value).ToList();
        }

        return ranking;
    }

    private static CheckInResponseDto MapToResponseDto(CheckIn checkIn) => new()
    {
        Id = checkIn.Id,
        UsuarioId = checkIn.UsuarioId,
        UsuarioNome = checkIn.Usuario?.Nome,
        EventoId = checkIn.EventoId,
        EventoTitulo = checkIn.Evento?.Titulo,
        EventoData = checkIn.Evento?.DataHora,
        DataHoraCheckIn = checkIn.DataHoraCheckIn,
        AdministradorNome = checkIn.Administrador?.Nome
    };
}
