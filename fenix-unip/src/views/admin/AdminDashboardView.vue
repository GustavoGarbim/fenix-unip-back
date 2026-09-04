<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAdminAuth } from '../../composables/useAdminAuth'
import { useNews } from '../../composables/useNews'
import PostCard from '../../components/PostCard.vue'

const router = useRouter()
const { logout } = useAdminAuth()
const { posts, addPost, deletePost } = useNews()

const form = reactive({ summary: '', author: '' })
const imagePreview = ref(null)
const fileInput = ref(null)

function onFileChange(event) {
  const file = event.target.files?.[0]
  if (!file) return
  const reader = new FileReader()
  reader.onload = () => {
    imagePreview.value = reader.result
  }
  reader.readAsDataURL(file)
}

function clearImage() {
  imagePreview.value = null
  if (fileInput.value) fileInput.value.value = ''
}

function publish() {
  if (!form.summary.trim()) return
  addPost({ summary: form.summary, author: form.author, image: imagePreview.value })
  form.summary = ''
  form.author = ''
  clearImage()
}

function handleLogout() {
  logout()
  router.push('/admin/login')
}
</script>

<template>
  <div class="mx-auto max-w-5xl px-5 py-16 sm:px-8 lg:py-24">
    <div class="flex flex-col justify-between gap-6 sm:flex-row sm:items-end">
      <div>
        <span class="section-label">Painel Administrativo</span>
        <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
          Gerenciar <span class="text-fenix-orange">Jornal Fênix</span>
        </h1>
        <p class="mt-3 max-w-lg text-white/60">Publique fotos e comunicados que aparecerão no feed para todos os sócios.</p>
      </div>
      <button type="button" class="btn-ghost self-start !py-2.5 !text-sm" @click="handleLogout">
        <span class="btn-label">Sair</span>
      </button>
    </div>

    <div class="mt-12 grid gap-6 lg:grid-cols-[0.9fr_1.1fr]">
      <div class="card animate-fade-up p-6 sm:p-8">
        <p class="font-display text-lg tracking-wide">Nova Postagem</p>
        <form class="mt-5 space-y-4" @submit.prevent="publish">
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Foto (opcional)</label>
            <input
              ref="fileInput"
              type="file"
              accept="image/*"
              class="block w-full text-sm text-white/60 file:mr-4 file:rounded-full file:border-0 file:bg-fenix-orange/15 file:px-4 file:py-2 file:text-xs file:font-semibold file:uppercase file:tracking-wide file:text-fenix-orange hover:file:bg-fenix-orange/25"
              @change="onFileChange"
            />
            <div v-if="imagePreview" class="relative mt-3 w-32">
              <img :src="imagePreview" alt="Pré-visualização" class="aspect-square w-32 rounded-lg object-cover" />
              <button
                type="button"
                class="absolute -right-2 -top-2 flex h-6 w-6 items-center justify-center rounded-full bg-fenix-black text-white/70 ring-1 ring-white/20 transition hover:text-white"
                @click="clearImage"
              >
                <svg class="h-3.5 w-3.5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
                </svg>
              </button>
            </div>
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Legenda</label>
            <textarea
              v-model="form.summary"
              rows="4"
              required
              placeholder="Escreva a legenda da postagem..."
              class="w-full resize-none border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Assinado por (opcional)</label>
            <input
              v-model="form.author"
              type="text"
              placeholder="Diretoria Fênix"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <button type="submit" class="btn-fire w-full !py-3 !text-base"><span class="btn-label">Publicar no Jornal</span></button>
        </form>
      </div>

      <div class="animate-fade-up" style="animation-delay: 0.1s">
        <p class="font-display text-lg tracking-wide">Postagens Publicadas</p>
        <div class="mt-5 space-y-4">
          <div v-for="post in posts" :key="post.id" class="relative">
            <PostCard :post="post" />
            <button
              type="button"
              class="absolute right-4 top-4 flex h-9 w-9 items-center justify-center rounded-full bg-black/60 text-white/70 backdrop-blur transition hover:bg-red-500/20 hover:text-red-400"
              title="Excluir postagem"
              @click="deletePost(post.id)"
            >
              <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                <path stroke-linecap="round" stroke-linejoin="round" d="M6 7h12M9 7V5a1 1 0 011-1h4a1 1 0 011 1v2m-7 0v12a1 1 0 001 1h6a1 1 0 001-1V7" />
              </svg>
            </button>
          </div>

          <div v-if="posts.length === 0" class="card p-8 text-center text-sm text-white/50">
            Nenhuma postagem ainda. Publique a primeira ao lado.
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
