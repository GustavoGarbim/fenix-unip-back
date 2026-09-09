import { ref } from 'vue'
import { get, post, put, del } from '../services/api'

function mapFromApi(m) {
  return {
    id: m.id ?? m.Id,
    nome: m.nome ?? m.Nome ?? '',
    descricao: m.descricao ?? m.Descricao ?? '',
    categoria: m.categoria ?? m.Categoria ?? '',
    tecnico: m.tecnico ?? m.Tecnico ?? '',
    imagemUrl: m.imagemUrl ?? m.ImagemUrl ?? '',
    ativo: m.ativo ?? m.Ativo ?? true,
  }
}

const modalidades = ref([])

async function fetchModalidades() {
  try {
    const data = await get('/modalidades', { auth: false })
    if (Array.isArray(data)) {
      modalidades.value = data.map(mapFromApi)
    }
  } catch {
    // mantém a lista atual em caso de falha
  }
}

fetchModalidades()

export function useModalidades() {
  async function createModalidade(form) {
    const payload = {
      Nome: form.nome,
      Descricao: form.descricao || null,
      Categoria: form.categoria || null,
      Tecnico: form.tecnico || null,
      ImagemUrl: form.imagemUrl || null,
      Ativo: !!form.ativo,
    }
    const created = await post('/modalidades', payload)
    modalidades.value.unshift(mapFromApi(created))
  }

  async function updateModalidade(id, form) {
    const payload = {
      Nome: form.nome,
      Descricao: form.descricao || null,
      Categoria: form.categoria || null,
      Tecnico: form.tecnico || null,
      ImagemUrl: form.imagemUrl || null,
      Ativo: !!form.ativo,
    }
    const updated = await put(`/modalidades/${id}`, payload)
    const idx = modalidades.value.findIndex((m) => m.id === id)
    if (idx !== -1) {
      modalidades.value[idx] = updated ? mapFromApi(updated) : { ...modalidades.value[idx], ...mapFromApi(payload) }
    }
  }

  async function deleteModalidade(id) {
    const previous = modalidades.value
    modalidades.value = modalidades.value.filter((m) => m.id !== id)
    try {
      await del(`/modalidades/${id}`)
    } catch (err) {
      modalidades.value = previous
      throw err
    }
  }

  return { modalidades, fetchModalidades, createModalidade, updateModalidade, deleteModalidade }
}
