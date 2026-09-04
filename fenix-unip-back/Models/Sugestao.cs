using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Sugestao
{
    public int Id { get; set; }

    [MaxLength(150)]
    public string? NomeAutor { get; set; }

    [MaxLength(150)]
    public string? Email { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    [Required, MaxLength(1000)]
    public string Mensagem { get; set; } = string.Empty;

    public DateTime DataEnvio { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Recebida";

    [MaxLength(1000)]
    public string? RespostaAdmin { get; set; }
}
