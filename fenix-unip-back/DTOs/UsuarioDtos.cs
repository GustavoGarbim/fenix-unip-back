using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class UsuarioResponseDto
{
    public int Id { get; set; }
    public string Nome { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string? RA { get; set; }
    public string? Curso { get; set; }
    public string? Telefone { get; set; }
    public DateTime? DataNascimento { get; set; }
    public DateTime DataCadastro { get; set; }
    public bool Ativo { get; set; }
}

public class UsuarioUpdateDto
{
    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? RA { get; set; }

    [MaxLength(100)]
    public string? Curso { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public DateTime? DataNascimento { get; set; }

    public bool Ativo { get; set; } = true;
}
