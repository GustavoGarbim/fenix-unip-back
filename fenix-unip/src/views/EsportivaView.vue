<script setup>
import { onMounted, reactive, ref } from 'vue'
import { useDemoMode } from '../composables/useDemoMode'
import { get, post } from '../services/api'

const { triggerDemo } = useDemoMode()

const monthAbbrev = ['JAN', 'FEV', 'MAR', 'ABR', 'MAI', 'JUN', 'JUL', 'AGO', 'SET', 'OUT', 'NOV', 'DEZ']

const events = ref([])
const modalidades = ref([])
const modalities = ref([])

const loadingAgenda = ref(false)

function mapEvent(e) {
  const dataHora = e.dataHora ?? e.DataHora
  const d = dataHora ? new Date(dataHora) : null
  return {
    id: e.id ?? e.Id,
    day: d ? String(d.getDate()).padStart(2, '0') : '--',
    month: d ? monthAbbrev[d.getMonth()] : '',
    title: e.titulo ?? e.Titulo ?? '',
    type: e.tipoEvento ?? e.TipoEvento ?? 'Evento',
    time: d ? d.toLocaleTimeString('pt-BR', { hour: '2-digit', minute: '2-digit' }) : '',
    place: e.local ?? e.Local ?? '',
  }
}

function mapModalidade(m) {
  return {
    id: m.id ?? m.Id,
    nome: m.nome ?? m.Nome,
  }
}

async function fetchAgenda() {
  loadingAgenda.value = true
  try {
    const [eventosData, modalidadesData] = await Promise.all([
      get('/eventos', { auth: false }),
      get('/modalidades', { auth: false }),
    ])
    if (Array.isArray(eventosData)) {
      events.value = eventosData.map(mapEvent)
    }
    if (Array.isArray(modalidadesData)) {
      modalidades.value = modalidadesData.map(mapModalidade)
      modalities.value = modalidades.value.map((m) => m.nome)
    }
  } catch {
    events.value = []
    modalities.value = []
  } finally {
    loadingAgenda.value = false
  }
}

onMounted(fetchAgenda)

const form = reactive({
  name: '',
  email: '',
  ra: '',
  modality: '',
  experience: '',
})

const submitting = ref(false)
const submitFeedback = ref('')

function typeClasses(type) {
  return type === 'Jogo'
    ? 'bg-fenix-red text-white'
    : 'border-2 border-white/15 bg-white/5 text-white/70'
}

async function submitTryout() {
  const selected = modalidades.value.find((m) => m.nome === form.modality)
  submitting.value = true
  submitFeedback.value = ''
  try {
    await post(
      '/tryouts',
      {
        modalidadeId: selected?.id ?? null,
        nomeCandidato: form.name,
        email: form.email,
        ra: form.ra,
        mensagem: form.experience || null,
      },
      { auth: false }
    )
    triggerDemo('Faça parte da Bateria')
    submitFeedback.value = 'Inscrição enviada com sucesso!'
    form.name = ''
    form.email = ''
    form.ra = ''
    form.modality = ''
    form.experience = ''
  } catch (err) {
    submitFeedback.value = err.message || 'Não foi possível enviar sua inscrição. Tente novamente.'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <div class="mx-auto max-w-7xl px-5 py-16 sm:px-8 lg:py-24">
    <span class="section-label">Seletivas</span>
    <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
      Treinos, jogos e <span class="text-fenix-orange">seletivas</span>
    </h1>
    <p class="mt-3 max-w-lg text-white/60">Acompanhe a agenda oficial da Fênix e faça parte da bateria disputando uma vaga no time.</p>

    <div class="mt-12 grid gap-6 lg:grid-cols-[1.15fr_0.85fr]">
      <div class="animate-fade-up card p-6 sm:p-8">
        <div class="flex items-center justify-between">
          <p class="font-display text-lg tracking-wide">Próximos Compromissos</p>
          <span class="section-label">Setembro 2026</span>
        </div>

        <p v-if="!loadingAgenda && events.length === 0" class="mt-6 text-sm text-white/50">
          Nenhum evento agendado no momento.
        </p>

        <ul class="mt-6 space-y-3">
          <li
            v-for="e in events"
            :key="e.id ?? e.title + e.day"
            class="group flex items-center gap-4 border-2 border-white/10 bg-white/[0.02] p-4 transition-all duration-300 hover:border-fenix-orange/40 hover:bg-white/[0.05]"
          >
            <div class="flex h-14 w-14 shrink-0 flex-col items-center justify-center border-2 border-white/10 bg-black/40 text-center">
              <span class="font-display text-xl leading-none text-fenix-gold">{{ e.day }}</span>
              <span class="text-[10px] uppercase tracking-wider text-white/50">{{ e.month }}</span>
            </div>
            <div class="min-w-0 flex-1">
              <p class="truncate font-semibold text-white">{{ e.title }}</p>
              <p class="text-xs text-white/50">{{ e.time }} &middot; {{ e.place }}</p>
            </div>
            <span class="shrink-0 -skew-x-6 px-3 py-1 text-[11px] font-bold uppercase tracking-wide" :class="typeClasses(e.type)">
              <span class="btn-label">{{ e.type }}</span>
            </span>
          </li>
        </ul>
      </div>

      <div class="animate-fade-up card p-6 sm:p-8" style="animation-delay: 0.1s">
        <p class="font-display text-lg tracking-wide">Faça Parte da Bateria</p>
        <p class="mt-1 text-sm text-white/50">Preencha seus dados e mostre sua garra na próxima seletiva da Fênix.</p>

        <form class="mt-6 space-y-4" @submit.prevent="submitTryout">
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Nome completo</label>
            <input
              v-model="form.name"
              type="text"
              required
              placeholder="Seu nome"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">E-mail</label>
            <input
              v-model="form.email"
              type="email"
              required
              placeholder="voce@email.com"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">RA</label>
            <input
              v-model="form.ra"
              type="text"
              required
              placeholder="N123456-7"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Modalidade</label>
            <select
              v-model="form.modality"
              required
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            >
              <option value="" disabled class="bg-fenix-black">Selecione uma modalidade</option>
              <option v-for="m in modalities" :key="m" :value="m" class="bg-fenix-black">{{ m }}</option>
            </select>
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Experiência (opcional)</label>
            <textarea
              v-model="form.experience"
              rows="3"
              placeholder="Conte um pouco sobre sua trajetória no esporte"
              class="w-full resize-none border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>

          <p v-if="submitFeedback" class="text-sm font-medium text-white/70">{{ submitFeedback }}</p>

          <button type="submit" class="btn-fire w-full !py-3.5 !text-base" :disabled="submitting">
            <span class="btn-label">{{ submitting ? 'Enviando...' : 'Faça Parte da Bateria' }}</span>
          </button>
        </form>
      </div>
    </div>
  </div>
</template>
