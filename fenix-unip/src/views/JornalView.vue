<script setup>
import { useNews } from '../composables/useNews'
import PostCard from '../components/PostCard.vue'

const { posts } = useNews()
</script>

<template>
  <div class="mx-auto max-w-xl px-5 py-16 sm:px-8 lg:py-24">
    <div class="text-center">
      <span class="section-label">Jornal Fênix</span>
      <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
        Fique por dentro da <span class="text-fenix-orange">Fênix</span>
      </h1>
      <p class="mx-auto mt-3 max-w-lg text-white/60">
        O feed oficial da atlética: comunicados, eventos e novidades publicados pela diretoria.
      </p>
    </div>

    <div class="mt-12 space-y-6">
      <PostCard v-for="post in posts" :key="post.id" :post="post" />

      <div v-if="posts.length === 0" class="card p-10 text-center text-white/50">
        Ainda não há postagens publicadas.
      </div>
    </div>
  </div>
</template>
