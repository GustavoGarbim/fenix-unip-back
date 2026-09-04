using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Noticia
{
    public int Id { get; set; }

    [MaxLength(200)]
    public string? Titulo { get; set; }

    [MaxLength(500)]
    public string? Resumo { get; set; }

    public string? Conteudo { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    public bool Destaque { get; set; }

    public int Curtidas { get; set; }

    public DateTime DataPublicacao { get; set; } = DateTime.UtcNow;

    public int? AdministradorId { get; set; }
    public Administrador? Administrador { get; set; }
}
