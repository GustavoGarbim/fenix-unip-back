using fenix_unip_back.Models;

namespace fenix_unip_back.Repositories.Interfaces;

public interface ICheckInRepository : IRepository<CheckIn>
{
    Task<CheckIn?> GetByUsuarioEventoAsync(int usuarioId, int eventoId);
    Task<List<CheckIn>> GetByUsuarioAsync(int usuarioId);
    Task<List<CheckIn>> GetFilteredAsync(int? eventoId, int? usuarioId);
    Task<List<CheckIn>> GetAllWithUsuarioAsync();
}
