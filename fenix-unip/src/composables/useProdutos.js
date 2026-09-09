import { ref } from 'vue'
import { get, post, put, del } from '../services/api'

function mapFromApi(p) {
  return {
    id: p.id ?? p.Id,
    nome: p.nome ?? p.Nome ?? '',
    descricao: p.descricao ?? p.Descricao ?? '',
    preco: Number(p.preco ?? p.Preco ?? 0),
    imagemUrl: p.imagemUrl ?? p.ImagemUrl ?? '',
    estoque: Number(p.estoque ?? p.Estoque ?? 0),
    categoria: p.categoria ?? p.Categoria ?? '',
    ativo: p.ativo ?? p.Ativo ?? true,
  }
}

const produtos = ref([])

async function fetchProdutos() {
  try {
    const data = await get('/produtos', { auth: false })
    if (Array.isArray(data)) {
      produtos.value = data.map(mapFromApi)
    }
  } catch {
    // mantém a lista atual em caso de falha
  }
}

fetchProdutos()

export function useProdutos() {
  async function createProduto(form) {
    const payload = {
      Nome: form.nome,
      Descricao: form.descricao || null,
      Preco: Number(form.preco) || 0,
      ImagemUrl: form.imagemUrl || null,
      Estoque: Number(form.estoque) || 0,
      Categoria: form.categoria || null,
      Ativo: !!form.ativo,
    }
    const created = await post('/produtos', payload)
    produtos.value.unshift(mapFromApi(created))
  }

  async function updateProduto(id, form) {
    const payload = {
      Nome: form.nome,
      Descricao: form.descricao || null,
      Preco: Number(form.preco) || 0,
      ImagemUrl: form.imagemUrl || null,
      Estoque: Number(form.estoque) || 0,
      Categoria: form.categoria || null,
      Ativo: !!form.ativo,
    }
    const updated = await put(`/produtos/${id}`, payload)
    const idx = produtos.value.findIndex((p) => p.id === id)
    if (idx !== -1) {
      produtos.value[idx] = updated ? mapFromApi(updated) : { ...produtos.value[idx], ...mapFromApi(payload) }
    }
  }

  async function deleteProduto(id) {
    const previous = produtos.value
    produtos.value = produtos.value.filter((p) => p.id !== id)
    try {
      await del(`/produtos/${id}`)
    } catch (err) {
      produtos.value = previous
      throw err
    }
  }

  return { produtos, fetchProdutos, createProduto, updateProduto, deleteProduto }
}
