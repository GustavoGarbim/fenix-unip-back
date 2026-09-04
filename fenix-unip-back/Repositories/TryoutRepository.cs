using fenix_unip_back.Data;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Repositories;

public class TryoutRepository : Repository<Tryout>, ITryoutRepository
{
    public TryoutRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Tryout>> GetAllWithModalidadeAsync() =>
        await DbSet.Include(t => t.Modalidade).ToListAsync();

    public async Task<Tryout?> GetByIdWithModalidadeAsync(int id) =>
        await DbSet.Include(t => t.Modalidade).FirstOrDefaultAsync(t => t.Id == id);
}
