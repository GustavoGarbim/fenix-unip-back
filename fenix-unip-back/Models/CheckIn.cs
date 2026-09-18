namespace fenix_unip_back.Models;

public class CheckIn
{
    public int Id { get; set; }

    public int UsuarioId { get; set; }
    public Usuario? Usuario { get; set; }

    public int EventoId { get; set; }
    public Evento? Evento { get; set; }

    public int? AdministradorId { get; set; }
    public Administrador? Administrador { get; set; }

    public DateTime DataHoraCheckIn { get; set; } = DateTime.UtcNow;
}
