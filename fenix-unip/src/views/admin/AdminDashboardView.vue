<script setup>
import { ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAdminAuth } from '../../composables/useAdminAuth'
import AdminJornal from './AdminJornal.vue'
import AdminProdutos from './AdminProdutos.vue'
import AdminEventos from './AdminEventos.vue'
import AdminModalidades from './AdminModalidades.vue'
import AdminTryouts from './AdminTryouts.vue'
import AdminSugestoes from './AdminSugestoes.vue'

const router = useRouter()
const { logout } = useAdminAuth()

const tabs = [
  { id: 'jornal', label: 'Jornal', component: AdminJornal },
  { id: 'produtos', label: 'Produtos', component: AdminProdutos },
  { id: 'eventos', label: 'Eventos', component: AdminEventos },
  { id: 'modalidades', label: 'Modalidades', component: AdminModalidades },
  { id: 'seletivas', label: 'Seletivas', component: AdminTryouts },
  { id: 'sugestoes', label: 'Sugestões', component: AdminSugestoes },
]

const activeTab = ref('jornal')

function handleLogout() {
  logout()
  router.push('/admin/login')
}
</script>

<template>
  <div class="mx-auto max-w-6xl px-5 py-16 sm:px-8 lg:py-24">
    <div class="flex flex-col justify-between gap-6 sm:flex-row sm:items-end">
      <div>
        <span class="section-label">Painel Administrativo</span>
        <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
          Painel <span class="text-fenix-orange">Fênix UNIP</span>
        </h1>
        <p class="mt-3 max-w-lg text-white/60">Gerencie jornal, loja, agenda, modalidades, seletivas e sugestões dos sócios.</p>
      </div>
      <button type="button" class="btn-ghost self-start !py-2.5 !text-sm" @click="handleLogout">
        <span class="btn-label">Sair</span>
      </button>
    </div>

    <div class="mt-10 flex flex-wrap gap-3 border-b-2 border-white/10 pb-6">
      <button
        v-for="tab in tabs"
        :key="tab.id"
        type="button"
        class="-skew-x-6 border-2 px-5 py-2 text-sm font-bold uppercase tracking-wide transition-all duration-200"
        :class="
          activeTab === tab.id
            ? 'border-fenix-orange bg-fenix-orange text-fenix-black'
            : 'border-white/15 text-white/60 hover:border-white/40 hover:text-white'
        "
        @click="activeTab = tab.id"
      >
        <span class="btn-label">{{ tab.label }}</span>
      </button>
    </div>

    <div class="mt-10">
      <component :is="tabs.find((t) => t.id === activeTab)?.component" />
    </div>
  </div>
</template>
