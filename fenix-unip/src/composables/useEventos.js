import { ref } from 'vue'
import { get, post, put, del } from '../services/api'

function mapFromApi(e) {
  const dataHora = e.dataHora ?? e.DataHora ?? null
  return {
    id: e.id ?? e.Id,
    modalidadeId: e.modalidadeId ?? e.ModalidadeId ?? null,
    modalidadeNome: e.modalidadeNome ?? e.ModalidadeNome ?? '',
    titulo: e.titulo ?? e.Titulo ?? '',
    descricao: e.descricao ?? e.Descricao ?? '',
    dataHora,
    local: e.local ?? e.Local ?? '',
    tipoEvento: e.tipoEvento ?? e.TipoEvento ?? '',
  }
}

const eventos = ref([])

async function fetchEventos() {
  try {
    const data = await get('/eventos', { auth: false })
    if (Array.isArray(data)) {
      eventos.value = data.map(mapFromApi)
    }
  } catch {
    // mantém a lista atual em caso de falha
  }
}

fetchEventos()

export function useEventos() {
  async function createEvento(form) {
    const payload = {
      ModalidadeId: Number(form.modalidadeId),
      Titulo: form.titulo,
      Descricao: form.descricao || null,
      DataHora: form.dataHora,
      Local: form.local || null,
      TipoEvento: form.tipoEvento,
    }
    const created = await post('/eventos', payload)
    eventos.value.unshift(mapFromApi(created))
  }

  async function updateEvento(id, form) {
    const payload = {
      ModalidadeId: Number(form.modalidadeId),
      Titulo: form.titulo,
      Descricao: form.descricao || null,
      DataHora: form.dataHora,
      Local: form.local || null,
      TipoEvento: form.tipoEvento,
    }
    const updated = await put(`/eventos/${id}`, payload)
    const idx = eventos.value.findIndex((e) => e.id === id)
    if (idx !== -1) {
      eventos.value[idx] = updated ? mapFromApi(updated) : { ...eventos.value[idx], ...mapFromApi(payload) }
    }
  }

  async function deleteEvento(id) {
    const previous = eventos.value
    eventos.value = eventos.value.filter((e) => e.id !== id)
    try {
      await del(`/eventos/${id}`)
    } catch (err) {
      eventos.value = previous
      throw err
    }
  }

  return { eventos, fetchEventos, createEvento, updateEvento, deleteEvento }
}
