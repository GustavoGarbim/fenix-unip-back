using System.ComponentModel.DataAnnotations;

namespace fenix_unip_back.DTOs;

public class ItemPedidoResponseDto
{
    public int Id { get; set; }
    public int ProdutoId { get; set; }
    public string? ProdutoNome { get; set; }
    public int Quantidade { get; set; }
    public decimal PrecoUnitario { get; set; }
}

public class PedidoResponseDto
{
    public int Id { get; set; }
    public int UsuarioId { get; set; }
    public DateTime DataPedido { get; set; }
    public string Status { get; set; } = string.Empty;
    public decimal ValorTotal { get; set; }
    public List<ItemPedidoResponseDto> Itens { get; set; } = new();
}

public class ItemPedidoCreateDto
{
    [Required]
    public int ProdutoId { get; set; }

    [Required]
    public int Quantidade { get; set; }
}

public class PedidoCreateDto
{
    [Required, MinLength(1)]
    public List<ItemPedidoCreateDto> Itens { get; set; } = new();
}
