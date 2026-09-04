using fenix_unip_back.Models;

namespace fenix_unip_back.Repositories.Interfaces;

public interface ITryoutRepository : IRepository<Tryout>
{
    Task<IEnumerable<Tryout>> GetAllWithModalidadeAsync();
    Task<Tryout?> GetByIdWithModalidadeAsync(int id);
}
