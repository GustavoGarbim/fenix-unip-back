using fenix_unip_back.DTOs;
using fenix_unip_back.Models;
using fenix_unip_back.Repositories.Interfaces;
using fenix_unip_back.Services.Interfaces;

namespace fenix_unip_back.Services;

public class EventoService : IEventoService
{
    private readonly IEventoRepository _repository;

    public EventoService(IEventoRepository repository)
    {
        _repository = repository;
    }

    public async Task<IEnumerable<EventoResponseDto>> GetAllAsync(int? modalidadeId = null)
    {
        var eventos = modalidadeId.HasValue
            ? await _repository.GetByModalidadeIdAsync(modalidadeId.Value)
            : await _repository.GetAllWithModalidadeAsync();

        return eventos.Select(ToResponseDto);
    }

    public async Task<EventoResponseDto?> GetByIdAsync(int id)
    {
        var evento = await _repository.GetByIdWithModalidadeAsync(id);
        return evento is null ? null : ToResponseDto(evento);
    }

    public async Task<EventoResponseDto> CreateAsync(EventoCreateDto dto)
    {
        var evento = new Evento
        {
            ModalidadeId = dto.ModalidadeId,
            Titulo = dto.Titulo,
            Descricao = dto.Descricao,
            DataHora = dto.DataHora,
            Local = dto.Local,
            TipoEvento = dto.TipoEvento
        };

        await _repository.AddAsync(evento);
        await _repository.SaveChangesAsync();

        var criado = await _repository.GetByIdWithModalidadeAsync(evento.Id);
        return ToResponseDto(criado!);
    }

    public async Task<bool> UpdateAsync(int id, EventoUpdateDto dto)
    {
        var evento = await _repository.GetByIdAsync(id);
        if (evento is null) return false;

        evento.ModalidadeId = dto.ModalidadeId;
        evento.Titulo = dto.Titulo;
        evento.Descricao = dto.Descricao;
        evento.DataHora = dto.DataHora;
        evento.Local = dto.Local;
        evento.TipoEvento = dto.TipoEvento;

        _repository.Update(evento);
        return await _repository.SaveChangesAsync();
    }

    public async Task<bool> DeleteAsync(int id)
    {
        var evento = await _repository.GetByIdAsync(id);
        if (evento is null) return false;

        _repository.Delete(evento);
        return await _repository.SaveChangesAsync();
    }

    private static EventoResponseDto ToResponseDto(Evento e) => new()
    {
        Id = e.Id,
        ModalidadeId = e.ModalidadeId,
        ModalidadeNome = e.Modalidade?.Nome,
        Titulo = e.Titulo,
        Descricao = e.Descricao,
        DataHora = e.DataHora,
        Local = e.Local,
        TipoEvento = e.TipoEvento
    };
}
