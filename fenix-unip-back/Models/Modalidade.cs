using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Modalidade
{
    public int Id { get; set; }

    [Required, MaxLength(100)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    [MaxLength(100)]
    public string? Tecnico { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    public bool Ativo { get; set; } = true;

    public ICollection<Evento> Eventos { get; set; } = new List<Evento>();
    public ICollection<Tryout> Tryouts { get; set; } = new List<Tryout>();
}
