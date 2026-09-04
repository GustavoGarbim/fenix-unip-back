using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class ModalidadeResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string? Descricao { get; set; }
    public string? Categoria { get; set; }
    public string? Tecnico { get; set; }
    public string? ImagemUrl { get; set; }
    public bool Ativo { get; set; }
}

public class ModalidadeCreateDto
{
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
}

public class ModalidadeUpdateDto : ModalidadeCreateDto
{
}
