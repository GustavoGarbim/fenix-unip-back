using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class SugestaoResponseDto
{
    public int Id { get; set; }
    public string? NomeAutor { get; set; }
    public string? Email { get; set; }
    public string? Categoria { get; set; }
    public string Mensagem { get; set; } = string.Empty;
    public DateTime DataEnvio { get; set; }
    public string Status { get; set; } = string.Empty;
    public string? RespostaAdmin { get; set; }
}

public class SugestaoCreateDto
{
    [MaxLength(150)]
    public string? NomeAutor { get; set; }

    [MaxLength(150)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    [Required, MaxLength(1000)]
    public string Mensagem { get; set; } = string.Empty;
}

public class SugestaoResponderDto
{
    [Required, MaxLength(1000)]
    public string RespostaAdmin { get; set; } = string.Empty;
}
