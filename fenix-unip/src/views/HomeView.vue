<script setup>
import { RouterLink } from 'vue-router'
import logo from '../img/fenix-unip.jpg'
import heroBg from '../img/post-monster-energy.jpg'
import fenixVideo from '../img/video-fenix.mp4'
import MarqueeBanner from '../components/MarqueeBanner.vue'

const stats = [
  { value: '1.2k+', label: 'Sócios Ativos' },
  { value: '14', label: 'Modalidades' },
  { value: '38', label: 'Títulos' },
  { value: '+92%', label: 'Engajamento Digital' },
]

const pillars = [
  {
    title: 'Vista a Fênix',
    text: 'Moletons, camisas e tirantes oficiais. Garante o seu manto e mostra a que veio.',
    to: '/loja',
    icon: 'M3 3h18l-1.5 12.5a2 2 0 01-2 1.5H6.5a2 2 0 01-2-1.5L3 3zM8 3v0a4 4 0 008 0v0',
  },
  {
    title: 'Portal do Sócio',
    text: 'Carteirinha digital, mensalidade e perfil do atleta, tudo em um só lugar.',
    to: '/portal',
    icon: 'M12 15a3 3 0 100-6 3 3 0 000 6zM5.6 20a6.5 6.5 0 0112.8 0M12 3v2m0 0a9 9 0 019 9 9 9 0 11-18 0 9 9 0 019-9z',
  },
  {
    title: 'Seletivas',
    text: 'Calendário de treinos e jogos, e inscrição direta pra fazer parte da bateria.',
    to: '/esportiva',
    icon: 'M8 7V3m8 4V3m-9 8h10M5 21h14a2 2 0 002-2V7a2 2 0 00-2-2H5a2 2 0 00-2 2v12a2 2 0 002 2z',
  },
]
</script>

<template>
  <div>
    <MarqueeBanner />

    <section class="relative flex min-h-[88vh] items-center overflow-hidden border-b-2 border-fenix-red/40">
      <div class="absolute inset-0 -z-20 bg-black">
        <img :src="heroBg" alt="" class="h-full w-full object-cover opacity-50 grayscale contrast-125" />
        <div class="absolute inset-0 bg-fenix-red/60 mix-blend-multiply" />
        <div class="absolute inset-0 bg-gradient-to-t from-fenix-black via-fenix-black/60 to-fenix-black/20" />
        <div class="absolute inset-0 bg-gradient-to-r from-fenix-black via-fenix-black/40 to-transparent" />
      </div>

      <div class="relative mx-auto w-full max-w-7xl px-5 py-24 sm:px-8 lg:py-0">
        <div class="max-w-3xl animate-fade-up">
          <span class="section-label mt-8">Atlética Oficial UNIP</span>
          <h1 class="mt-6 font-display text-6xl leading-[1.15] tracking-wide sm:text-7xl lg:text-8xl">
            NASCIDOS NO FOGO,
            <br />
            <span class="text-fenix-orange">FORJADOS NA VITÓRIA.</span>
          </h1>
          <p class="mt-6 max-w-xl text-lg leading-relaxed text-white/70">
            A Fênix é raça, é integração, é rivalidade em quadra e união fora dela.
            Chega de torcer de fora: faça parte da bateria.
          </p>
          <div class="mt-10 flex flex-col gap-5 sm:flex-row">
            <RouterLink to="/portal" class="btn-fire">
              <span class="btn-label">
                Seja Fênix!
                <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M17 8l4 4m0 0l-4 4m4-4H3" />
                </svg>
              </span>
            </RouterLink>
            <RouterLink to="/loja" class="btn-ghost">
              <span class="btn-label">Vista a Fênix</span>
            </RouterLink>
          </div>

          <div class="mt-16 mb-10 grid grid-cols-2 gap-x-5 gap-y-6 sm:grid-cols-4 sm:gap-x-6">
            <div v-for="s in stats" :key="s.label">
              <p class="font-display text-4xl text-fenix-gold">{{ s.value }}</p>
              <p class="mt-1 text-xs uppercase tracking-wider text-white/50">{{ s.label }}</p>
            </div>
          </div>
        </div>
      </div>
    </section>

    <section class="mx-auto max-w-7xl px-5 py-24 sm:px-8">
      <div class="max-w-2xl">
        <span class="section-label">O Ecossistema Fênix</span>
        <h2 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
          Tudo o que a sua atlética precisa,
          <span class="text-fenix-orange">em um só lugar</span>
        </h2>
      </div>

      <div class="mt-16 grid gap-6 md:grid-cols-3">
        <RouterLink
          v-for="(p, i) in pillars"
          :key="p.title"
          :to="p.to"
          class="card group animate-fade-up flex flex-col gap-5 p-8"
          :style="{ animationDelay: `${i * 0.1}s` }"
        >
          <div class="flex h-14 w-14 items-center justify-center border-2 border-fenix-orange/40 bg-fenix-orange/10 transition-colors duration-300 group-hover:bg-fenix-orange/20">
            <svg class="h-7 w-7 text-fenix-orange" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="1.8">
              <path stroke-linecap="round" stroke-linejoin="round" :d="p.icon" />
            </svg>
          </div>
          <div>
            <h3 class="font-display text-2xl tracking-wide">{{ p.title }}</h3>
            <p class="mt-2 text-sm leading-relaxed text-white/60">{{ p.text }}</p>
          </div>
          <span class="mt-auto flex items-center gap-2 text-sm font-bold uppercase tracking-wide text-fenix-orange">
            Explorar
            <svg class="h-4 w-4 transition-transform group-hover:translate-x-1" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
              <path stroke-linecap="round" stroke-linejoin="round" d="M17 8l4 4m0 0l-4 4m4-4H3" />
            </svg>
          </span>
        </RouterLink>
      </div>
    </section>

    <section class="mx-auto max-w-7xl px-5 py-24 sm:px-8">
      <div class="grid gap-12 lg:grid-cols-[0.8fr_1.2fr] lg:items-center">
        <div class="animate-fade-up justify-self-center">
          <div class="relative w-64 -rotate-2 overflow-hidden border-4 border-white bg-black shadow-ember-lg sm:w-72">
            <video
              class="aspect-[9/16] w-full object-cover"
              :src="fenixVideo"
              autoplay
              muted
              loop
              playsinline
              controls
            />
          </div>
        </div>

        <div class="animate-fade-up text-center lg:text-left" style="animation-delay: 0.1s">
          <span class="section-label">Direto do Instagram</span>
          <h2 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
            Sinta a energia <span class="text-fenix-orange">por dentro</span>
          </h2>
          <p class="mx-auto mt-4 max-w-lg text-white/60 lg:mx-0">
            Confraternizações, jogos e bastidores: acompanhe o dia a dia da Fênix em vídeo e
            fique por dentro de tudo no nosso Jornal.
          </p>
          <RouterLink to="/jornal" class="btn-fire mt-8 inline-flex">
            <span class="btn-label">Ver Jornal Fênix</span>
          </RouterLink>
        </div>
      </div>
    </section>

    <section class="relative overflow-hidden border-y-2 border-fenix-red/40 bg-fenix-ember py-24">
      <div class="absolute -left-24 top-0 h-72 w-72 rounded-full bg-fenix-red/20 blur-[100px]" />
      <div class="absolute -right-24 bottom-0 h-72 w-72 rounded-full bg-fenix-orange/15 blur-[100px]" />
      <img :src="logo" alt="" class="pointer-events-none absolute -right-16 -top-16 h-72 w-72 rotate-12 select-none opacity-[0.07]" />
      <div class="relative mx-auto max-w-4xl px-5 text-center sm:px-8">
        <h2 class="font-display text-4xl tracking-wide sm:text-5xl">Sua jornada na Fênix começa agora</h2>
        <p class="mx-auto mt-5 max-w-xl text-lg text-white/70">
          Garanta sua carteirinha digital, acompanhe treinos e vista as cores da sua atlética.
        </p>
        <div class="mt-9 flex flex-col justify-center gap-5 sm:flex-row">
          <RouterLink to="/portal" class="btn-fire">
            <span class="btn-label">Seja Fênix!</span>
          </RouterLink>
          <RouterLink to="/esportiva" class="btn-ghost">
            <span class="btn-label">Ver Seletivas</span>
          </RouterLink>
        </div>
      </div>
    </section>
  </div>
</template>
