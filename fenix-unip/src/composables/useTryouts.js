import { ref } from 'vue'
import { get, put } from '../services/api'

function mapFromApi(t) {
  return {
    id: t.id ?? t.Id,
    modalidadeId: t.modalidadeId ?? t.ModalidadeId ?? null,
    modalidadeNome: t.modalidadeNome ?? t.ModalidadeNome ?? '',
    nomeCandidato: t.nomeCandidato ?? t.NomeCandidato ?? '',
    email: t.email ?? t.Email ?? '',
    telefone: t.telefone ?? t.Telefone ?? '',
    ra: t.ra ?? t.RA ?? t.Ra ?? '',
    curso: t.curso ?? t.Curso ?? '',
    mensagem: t.mensagem ?? t.Mensagem ?? '',
    dataInscricao: t.dataInscricao ?? t.DataInscricao ?? '',
    status: t.status ?? t.Status ?? 'Pendente',
  }
}

const tryouts = ref([])

export function useTryouts() {
  async function fetchTryouts() {
    try {
      const data = await get('/tryouts')
      if (Array.isArray(data)) {
        tryouts.value = data.map(mapFromApi)
      }
    } catch {
      // mantém a lista atual em caso de falha
    }
  }

  async function updateStatus(id, status) {
    const previous = tryouts.value
    const idx = tryouts.value.findIndex((t) => t.id === id)
    if (idx !== -1) {
      tryouts.value[idx] = { ...tryouts.value[idx], status }
    }
    try {
      await put(`/tryouts/${id}/status`, { Status: status })
    } catch (err) {
      tryouts.value = previous
      throw err
    }
  }

  return { tryouts, fetchTryouts, updateStatus }
}
