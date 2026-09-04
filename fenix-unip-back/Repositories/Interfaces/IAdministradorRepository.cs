using fenix_unip_back.Models;

namespace fenix_unip_back.Repositories.Interfaces;

public interface IAdministradorRepository : IRepository<Administrador>
{
    Task<Administrador?> GetByEmailAsync(string email);
}
