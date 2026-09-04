using fenix_unip_back.Models;

namespace fenix_unip_back.Repositories.Interfaces;

public interface IPedidoRepository : IRepository<Pedido>
{
    Task<IEnumerable<Pedido>> GetAllWithItensAsync();
    Task<Pedido?> GetByIdWithItensAsync(int id);
}
