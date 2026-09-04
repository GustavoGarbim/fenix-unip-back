using fenix_unip_back.Data;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Repositories;

public class AdministradorRepository : Repository<Administrador>, IAdministradorRepository
{
    public AdministradorRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<Administrador?> GetByEmailAsync(string email) =>
        await DbSet.FirstOrDefaultAsync(a => a.Email == email);
}
