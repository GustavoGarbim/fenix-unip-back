using fenix_unip_back.Models;

namespace fenix_unip_back.Repositories.Interfaces;

public interface IEventoRepository : IRepository<Evento>
{
    Task<IEnumerable<Evento>> GetAllWithModalidadeAsync();
    Task<Evento?> GetByIdWithModalidadeAsync(int id);
    Task<IEnumerable<Evento>> GetByModalidadeIdAsync(int modalidadeId);
}
