using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class EventoResponseDto
{
    public int Id { get; set; }
    public int ModalidadeId { get; set; }
    public string? ModalidadeNome { get; set; }
    public string Titulo { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public DateTime DataHora { get; set; }
    public string? Local { get; set; }
    public string TipoEvento { get; set; } = string.Empty;
}

public class EventoCreateDto
{
    [Required]
    public int ModalidadeId { get; set; }

    [Required, MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    [Required]
    public DateTime DataHora { get; set; }

    [MaxLength(150)]
    public string? Local { get; set; }

    [Required, MaxLength(50)]
    public string TipoEvento { get; set; } = string.Empty;
}

public class EventoUpdateDto : EventoCreateDto
{
}
