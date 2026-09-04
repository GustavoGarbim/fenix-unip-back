<script setup>
import { RouterLink } from 'vue-router'
import logo from '../img/fenix-unip.jpg'
import MarqueeBanner from './MarqueeBanner.vue'

const year = 2026
</script>

<template>
  <footer class="border-t-2 border-fenix-red/40 bg-black">
    <MarqueeBanner />

    <div class="mx-auto max-w-7xl px-5 py-14 sm:px-8">
      <div class="grid gap-10 sm:grid-cols-2 lg:grid-cols-4">
        <div>
          <div class="flex items-center gap-2.5">
            <img :src="logo" alt="Atlética Fênix UNIP" class="h-12 w-12 object-contain" />
            <span class="font-display text-2xl tracking-wide">FÊNIX <span class="text-fenix-orange">UNIP</span></span>
          </div>
          <p class="mt-4 text-sm leading-relaxed text-white/50">
            Nascidos no fogo, forjados na vitória. A atlética oficial da UNIP.
          </p>
        </div>

        <div>
          <p class="section-label !rotate-0">Navegação</p>
          <ul class="mt-4 space-y-2.5 text-sm text-white/60">
            <li><RouterLink to="/loja" class="transition hover:text-fenix-orange">Vista a Fênix</RouterLink></li>
            <li><RouterLink to="/portal" class="transition hover:text-fenix-orange">Portal do Sócio</RouterLink></li>
            <li><RouterLink to="/esportiva" class="transition hover:text-fenix-orange">Seletivas</RouterLink></li>
            <li><RouterLink to="/jornal" class="transition hover:text-fenix-orange">Jornal Fênix</RouterLink></li>
            <li><RouterLink to="/sugestoes" class="transition hover:text-fenix-orange">Canal de Sugestões</RouterLink></li>
          </ul>
        </div>

        <div>
          <p class="section-label !rotate-0">Contato</p>
          <ul class="mt-4 space-y-2.5 text-sm text-white/60">
            <li>contato@fenixunip.com.br</li>
            <li>@unipfenix</li>
            <li>Campus UNIP, São Paulo</li>
          </ul>
        </div>

        <div>
          <p class="section-label !rotate-0">Redes Sociais</p>
          <div class="mt-4 flex gap-3">
            <a
              v-for="i in 3"
              :key="i"
              href="#"
              class="flex h-10 w-10 items-center justify-center border-2 border-white/15 text-white/60 transition hover:border-fenix-orange hover:text-fenix-orange"
            >
              <svg class="h-4 w-4" fill="currentColor" viewBox="0 0 24 24"><circle cx="12" cy="12" r="10" /></svg>
            </a>
          </div>
        </div>
      </div>

      <div class="mt-12 flex flex-col items-center justify-between gap-4 border-t-2 border-white/10 pt-6 text-xs text-white/40 sm:flex-row">
        <p>&copy; {{ year }} Atlética Fênix UNIP. Todos os direitos reservados.</p>
        <div class="flex items-center gap-4">
          <p>Demonstração visual &middot; Nenhum dado real é processado.</p>
          <RouterLink to="/admin/login" class="transition hover:text-fenix-orange">Área Administrativa</RouterLink>
        </div>
      </div>
    </div>
  </footer>
</template>
