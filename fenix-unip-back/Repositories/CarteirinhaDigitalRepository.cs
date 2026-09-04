using fenix_unip_back.Data;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Repositories;

public class CarteirinhaDigitalRepository : Repository<CarteirinhaDigital>, ICarteirinhaDigitalRepository
{
    public CarteirinhaDigitalRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<CarteirinhaDigital?> GetByUsuarioIdAsync(int usuarioId) =>
        await DbSet.Include(c => c.Usuario).FirstOrDefaultAsync(c => c.UsuarioId == usuarioId);
}
