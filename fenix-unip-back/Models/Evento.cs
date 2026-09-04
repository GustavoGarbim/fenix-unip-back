using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Evento
{
    public int Id { get; set; }

    public int ModalidadeId { get; set; }
    public Modalidade? Modalidade { get; set; }

    [Required, MaxLength(150)]
    public string Titulo { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    public DateTime DataHora { get; set; }

    [MaxLength(150)]
    public string? Local { get; set; }

    [Required, MaxLength(50)]
    public string TipoEvento { get; set; } = string.Empty;
}
