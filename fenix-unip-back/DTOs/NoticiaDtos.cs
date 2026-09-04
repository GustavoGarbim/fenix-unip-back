using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class NoticiaResponseDto
{
    public int Id { get; set; }
    public string? Titulo { get; set; }
    public string? Resumo { get; set; }
    public string? Conteudo { get; set; }
    public string? ImagemUrl { get; set; }
    public string? Categoria { get; set; }
    public bool Destaque { get; set; }
    public int Curtidas { get; set; }
    public DateTime DataPublicacao { get; set; }
    public int? AdministradorId { get; set; }
}

public class NoticiaCreateDto
{
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

    public int? AdministradorId { get; set; }
}

public class NoticiaUpdateDto : NoticiaCreateDto
{
}
