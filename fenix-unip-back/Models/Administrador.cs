using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.Models;

public class Administrador
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [Required, MaxLength(150)]
    public string Email { get; set; } = string.Empty;

    [Required, MaxLength(256)]
    public string SenhaHash { get; set; } = string.Empty;

    [Required, MaxLength(50)]
    public string Role { get; set; } = "Admin";

    public DateTime DataCriacao { get; set; } = DateTime.UtcNow;

    public ICollection<Noticia> Noticias { get; set; } = new List<Noticia>();
}
