using fenix_unip_back.Models;
using Microsoft.EntityFrameworkCore;

namespace fenix_unip_back.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Usuario> Usuarios => Set<Usuario>();
    public DbSet<Administrador> Administradores => Set<Administrador>();
    public DbSet<Noticia> Noticias => Set<Noticia>();
    public DbSet<Modalidade> Modalidades => Set<Modalidade>();
    public DbSet<Evento> Eventos => Set<Evento>();
    public DbSet<Tryout> Tryouts => Set<Tryout>();
    public DbSet<Produto> Produtos => Set<Produto>();
    public DbSet<Pedido> Pedidos => Set<Pedido>();
    public DbSet<ItemPedido> ItensPedido => Set<ItemPedido>();
    public DbSet<CarteirinhaDigital> CarteirinhasDigitais => Set<CarteirinhaDigital>();
    public DbSet<Sugestao> Sugestoes => Set<Sugestao>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Usuarios
        modelBuilder.Entity<Usuario>(entity =>
        {
            entity.ToTable("Usuarios");
            entity.HasIndex(u => u.Email).IsUnique();
            entity.HasIndex(u => u.RA).IsUnique().HasFilter("[RA] IS NOT NULL");
        });

        // Administradores
        modelBuilder.Entity<Administrador>(entity =>
        {
            entity.ToTable("Administradores");
            entity.HasIndex(a => a.Email).IsUnique();
        });

        // Noticias
        modelBuilder.Entity<Noticia>(entity =>
        {
            entity.ToTable("Noticias");
            entity.HasOne(n => n.Administrador)
                .WithMany(a => a.Noticias)
                .HasForeignKey(n => n.AdministradorId)
                .OnDelete(DeleteBehavior.SetNull);
        });

        // Modalidades
        modelBuilder.Entity<Modalidade>(entity =>
        {
            entity.ToTable("Modalidades");
        });

        // Eventos
        modelBuilder.Entity<Evento>(entity =>
        {
            entity.ToTable("Eventos");
            entity.HasOne(e => e.Modalidade)
                .WithMany(m => m.Eventos)
                .HasForeignKey(e => e.ModalidadeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Tryouts
        modelBuilder.Entity<Tryout>(entity =>
        {
            entity.ToTable("Tryouts");
            entity.HasOne(t => t.Modalidade)
                .WithMany(m => m.Tryouts)
                .HasForeignKey(t => t.ModalidadeId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Produtos
        modelBuilder.Entity<Produto>(entity =>
        {
            entity.ToTable("Produtos");
        });

        // Pedidos
        modelBuilder.Entity<Pedido>(entity =>
        {
            entity.ToTable("Pedidos");
            entity.HasOne(p => p.Usuario)
                .WithMany(u => u.Pedidos)
                .HasForeignKey(p => p.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // ItensPedido
        modelBuilder.Entity<ItemPedido>(entity =>
        {
            entity.ToTable("ItensPedido");
            entity.HasOne(i => i.Pedido)
                .WithMany(p => p.Itens)
                .HasForeignKey(i => i.PedidoId)
                .OnDelete(DeleteBehavior.Cascade);
            entity.HasOne(i => i.Produto)
                .WithMany(p => p.ItensPedido)
                .HasForeignKey(i => i.ProdutoId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // CarteirinhasDigitais
        modelBuilder.Entity<CarteirinhaDigital>(entity =>
        {
            entity.ToTable("CarteirinhasDigitais");
            entity.HasIndex(c => c.UsuarioId).IsUnique();
            entity.HasIndex(c => c.NumeroCarteirinha).IsUnique();
            entity.HasOne(c => c.Usuario)
                .WithOne(u => u.CarteirinhaDigital)
                .HasForeignKey<CarteirinhaDigital>(c => c.UsuarioId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Sugestoes
        modelBuilder.Entity<Sugestao>(entity =>
        {
            entity.ToTable("Sugestoes");
        });
    }
}
