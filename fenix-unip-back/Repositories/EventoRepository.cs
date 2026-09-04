using fenix_unip_back.Data;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Repositories;

public class EventoRepository : Repository<Evento>, IEventoRepository
{
    public EventoRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<Evento>> GetAllWithModalidadeAsync() =>
        await DbSet.Include(e => e.Modalidade).ToListAsync();

    public async Task<Evento?> GetByIdWithModalidadeAsync(int id) =>
        await DbSet.Include(e => e.Modalidade).FirstOrDefaultAsync(e => e.Id == id);

    public async Task<IEnumerable<Evento>> GetByModalidadeIdAsync(int modalidadeId) =>
        await DbSet.Include(e => e.Modalidade).Where(e => e.ModalidadeId == modalidadeId).ToListAsync();
}
