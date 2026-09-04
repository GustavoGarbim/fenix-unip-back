using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace fenix_unip_back.Models;

public class Pedido
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public DateTime DataPedido { get; set; } = DateTime.UtcNow;

    [Required, MaxLength(30)]
    public string Status { get; set; } = "Pendente";

    [Column(TypeName = "decimal(10,2)")]
    public decimal ValorTotal { get; set; }

    public ICollection<ItemPedido> Itens { get; set; } = new List<ItemPedido>();
}
