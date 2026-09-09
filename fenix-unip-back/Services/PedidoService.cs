using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class PedidoService : IPedidoService
{
    private readonly IPedidoRepository _pedidoRepository;
    private readonly IRepository<Produto> _produtoRepository;

    public PedidoService(IPedidoRepository pedidoRepository, IRepository<Produto> produtoRepository)
    {
        _pedidoRepository = pedidoRepository;
        _produtoRepository = produtoRepository;
    }

    public async Task<IEnumerable<PedidoResponseDto>> GetAllAsync()
    {
        var pedidos = await _pedidoRepository.GetAllWithItensAsync();
        return pedidos.Select(ToResponseDto);
    }

    public async Task<IEnumerable<PedidoResponseDto>> GetAllByUsuarioAsync(int usuarioId)
    {
        var pedidos = await _pedidoRepository.GetAllByUsuarioIdAsync(usuarioId);
        return pedidos.Select(ToResponseDto);
    }

    public async Task<PedidoResponseDto?> GetByIdAsync(int id)
    {
        var pedido = await _pedidoRepository.GetByIdWithItensAsync(id);
        return pedido is null ? null : ToResponseDto(pedido);
    }

    public async Task<PedidoResponseDto> CreateAsync(PedidoCreateDto dto, int usuarioId)
    {
        var pedido = new Pedido
        {
            UsuarioId = usuarioId,
            DataPedido = DateTime.UtcNow,
            Status = "Pendente"
        };

        decimal valorTotal = 0;

        foreach (var itemDto in dto.Itens)
        {
            var produto = await _produtoRepository.GetByIdAsync(itemDto.ProdutoId);
            if (produto is null)
            {
                throw new InvalidOperationException($"Produto {itemDto.ProdutoId} não encontrado.");
            }

            var item = new ItemPedido
            {
                ProdutoId = produto.Id,
                Quantidade = itemDto.Quantidade,
                PrecoUnitario = produto.Preco
            };

            valorTotal += item.PrecoUnitario * item.Quantidade;
            pedido.Itens.Add(item);
        }

        pedido.ValorTotal = valorTotal;

        await _pedidoRepository.AddAsync(pedido);
        await _pedidoRepository.SaveChangesAsync();

        var criado = await _pedidoRepository.GetByIdWithItensAsync(pedido.Id);
        return ToResponseDto(criado!);
    }

    private static PedidoResponseDto ToResponseDto(Pedido p) => new()
    {
        Id = p.Id,
        UsuarioId = p.UsuarioId,
        DataPedido = p.DataPedido,
        Status = p.Status,
        ValorTotal = p.ValorTotal,
        Itens = p.Itens.Select(i => new ItemPedidoResponseDto
        {
            Id = i.Id,
            ProdutoId = i.ProdutoId,
            ProdutoNome = i.Produto?.Nome,
            Quantidade = i.Quantidade,
            PrecoUnitario = i.PrecoUnitario
        }).ToList()
    };
}
