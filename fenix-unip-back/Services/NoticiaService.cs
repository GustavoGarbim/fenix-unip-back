using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class NoticiaService : INoticiaService
{
    private readonly IRepository<Noticia> _repository;

    public NoticiaService(IRepository<Noticia> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<NoticiaResponseDto>> GetAllAsync()
    {
        var noticias = await _repository.GetAllAsync();
        return noticias.Select(ToResponseDto);
    }

    public async Task<NoticiaResponseDto?> GetByIdAsync(int id)
    {
        var noticia = await _repository.GetByIdAsync(id);
        return noticia is null ? null : ToResponseDto(noticia);
    }

    public async Task<NoticiaResponseDto> CreateAsync(NoticiaCreateDto dto)
    {
        var noticia = new Noticia
        {
            Titulo = dto.Titulo,
            Resumo = dto.Resumo,
            Conteudo = dto.Conteudo,
            ImagemUrl = dto.ImagemUrl,
            Categoria = dto.Categoria,
            Destaque = dto.Destaque,
            AdministradorId = dto.AdministradorId,
            DataPublicacao = DateTime.UtcNow
        };

        await _repository.AddAsync(noticia);
        await _repository.SaveChangesAsync();

        return ToResponseDto(noticia);
    }

    public async Task<bool> UpdateAsync(int id, NoticiaUpdateDto dto)
    {
        var noticia = await _repository.GetByIdAsync(id);
        if (noticia is null) return false;

        noticia.Titulo = dto.Titulo;
        noticia.Resumo = dto.Resumo;
        noticia.Conteudo = dto.Conteudo;
        noticia.ImagemUrl = dto.ImagemUrl;
        noticia.Categoria = dto.Categoria;
        noticia.Destaque = dto.Destaque;
        noticia.AdministradorId = dto.AdministradorId;

        _repository.Update(noticia);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var noticia = await _repository.GetByIdAsync(id);
        if (noticia is null) return false;

        _repository.Delete(noticia);
        return await _repository.SaveChangesAsync();
    }

    private static NoticiaResponseDto ToResponseDto(Noticia n) => new()
    {
        Id = n.Id,
        Titulo = n.Titulo,
        Resumo = n.Resumo,
        Conteudo = n.Conteudo,
        ImagemUrl = n.ImagemUrl,
        Categoria = n.Categoria,
        Destaque = n.Destaque,
        Curtidas = n.Curtidas,
        DataPublicacao = n.DataPublicacao,
        AdministradorId = n.AdministradorId
    };
}
