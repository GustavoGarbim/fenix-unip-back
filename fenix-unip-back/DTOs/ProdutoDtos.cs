using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class ProdutoResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public decimal Preco { get; set; }
    public string? ImagemUrl { get; set; }
    public int Estoque { get; set; }
    public string? Categoria { get; set; }
    public bool Ativo { get; set; }
}

public class ProdutoCreateDto
{
    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    [Required]
    public decimal Preco { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    public int Estoque { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    public bool Ativo { get; set; } = true;
}

public class ProdutoUpdateDto : ProdutoCreateDto
{
}
