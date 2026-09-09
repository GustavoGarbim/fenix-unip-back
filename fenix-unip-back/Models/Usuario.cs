using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Usuario
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string SenhaHash { get; set; } = string.Empty;

    [MaxLength(20)]
    public string? RA { get; set; }

    [MaxLength(100)]
    public string? Curso { get; set; }

    [MaxLength(20)]
    public string? Telefone { get; set; }

    public DateTime? DataNascimento { get; set; }

    public DateTime DataCadastro { get; set; } = DateTime.UtcNow;

    public bool Ativo { get; set; } = true;

    [MaxLength(100)]
    public string? SenhaResetToken { get; set; }

    public DateTime? SenhaResetExpiraEm { get; set; }

    public CarteirinhaDigital? CarteirinhaDigital { get; set; }
    public ICollection<Pedido> Pedidos { get; set; } = new List<Pedido>();
}
