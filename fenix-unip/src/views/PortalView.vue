<script setup>
import { computed, onMounted, reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import logo from '../img/fenix-unip.jpg'
import { useAuth } from '../composables/useAuth'
import { useDemoMode } from '../composables/useDemoMode'
import { get, put } from '../services/api'

const router = useRouter()
const { user, logout } = useAuth()
const { notify } = useDemoMode()

const perks = [
  'Entrada gratuita em jogos oficiais',
  'Desconto de 20% na loja virtual',
  'Prioridade em inscrições de seletivas',
  'Acesso a eventos exclusivos de sócios',
]

// ---------- Perfil + Carteirinha ----------
const loadingProfile = ref(true)
const profileError = ref('')

const profile = reactive({
  id: null,
  nome: '',
  email: '',
  ra: '',
  curso: '',
  telefone: '',
  dataNascimento: '',
  dataCadastro: '',
  ativo: true,
})

const carteirinha = reactive({
  id: null,
  numero: '',
  dataEmissao: '',
  dataValidade: '',
  status: '',
})
const carteirinhaError = ref('')

function mapUsuario(u) {
  return {
    id: u.id ?? u.Id,
    nome: u.nome ?? u.Nome ?? '',
    email: u.email ?? u.Email ?? '',
    ra: u.ra ?? u.RA ?? '',
    curso: u.curso ?? u.Curso ?? '',
    telefone: u.telefone ?? u.Telefone ?? '',
    dataNascimento: u.dataNascimento ?? u.DataNascimento ?? '',
    dataCadastro: u.dataCadastro ?? u.DataCadastro ?? '',
    ativo: u.ativo ?? u.Ativo ?? true,
  }
}

function mapCarteirinha(c) {
  return {
    id: c.id ?? c.Id,
    numero: c.numeroCarteirinha ?? c.NumeroCarteirinha ?? '',
    dataEmissao: c.dataEmissao ?? c.DataEmissao ?? '',
    dataValidade: c.dataValidade ?? c.DataValidade ?? '',
    status: c.status ?? c.Status ?? 'Ativa',
  }
}

function formatDate(value) {
  if (!value) return '—'
  const d = new Date(value)
  if (Number.isNaN(d.getTime())) return '—'
  return d.toLocaleDateString('pt-BR')
}

function formatMonthYear(value) {
  if (!value) return '—'
  const d = new Date(value)
  if (Number.isNaN(d.getTime())) return '—'
  return d.toLocaleDateString('pt-BR', { month: 'long', year: 'numeric' })
}

function toDateInputValue(value) {
  if (!value) return ''
  const d = new Date(value)
  if (Number.isNaN(d.getTime())) return ''
  return d.toISOString().slice(0, 10)
}

async function loadPortalData() {
  const userId = user.value?.id
  if (!userId) {
    profileError.value = 'Sessão inválida. Faça login novamente.'
    loadingProfile.value = false
    return
  }

  loadingProfile.value = true
  profileError.value = ''
  carteirinhaError.value = ''

  const [usuarioResult, carteirinhaResult] = await Promise.allSettled([
    get(`/usuarios/${userId}`),
    get(`/carteirinhas/usuario/${userId}`),
  ])

  if (usuarioResult.status === 'fulfilled') {
    Object.assign(profile, mapUsuario(usuarioResult.value))
  } else {
    profileError.value = 'Não foi possível carregar seus dados. Tente novamente mais tarde.'
  }

  if (carteirinhaResult.status === 'fulfilled') {
    Object.assign(carteirinha, mapCarteirinha(carteirinhaResult.value))
  } else {
    carteirinhaError.value = 'Carteirinha não encontrada.'
  }

  loadingProfile.value = false
}

const statusBadgeClasses = computed(() => {
  const status = (carteirinha.status || '').toLowerCase()
  if (status === 'ativa') return 'border-emerald-500/30 bg-emerald-500/10 text-emerald-400'
  if (status === 'pendente') return 'border-fenix-gold/30 bg-fenix-gold/10 text-fenix-gold'
  return 'border-white/15 bg-white/5 text-white/60'
})

// ---------- Baixar carteirinha (impressão) ----------
function baixarCarteirinha() {
  window.print()
}

// ---------- Editar Perfil ----------
const showEditForm = ref(false)
const savingProfile = ref(false)
const editForm = reactive({
  nome: '',
  ra: '',
  curso: '',
  telefone: '',
  dataNascimento: '',
})

function openEditForm() {
  editForm.nome = profile.nome
  editForm.ra = profile.ra
  editForm.curso = profile.curso
  editForm.telefone = profile.telefone
  editForm.dataNascimento = toDateInputValue(profile.dataNascimento)
  showEditForm.value = true
}

function closeEditForm() {
  showEditForm.value = false
}

async function saveProfile() {
  if (!profile.id) return
  savingProfile.value = true
  try {
    await put(`/usuarios/${profile.id}`, {
      Nome: editForm.nome,
      RA: editForm.ra,
      Curso: editForm.curso,
      Telefone: editForm.telefone,
      DataNascimento: editForm.dataNascimento || null,
      Ativo: profile.ativo,
    })
    profile.nome = editForm.nome
    profile.ra = editForm.ra
    profile.curso = editForm.curso
    profile.telefone = editForm.telefone
    profile.dataNascimento = editForm.dataNascimento
    showEditForm.value = false
    notify({
      title: 'Perfil atualizado',
      message: 'Seus dados foram salvos com sucesso.',
    })
  } catch (err) {
    notify({
      title: 'Não foi possível salvar',
      message: err.message || 'Ocorreu um erro ao atualizar seu perfil. Tente novamente.',
    })
  } finally {
    savingProfile.value = false
  }
}

// ---------- Histórico de pedidos ----------
const showHistorico = ref(false)
const loadingPedidos = ref(false)
const pedidosError = ref('')
const pedidos = ref([])

function mapPedido(p) {
  return {
    id: p.id ?? p.Id,
    dataPedido: p.dataPedido ?? p.DataPedido ?? '',
    status: p.status ?? p.Status ?? '',
    valorTotal: p.valorTotal ?? p.ValorTotal ?? 0,
    itens: (p.itens ?? p.Itens ?? []).map((it) => ({
      id: it.id ?? it.Id,
      produtoNome: it.produtoNome ?? it.ProdutoNome ?? '',
      quantidade: it.quantidade ?? it.Quantidade ?? 0,
      precoUnitario: it.precoUnitario ?? it.PrecoUnitario ?? 0,
    })),
  }
}

async function toggleHistorico() {
  showHistorico.value = !showHistorico.value
  if (showHistorico.value && pedidos.value.length === 0 && !loadingPedidos.value) {
    loadingPedidos.value = true
    pedidosError.value = ''
    try {
      const data = await get('/pedidos')
      pedidos.value = Array.isArray(data) ? data.map(mapPedido) : []
    } catch (err) {
      pedidosError.value = err.message || 'Não foi possível carregar seu histórico de pedidos.'
    } finally {
      loadingPedidos.value = false
    }
  }
}

function formatCurrency(value) {
  return Number(value || 0).toLocaleString('pt-BR', { style: 'currency', currency: 'BRL' })
}

// ---------- Suporte financeiro ----------
function abrirSuporteFinanceiro() {
  notify({
    title: 'Pagamento indisponível',
    message:
      'Nenhum método de pagamento foi configurado ainda. Entre em contato com o suporte da Fênix para regularizar sua situação.',
  })
}

// ---------- Trocar conta ----------
function trocarConta() {
  logout()
  router.push('/login')
}

onMounted(loadPortalData)
</script>

<template>
  <div class="mx-auto max-w-7xl px-5 py-16 sm:px-8 lg:py-24">
    <div class="print-hide">
      <span class="section-label">Portal do Sócio</span>
      <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
        Sua conexão com a <span class="text-fenix-orange">Fênix</span>
      </h1>
      <p class="mt-3 max-w-lg text-white/60">Perfil, associação e carteirinha digital em um dashboard exclusivo para sócios.</p>

      <p v-if="profileError" class="mt-6 border-2 border-fenix-red/40 bg-fenix-red/10 px-4 py-3 text-sm text-fenix-red">
        {{ profileError }}
      </p>
    </div>

    <div class="mt-12 grid gap-6 lg:grid-cols-[1.1fr_0.9fr]">
      <div class="animate-fade-up">
        <div id="carteirinha-card" class="relative overflow-hidden border-4 border-black bg-fire-gradient p-8 shadow-ember-lg">
          <div class="absolute -right-10 -top-10 h-48 w-48 rounded-full bg-white/10 blur-2xl" />
          <div class="absolute -bottom-16 -left-10 h-56 w-56 rounded-full bg-black/20 blur-3xl" />

          <div class="relative flex items-start justify-between">
            <div class="flex items-center gap-3">
              <img :src="logo" alt="Fênix UNIP" class="h-12 w-12 shrink-0 rounded-full object-contain ring-2 ring-white/40" />
              <div>
                <p class="text-xs font-semibold uppercase tracking-[0.25em] text-white/80">Carteirinha Digital</p>
                <p class="mt-1 font-display text-2xl tracking-wide">FÊNIX UNIP</p>
              </div>
            </div>
            <span class="-rotate-3 border-2 border-white/50 bg-white/15 px-3 py-1 text-xs font-bold uppercase tracking-wide backdrop-blur">
              {{ loadingProfile ? '...' : (carteirinha.status || '—') }}
            </span>
          </div>

          <div class="relative mt-10 flex items-end justify-between gap-6">
            <div>
              <p class="font-display text-2xl tracking-wide">{{ loadingProfile ? 'Carregando...' : (profile.nome || '—') }}</p>
              <p class="mt-1 text-sm text-white/85">RA {{ loadingProfile ? '...' : (profile.ra || '—') }}</p>
              <p class="text-sm text-white/85">{{ loadingProfile ? '' : (profile.curso || '—') }}</p>
              <p class="mt-4 text-xs uppercase tracking-wider text-white/70">
                Carteirinha {{ carteirinha.numero || '—' }} &middot; sócio desde {{ formatMonthYear(profile.dataCadastro) }}
              </p>
              <p v-if="carteirinhaError" class="mt-1 text-xs text-white/70">{{ carteirinhaError }}</p>
            </div>

            <div class="flex h-24 w-24 shrink-0 items-center justify-center border-2 border-black bg-white p-2 shadow-lg">
              <svg viewBox="0 0 100 100" class="h-full w-full text-fenix-black">
                <rect x="4" y="4" width="26" height="26" fill="none" stroke="currentColor" stroke-width="6" />
                <rect x="70" y="4" width="26" height="26" fill="none" stroke="currentColor" stroke-width="6" />
                <rect x="4" y="70" width="26" height="26" fill="none" stroke="currentColor" stroke-width="6" />
                <rect x="12" y="12" width="10" height="10" fill="currentColor" />
                <rect x="78" y="12" width="10" height="10" fill="currentColor" />
                <rect x="12" y="78" width="10" height="10" fill="currentColor" />
                <rect x="40" y="10" width="8" height="8" fill="currentColor" />
                <rect x="54" y="10" width="8" height="8" fill="currentColor" />
                <rect x="40" y="40" width="8" height="8" fill="currentColor" />
                <rect x="52" y="40" width="8" height="8" fill="currentColor" />
                <rect x="64" y="40" width="8" height="8" fill="currentColor" />
                <rect x="40" y="52" width="8" height="8" fill="currentColor" />
                <rect x="64" y="52" width="8" height="8" fill="currentColor" />
                <rect x="40" y="64" width="8" height="8" fill="currentColor" />
                <rect x="52" y="64" width="8" height="8" fill="currentColor" />
                <rect x="76" y="52" width="8" height="8" fill="currentColor" />
                <rect x="76" y="76" width="18" height="8" fill="currentColor" />
                <rect x="52" y="88" width="8" height="8" fill="currentColor" />
              </svg>
            </div>
          </div>
        </div>

        <div class="card mt-6 p-6 print-hide">
          <p class="font-display text-lg tracking-wide">Benefícios do Plano</p>
          <ul class="mt-4 space-y-3">
            <li v-for="perk in perks" :key="perk" class="flex items-start gap-3 text-sm text-white/70">
              <svg class="mt-0.5 h-5 w-5 shrink-0 text-fenix-orange" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
                <path stroke-linecap="round" stroke-linejoin="round" d="M5 13l4 4L19 7" />
              </svg>
              {{ perk }}
            </li>
          </ul>
        </div>
      </div>

      <div class="print-hide animate-fade-up flex flex-col gap-6" style="animation-delay: 0.1s">
        <!-- Status da Associação -->
        <div class="card p-6">
          <div class="flex items-center justify-between">
            <p class="font-display text-lg tracking-wide">Status da Associação</p>
            <span class="border-2 px-3 py-1 text-xs font-bold uppercase" :class="statusBadgeClasses">
              {{ loadingProfile ? '...' : (carteirinha.status || 'Indisponível') }}
            </span>
          </div>
          <div class="mt-6 space-y-3 text-sm text-white/70">
            <p>Número da carteirinha: <span class="text-white">{{ carteirinha.numero || '—' }}</span></p>
            <p>Emitida em: <span class="text-white">{{ formatDate(carteirinha.dataEmissao) }}</span></p>
            <p>Válida até: <span class="text-white">{{ formatDate(carteirinha.dataValidade) }}</span></p>
          </div>
          <button type="button" class="btn-fire mt-6 w-full !py-3 !text-base" @click="abrirSuporteFinanceiro">
            <span class="btn-label">Regularizar Associação</span>
          </button>
        </div>

        <!-- Acesso Rápido -->
        <div class="card p-6">
          <p class="font-display text-lg tracking-wide">Acesso Rápido</p>
          <div class="mt-4 grid grid-cols-2 gap-3">
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="baixarCarteirinha">
              <span class="btn-label">Baixar Carteirinha</span>
            </button>
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="openEditForm">
              <span class="btn-label">Editar Perfil</span>
            </button>
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="toggleHistorico">
              <span class="btn-label">{{ showHistorico ? 'Fechar Histórico' : 'Histórico' }}</span>
            </button>
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="trocarConta">
              <span class="btn-label">Trocar Conta</span>
            </button>
          </div>
        </div>

        <!-- Editar Perfil (inline) -->
        <div v-if="showEditForm" class="card p-6">
          <p class="font-display text-lg tracking-wide">Editar Perfil</p>
          <form class="mt-4 space-y-4" @submit.prevent="saveProfile">
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Nome</label>
              <input
                v-model="editForm.nome"
                type="text"
                required
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">RA</label>
              <input
                v-model="editForm.ra"
                type="text"
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Curso</label>
              <input
                v-model="editForm.curso"
                type="text"
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Telefone</label>
              <input
                v-model="editForm.telefone"
                type="text"
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Data de Nascimento</label>
              <input
                v-model="editForm.dataNascimento"
                type="date"
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>

            <div class="flex gap-3">
              <button type="submit" class="btn-fire flex-1 !py-3 !text-sm" :disabled="savingProfile">
                <span class="btn-label">{{ savingProfile ? 'Salvando...' : 'Salvar' }}</span>
              </button>
              <button type="button" class="btn-ghost flex-1 !py-3 !text-sm" @click="closeEditForm">
                <span class="btn-label">Cancelar</span>
              </button>
            </div>
          </form>
        </div>

        <!-- Histórico de Pedidos -->
        <div v-if="showHistorico" class="card p-6">
          <p class="font-display text-lg tracking-wide">Histórico de Pedidos</p>

          <p v-if="loadingPedidos" class="mt-4 text-sm text-white/50">Carregando pedidos...</p>
          <p v-else-if="pedidosError" class="mt-4 text-sm text-fenix-red">{{ pedidosError }}</p>
          <p v-else-if="pedidos.length === 0" class="mt-4 text-sm text-white/50">
            Você ainda não fez nenhum pedido na loja.
            <router-link to="/loja" class="text-fenix-orange underline">Visite a loja</router-link>
          </p>

          <ul v-else class="mt-4 space-y-4">
            <li v-for="pedido in pedidos" :key="pedido.id" class="border-2 border-white/10 bg-white/[0.02] p-4">
              <div class="flex items-center justify-between">
                <p class="text-sm font-semibold text-white">Pedido #{{ pedido.id }}</p>
                <span class="text-xs font-bold uppercase text-fenix-gold">{{ pedido.status }}</span>
              </div>
              <p class="mt-1 text-xs text-white/50">{{ formatDate(pedido.dataPedido) }} &middot; {{ formatCurrency(pedido.valorTotal) }}</p>
              <ul class="mt-3 space-y-1">
                <li v-for="item in pedido.itens" :key="item.id" class="text-xs text-white/70">
                  {{ item.quantidade }}x {{ item.produtoNome }} &middot; {{ formatCurrency(item.precoUnitario) }}
                </li>
              </ul>
            </li>
          </ul>
        </div>
      </div>
    </div>
  </div>
</template>

<style scoped>
@media print {
  .print-hide {
    display: none !important;
  }
}
</style>

<style>
@media print {
  header,
  nav,
  footer {
    display: none !important;
  }
}
</style>
