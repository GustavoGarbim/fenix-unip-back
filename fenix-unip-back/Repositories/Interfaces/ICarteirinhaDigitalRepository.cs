using fenix_unip_back.Models;

namespace fenix_unip_back.Repositories.Interfaces;

public interface ICarteirinhaDigitalRepository : IRepository<CarteirinhaDigital>
{
    Task<CarteirinhaDigital?> GetByUsuarioIdAsync(int usuarioId);
}
