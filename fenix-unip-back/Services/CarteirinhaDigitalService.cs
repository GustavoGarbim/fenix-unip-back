using fenix_unip_back.DTOs;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class CarteirinhaDigitalService : ICarteirinhaDigitalService
{
    private readonly ICarteirinhaDigitalRepository _repository;

    public CarteirinhaDigitalService(ICarteirinhaDigitalRepository repository)
    {
        _repository = repository;
    }

    public async Task<CarteirinhaDigitalResponseDto?> GetByUsuarioIdAsync(int usuarioId)
    {
        var carteirinha = await _repository.GetByUsuarioIdAsync(usuarioId);
        if (carteirinha is null) return null;

        return new CarteirinhaDigitalResponseDto
        {
            Id = carteirinha.Id,
            UsuarioId = carteirinha.UsuarioId,
            UsuarioNome = carteirinha.Usuario?.Nome,
            NumeroCarteirinha = carteirinha.NumeroCarteirinha,
            DataEmissao = carteirinha.DataEmissao,
            DataValidade = carteirinha.DataValidade,
            Status = carteirinha.Status
        };
    }
}
