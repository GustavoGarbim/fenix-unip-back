<script setup>
import { ref } from 'vue'
import logo from '../img/fenix-unip.jpg'
import { useDemoMode } from '../composables/useDemoMode'

const props = defineProps({
  post: { type: Object, required: true },
})

const { triggerDemo } = useDemoMode()

const liked = ref(false)

function toggleLike() {
  liked.value = !liked.value
  triggerDemo('Curtir postagem')
}

function formatDate(iso) {
  const d = new Date(iso + 'T00:00:00')
  return d.toLocaleDateString('pt-BR', { day: '2-digit', month: 'short', year: 'numeric' })
}
</script>

<template>
  <article class="card relative animate-fade-up overflow-visible">
    <span class="absolute -top-3 left-6 z-10 -skew-x-6 bg-fenix-orange px-3 py-1 text-[11px] font-display uppercase tracking-wider text-fenix-black shadow-lg">
      <span class="btn-label">Boletim Fênix</span>
    </span>

    <div class="flex items-center gap-3 border-b-2 border-white/10 p-4 pt-6">
      <div class="h-10 w-10 shrink-0 overflow-hidden border-2 border-fenix-orange/40" style="clip-path: polygon(0 0, 100% 0, 100% 85%, 85% 100%, 0 100%)">
        <img :src="logo" alt="Fênix UNIP" class="h-full w-full object-cover" />
      </div>
      <div class="min-w-0 flex-1">
        <p class="truncate font-display text-lg tracking-wide text-white">{{ post.author }}</p>
        <p class="text-xs text-white/40">{{ formatDate(post.date) }}</p>
      </div>
    </div>

    <div v-if="post.image" class="aspect-square w-full border-b-2 border-white/10 bg-black/40">
      <img :src="post.image" :alt="post.summary" class="h-full w-full object-cover" />
    </div>

    <div class="p-4">
      <p class="text-sm leading-relaxed text-white/80">{{ post.summary }}</p>
    </div>

    <div class="flex items-center justify-between border-t-2 border-white/10 px-4 py-3">
      <div class="flex items-center gap-4">
        <button type="button" class="transition hover:scale-110" :aria-pressed="liked" @click="toggleLike">
          <svg
            class="h-6 w-6 transition-colors"
            :class="liked ? 'fill-fenix-orange text-fenix-orange' : 'fill-none text-white/80'"
            viewBox="0 0 24 24"
            stroke="currentColor"
            stroke-width="1.8"
          >
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M17.66 18.66A8 8 0 116.34 7.34S7 9 9 10c0-2 .5-5 3-7 2 2 4.09 2.78 5.66 4.34A7.97 7.97 0 0120 13a7.97 7.97 0 01-2.34 5.66z"
            />
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M9.88 16.12A3 3 0 1012.02 11L12 10a5 5 0 00-3 4 3 3 0 00.88 2.12z"
            />
          </svg>
        </button>
        <button type="button" class="text-white/80 transition hover:scale-110" @click="triggerDemo('Comentar postagem')">
          <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.8">
            <path
              stroke-linecap="round"
              stroke-linejoin="round"
              d="M21 11.5a8.5 8.5 0 01-8.5 8.5 8.4 8.4 0 01-3.8-.9L3 21l1.9-5.7A8.4 8.4 0 013 11.5 8.5 8.5 0 0111.5 3 8.5 8.5 0 0121 11.5z"
            />
          </svg>
        </button>
        <button type="button" class="text-white/80 transition hover:scale-110" @click="triggerDemo('Compartilhar postagem')">
          <svg class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.8">
            <path stroke-linecap="round" stroke-linejoin="round" d="M22 2L11 13M22 2l-7 20-4-9-9-4 20-7z" />
          </svg>
        </button>
      </div>
      <p class="text-xs font-bold uppercase tracking-wide text-fenix-orange">
        {{ (post.likes || 0) + (liked ? 1 : 0) }} brasas
      </p>
    </div>
  </article>
</template>
