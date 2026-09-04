<script setup>
import { ref } from 'vue'
import { RouterLink, useRoute } from 'vue-router'
import logo from '../img/fenix-unip.jpg'

const route = useRoute()
const open = ref(false)

const WHATSAPP_URL = 'https://chat.whatsapp.com/LlQaf6V5Yy3LYLoY17riut?s=sw&p=i&mlu=4'

const links = [
  { to: '/', label: 'Início' },
  { to: '/loja', label: 'Loja' },
  { to: '/portal', label: 'Portal do Sócio' },
  { to: '/esportiva', label: 'Seletivas' },
  { to: '/jornal', label: 'Jornal' },
  { to: '/sugestoes', label: 'Sugestões' },
]
</script>

<template>
  <header class="fixed inset-x-0 top-0 z-50 border-b-2 border-fenix-red/40 bg-fenix-black/90 backdrop-blur-md">
    <nav class="mx-auto flex max-w-7xl items-center justify-between px-5 py-3 sm:px-8">
      <RouterLink to="/" class="flex items-center gap-2.5" @click="open = false">
        <img :src="logo" alt="Atlética Fênix UNIP" class="h-12 w-12 object-contain drop-shadow-[0_0_12px_rgba(249,115,22,0.35)]" />
        <span class="font-display text-2xl leading-none tracking-wide">
          FÊNIX <span class="text-fenix-orange">UNIP</span>
        </span>
      </RouterLink>

      <div class="hidden items-center gap-1 lg:flex">
        <RouterLink
          v-for="link in links"
          :key="link.to"
          :to="link.to"
          class="relative px-3 py-2 font-display text-sm uppercase tracking-wide text-white/60 transition-colors duration-200 hover:text-white"
          :class="{ 'text-white': route.path === link.to }"
        >
          {{ link.label }}
          <span
            v-if="route.path === link.to"
            class="absolute inset-x-2 -bottom-[3px] h-[3px] -skew-x-6 bg-fenix-orange"
          />
        </RouterLink>
      </div>

      <div class="hidden items-center gap-3 lg:flex">
        <a
          :href="WHATSAPP_URL"
          target="_blank"
          rel="noopener noreferrer"
          class="flex h-10 w-10 items-center justify-center border-2 border-white/15 bg-white/5 text-[#25D366] transition hover:border-[#25D366]/60 hover:bg-[#25D366]/10"
          aria-label="Entrar no grupo do WhatsApp"
          title="Grupo do WhatsApp"
        >
          <svg class="h-5 w-5" fill="currentColor" viewBox="0 0 24 24">
            <path d="M12.04 2C6.58 2 2.13 6.45 2.13 11.91c0 1.75.46 3.45 1.32 4.95L2 22l5.28-1.39a9.9 9.9 0 004.76 1.21h.01c5.46 0 9.9-4.45 9.9-9.91C21.96 6.45 17.5 2 12.04 2zm0 18.1h-.01a8.2 8.2 0 01-4.19-1.15l-.3-.18-3.13.82.84-3.05-.2-.31a8.18 8.18 0 01-1.26-4.4c0-4.54 3.7-8.24 8.26-8.24 2.2 0 4.28.86 5.83 2.42a8.18 8.18 0 012.42 5.83c0 4.55-3.7 8.26-8.26 8.26zm4.52-6.19c-.25-.12-1.47-.72-1.7-.81-.23-.08-.39-.12-.56.13-.17.24-.64.81-.78.97-.14.17-.29.19-.53.06-.25-.12-1.04-.38-1.98-1.22-.73-.65-1.23-1.46-1.37-1.7-.14-.25-.01-.38.11-.5.11-.12.25-.29.37-.44.13-.15.17-.25.25-.42.08-.17.04-.31-.02-.44-.06-.12-.56-1.36-.77-1.86-.2-.48-.41-.42-.56-.43-.14-.01-.31-.01-.48-.01-.17 0-.44.06-.67.31-.23.25-.87.85-.87 2.08 0 1.23.89 2.42 1.02 2.58.12.17 1.75 2.68 4.25 3.75.6.26 1.06.41 1.42.53.6.19 1.14.16 1.57.1.48-.07 1.47-.6 1.68-1.18.21-.58.21-1.07.14-1.18-.06-.1-.23-.16-.48-.28z" />
          </svg>
        </a>
        <RouterLink to="/portal" class="btn-fire !px-5 !py-2.5 !text-sm"><span class="btn-label">Seja Fênix!</span></RouterLink>
      </div>

      <button
        type="button"
        class="flex h-10 w-10 items-center justify-center text-white lg:hidden"
        @click="open = !open"
        aria-label="Abrir menu"
      >
        <svg v-if="!open" class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M4 6h16M4 12h16M4 18h16" />
        </svg>
        <svg v-else class="h-6 w-6" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
        </svg>
      </button>
    </nav>

    <Transition name="drop">
      <div v-if="open" class="border-t-2 border-fenix-red/40 bg-fenix-black px-5 pb-6 pt-2 lg:hidden">
        <RouterLink
          v-for="link in links"
          :key="link.to"
          :to="link.to"
          class="block px-3 py-3 font-display text-base uppercase tracking-wide text-white/80 transition hover:bg-white/5 hover:text-white"
          @click="open = false"
        >
          {{ link.label }}
        </RouterLink>
        <div class="mt-3 flex flex-col gap-3">
          <a
            :href="WHATSAPP_URL"
            target="_blank"
            rel="noopener noreferrer"
            class="flex w-full items-center justify-center gap-2 border-2 border-white/15 bg-white/5 px-5 py-3 text-base font-semibold text-[#25D366] transition hover:border-[#25D366]/60 hover:bg-[#25D366]/10"
            @click="open = false"
          >
            <svg class="h-5 w-5" fill="currentColor" viewBox="0 0 24 24">
              <path d="M12.04 2C6.58 2 2.13 6.45 2.13 11.91c0 1.75.46 3.45 1.32 4.95L2 22l5.28-1.39a9.9 9.9 0 004.76 1.21h.01c5.46 0 9.9-4.45 9.9-9.91C21.96 6.45 17.5 2 12.04 2zm0 18.1h-.01a8.2 8.2 0 01-4.19-1.15l-.3-.18-3.13.82.84-3.05-.2-.31a8.18 8.18 0 01-1.26-4.4c0-4.54 3.7-8.24 8.26-8.24 2.2 0 4.28.86 5.83 2.42a8.18 8.18 0 012.42 5.83c0 4.55-3.7 8.26-8.26 8.26zm4.52-6.19c-.25-.12-1.47-.72-1.7-.81-.23-.08-.39-.12-.56.13-.17.24-.64.81-.78.97-.14.17-.29.19-.53.06-.25-.12-1.04-.38-1.98-1.22-.73-.65-1.23-1.46-1.37-1.7-.14-.25-.01-.38.11-.5.11-.12.25-.29.37-.44.13-.15.17-.25.25-.42.08-.17.04-.31-.02-.44-.06-.12-.56-1.36-.77-1.86-.2-.48-.41-.42-.56-.43-.14-.01-.31-.01-.48-.01-.17 0-.44.06-.67.31-.23.25-.87.85-.87 2.08 0 1.23.89 2.42 1.02 2.58.12.17 1.75 2.68 4.25 3.75.6.26 1.06.41 1.42.53.6.19 1.14.16 1.57.1.48-.07 1.47-.6 1.68-1.18.21-.58.21-1.07.14-1.18-.06-.1-.23-.16-.48-.28z" />
            </svg>
            Grupo do WhatsApp
          </a>
          <RouterLink to="/portal" class="btn-fire w-full !text-base" @click="open = false"><span class="btn-label">Seja Fênix!</span></RouterLink>
        </div>
      </div>
    </Transition>
  </header>
</template>

<style scoped>
.drop-enter-active,
.drop-leave-active {
  transition: all 0.25s ease;
}
.drop-enter-from,
.drop-leave-to {
  opacity: 0;
  transform: translateY(-8px);
}
</style>
