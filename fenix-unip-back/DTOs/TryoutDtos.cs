using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class TryoutResponseDto
{
    public int Id { get; set; }
    public int ModalidadeId { get; set; }
    public string? ModalidadeNome { get; set; }
    public string NomeCandidato { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? Telefone { get; set; }
    public string? RA { get; set; }
    public string? Curso { get; set; }
    public string? Mensagem { get; set; }
    public DateTime DataInscricao { get; set; }
    public string Status { get; set; } = string.Empty;
}

public class TryoutCreateDto
{
    [Required]
    public int ModalidadeId { get; set; }

    [Required, MaxLength(150)]
    public string NomeCandidato { get; set; } = string.Empty;

    [Required, EmailAddress, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? Telefone { get; set; }

    [MaxLength(20)]
    public string? RA { get; set; }

    [MaxLength(100)]
    public string? Curso { get; set; }

    [MaxLength(1000)]
    public string? Mensagem { get; set; }
}

public class TryoutStatusUpdateDto
{
    [Required, MaxLength(30)]
    public string Status { get; set; } = string.Empty;
}
