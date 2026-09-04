using fenix_unip_back.Data;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Repositories;

public class PedidoRepository : Repository<Pedido>, IPedidoRepository
{
    public PedidoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Pedido>> GetAllWithItensAsync() =>
        await DbSet.Include(p => p.Itens).ThenInclude(i => i.Produto).ToListAsync();

    public async Task<Pedido?> GetByIdWithItensAsync(int id) =>
        await DbSet.Include(p => p.Itens).ThenInclude(i => i.Produto).FirstOrDefaultAsync(p => p.Id == id);
}
