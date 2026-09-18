using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class QrCodeTokenResponseDto
{
    public string Token { get; set; } = string.Empty;
    public DateTime ExpiraEm { get; set; }
    public int EventoId { get; set; }
    public string EventoTitulo { get; set; } = string.Empty;
    public DateTime EventoData { get; set; }
}

public class CheckInValidarRequestDto
{
    [Required]
    public string Token { get; set; } = string.Empty;
}

public class CheckInResultDto
{
    public bool Sucesso { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public int UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public int EventoId { get; set; }
    public string? EventoTitulo { get; set; }
    public DateTime? DataHoraCheckIn { get; set; }
    public int TotalSelos { get; set; }
}

public class CheckInResponseDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public string? UsuarioNome { get; set; }
    public int EventoId { get; set; }
    public string? EventoTitulo { get; set; }
    public DateTime? EventoData { get; set; }
    public DateTime DataHoraCheckIn { get; set; }
    public string? AdministradorNome { get; set; }
}

public class MeusSelosResponseDto
{
    public int TotalSelos { get; set; }
    public List<CheckInResponseDto> Historico { get; set; } = new();
}

public class RankingSelosItemDto
{
    public int UsuarioId { get; set; }
    public string UsuarioNome { get; set; } = string.Empty;
    public string? RA { get; set; }
    public int TotalSelos { get; set; }
}
