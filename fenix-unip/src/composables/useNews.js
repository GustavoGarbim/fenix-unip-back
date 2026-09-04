import { ref } from 'vue'
import { get, post, del } from '../services/api'

function mapFromApi(n) {
  return {
    id: n.id ?? n.Id,
    title: n.titulo ?? n.Titulo ?? null,
    summary: n.resumo ?? n.Resumo ?? n.conteudo ?? n.Conteudo ?? '',
    author: n.autor ?? n.Autor ?? 'Diretoria Fênix',
    date: (n.dataPublicacao ?? n.DataPublicacao ?? '').slice(0, 10) || new Date().toISOString().slice(0, 10),
    image: n.imagemUrl ?? n.ImagemUrl ?? null,
    likes: n.curtidas ?? n.Curtidas ?? 0,
  }
}

const posts = ref([])

async function fetchPosts() {
  try {
    const data = await get('/noticias', { auth: false })
    if (Array.isArray(data)) {
      posts.value = data.map(mapFromApi).sort((a, b) => (a.date < b.date ? 1 : -1))
    }
  } catch {
    // mantém a lista atual (vazia) em caso de falha ao buscar
  }
}

fetchPosts()

export function useNews() {
  async function addPost({ summary, author, image }) {
    const payload = {
      titulo: null,
      resumo: summary,
      conteudo: summary,
      imagemUrl: image || null,
    }
    try {
      const created = await post('/noticias', payload)
      posts.value.unshift(
        mapFromApi(created) ?? {
          id: Date.now(),
          title: null,
          summary,
          author: author?.trim() || 'Diretoria Fênix',
          date: new Date().toISOString().slice(0, 10),
          image: image || null,
          likes: 0,
        }
      )
    } catch (err) {
      console.error('Falha ao publicar notícia:', err.message)
    }
  }

  async function deletePost(id) {
    const previous = posts.value
    posts.value = posts.value.filter((p) => p.id !== id)
    try {
      await del(`/noticias/${id}`)
    } catch (err) {
      posts.value = previous
      console.error('Falha ao excluir notícia:', err.message)
    }
  }

  return { posts, addPost, deletePost, fetchPosts }
}
