using fenix_unip_back.Data;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Repositories;

public class CheckInRepository : Repository<CheckIn>, ICheckInRepository
{
    public CheckInRepository(AppDbContext context) : base(context)
    {
    }

    public async Task<CheckIn?> GetByUsuarioEventoAsync(int usuarioId, int eventoId) =>
        await DbSet.FirstOrDefaultAsync(c => c.UsuarioId == usuarioId && c.EventoId == eventoId);

    public async Task<List<CheckIn>> GetByUsuarioAsync(int usuarioId) =>
        await DbSet
            .Include(c => c.Evento)
            .Where(c => c.UsuarioId == usuarioId)
            .OrderByDescending(c => c.DataHoraCheckIn)
            .ToListAsync();

    public async Task<List<CheckIn>> GetFilteredAsync(int? eventoId, int? usuarioId)
    {
        var query = DbSet
            .Include(c => c.Usuario)
            .Include(c => c.Evento)
            .Include(c => c.Administrador)
            .AsQueryable();

        if (eventoId.HasValue)
        {
            query = query.Where(c => c.EventoId == eventoId.Value);
        }

        if (usuarioId.HasValue)
        {
            query = query.Where(c => c.UsuarioId == usuarioId.Value);
        }

        return await query.OrderByDescending(c => c.DataHoraCheckIn).ToListAsync();
    }

    public async Task<List<CheckIn>> GetAllWithUsuarioAsync() =>
        await DbSet.Include(c => c.Usuario).ToListAsync();
}
