import { ref } from 'vue'
import { get, put } from '../services/api'

function mapFromApi(s) {
  return {
    id: s.id ?? s.Id,
    nomeAutor: s.nomeAutor ?? s.NomeAutor ?? '',
    email: s.email ?? s.Email ?? '',
    categoria: s.categoria ?? s.Categoria ?? '',
    mensagem: s.mensagem ?? s.Mensagem ?? '',
    dataEnvio: s.dataEnvio ?? s.DataEnvio ?? '',
    status: s.status ?? s.Status ?? '',
    respostaAdmin: s.respostaAdmin ?? s.RespostaAdmin ?? '',
  }
}

const sugestoes = ref([])

export function useSugestoes() {
  async function fetchSugestoes() {
    try {
      const data = await get('/sugestoes')
      if (Array.isArray(data)) {
        sugestoes.value = data.map(mapFromApi)
      }
    } catch {
      // mantém a lista atual em caso de falha
    }
  }

  async function responder(id, respostaAdmin) {
    const updated = await put(`/sugestoes/${id}/responder`, { RespostaAdmin: respostaAdmin })
    const idx = sugestoes.value.findIndex((s) => s.id === id)
    if (idx !== -1) {
      sugestoes.value[idx] = updated
        ? mapFromApi(updated)
        : { ...sugestoes.value[idx], respostaAdmin, status: 'Respondida' }
    }
  }

  return { sugestoes, fetchSugestoes, responder }
}
