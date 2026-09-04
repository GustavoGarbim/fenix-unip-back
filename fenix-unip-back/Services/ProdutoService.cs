using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class ProdutoService : IProdutoService
{
    private readonly IRepository<Produto> _repository;

    public ProdutoService(IRepository<Produto> repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<ProdutoResponseDto>> GetAllAsync()
    {
        var produtos = await _repository.GetAllAsync();
        return produtos.Select(ToResponseDto);
    }

    public async Task<ProdutoResponseDto?> GetByIdAsync(int id)
    {
        var produto = await _repository.GetByIdAsync(id);
        return produto is null ? null : ToResponseDto(produto);
    }

    public async Task<ProdutoResponseDto> CreateAsync(ProdutoCreateDto dto)
    {
        var produto = new Produto
        {
            Nome = dto.Nome,
            Descricao = dto.Descricao,
            Preco = dto.Preco,
            ImagemUrl = dto.ImagemUrl,
            Estoque = dto.Estoque,
            Categoria = dto.Categoria,
            Ativo = dto.Ativo
        };

        await _repository.AddAsync(produto);
        await _repository.SaveChangesAsync();

        return ToResponseDto(produto);
    }

    public async Task<bool> UpdateAsync(int id, ProdutoUpdateDto dto)
    {
        var produto = await _repository.GetByIdAsync(id);
        if (produto is null) return false;

        produto.Nome = dto.Nome;
        produto.Descricao = dto.Descricao;
        produto.Preco = dto.Preco;
        produto.ImagemUrl = dto.ImagemUrl;
        produto.Estoque = dto.Estoque;
        produto.Categoria = dto.Categoria;
        produto.Ativo = dto.Ativo;

        _repository.Update(produto);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var produto = await _repository.GetByIdAsync(id);
        if (produto is null) return false;

        _repository.Delete(produto);
        return await _repository.SaveChangesAsync();
    }

    private static ProdutoResponseDto ToResponseDto(Produto p) => new()
    {
        Id = p.Id,
        Nome = p.Nome,
        Descricao = p.Descricao,
        Preco = p.Preco,
        ImagemUrl = p.ImagemUrl,
        Estoque = p.Estoque,
        Categoria = p.Categoria,
        Ativo = p.Ativo
    };
}
