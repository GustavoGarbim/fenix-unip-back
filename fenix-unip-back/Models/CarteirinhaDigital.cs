using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class CarteirinhaDigital
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    [Required, MaxLength(30)]
    public string NumeroCarteirinha { get; set; } = string.Empty;

    public DateTime DataEmissao { get; set; } = DateTime.UtcNow;

    public DateTime DataValidade { get; set; }

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Ativa";
}
