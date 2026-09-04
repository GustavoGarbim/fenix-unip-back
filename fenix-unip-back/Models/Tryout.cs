using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Tryout
{
    public int Id { get; set; }

    public int ModalidadeId { get; set; }
    public Modalidade? Modalidade { get; set; }

    [Required, MaxLength(150)]
    public string NomeCandidato { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefone { get; set; }

    [MaxLength(20)]
    public string? RA { get; set; }

    [MaxLength(100)]
    public string? Curso { get; set; }

    [MaxLength(1000)]
    public string? Mensagem { get; set; }

    public DateTime DataInscricao { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Pendente";
}
