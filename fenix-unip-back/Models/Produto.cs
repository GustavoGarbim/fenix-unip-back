using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fenix_unip_back.Models;

public class Produto
{
    public int Id { get; set; }

    [Required, MaxLength(150)]
    public string Nome { get; set; } = string.Empty;

    [MaxLength(500)]
    public string? Descricao { get; set; }

    [Column(TypeName = "decimal(10,2)")]
    public decimal Preco { get; set; }

    [MaxLength(500)]
    public string? ImagemUrl { get; set; }

    public int Estoque { get; set; }

    [MaxLength(50)]
    public string? Categoria { get; set; }

    public bool Ativo { get; set; } = true;

    public ICollection<ItemPedido> ItensPedido { get; set; } = new List<ItemPedido>();
}
