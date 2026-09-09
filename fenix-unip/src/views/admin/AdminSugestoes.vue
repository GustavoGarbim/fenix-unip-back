<script setup>
import { onMounted, reactive, ref, watch } from 'vue'
import { useSugestoes } from '../../composables/useSugestoes'

const { sugestoes, fetchSugestoes, responder } = useSugestoes()

onMounted(fetchSugestoes)

const drafts = reactive({})
const sending = ref(null)
const feedback = reactive({})

watch(
  sugestoes,
  (list) => {
    for (const s of list) {
      if (drafts[s.id] === undefined) {
        drafts[s.id] = s.respostaAdmin || ''
      }
    }
  },
  { immediate: true }
)

function statusClasses(status) {
  const s = (status || '').toLowerCase()
  if (s === 'respondida') return 'border-emerald-500/30 bg-emerald-500/10 text-emerald-400'
  if (s === 'pendente') return 'border-fenix-gold/30 bg-fenix-gold/10 text-fenix-gold'
  return 'border-white/15 bg-white/5 text-white/70'
}

function formatDate(iso) {
  if (!iso) return '—'
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return '—'
  return d.toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
}

async function submitResposta(sugestao) {
  const texto = (drafts[sugestao.id] || '').trim()
  if (!texto) return
  sending.value = sugestao.id
  feedback[sugestao.id] = ''
  try {
    await responder(sugestao.id, texto)
    feedback[sugestao.id] = 'Resposta enviada com sucesso.'
  } catch (err) {
    feedback[sugestao.id] = err.message || 'Não foi possível enviar a resposta.'
  } finally {
    sending.value = null
  }
}
</script>

<template>
  <div>
    <span class="section-label">Sócios</span>
    <h2 class="mt-3 font-display text-2xl tracking-wide sm:text-3xl">
      Caixa de <span class="text-fenix-orange">Sugestões</span>
    </h2>

    <div class="mt-8 space-y-4">
      <div v-for="s in sugestoes" :key="s.id" class="card animate-fade-up p-5 sm:p-6">
        <div class="flex flex-wrap items-start justify-between gap-4">
          <div class="min-w-0">
            <p class="font-display text-lg tracking-wide">{{ s.nomeAutor || 'Anônimo' }}</p>
            <p class="text-xs text-white/50">
              {{ s.categoria || 'Geral' }} &middot; {{ s.email || 'sem e-mail' }} &middot; {{ formatDate(s.dataEnvio) }}
            </p>
          </div>
          <span class="shrink-0 -skew-x-6 border-2 px-3 py-1 text-[11px] font-bold uppercase tracking-wide" :class="statusClasses(s.status)">
            <span class="btn-label">{{ s.status || 'Pendente' }}</span>
          </span>
        </div>

        <p class="mt-3 border-l-2 border-white/15 pl-3 text-sm text-white/70">{{ s.mensagem }}</p>

        <div class="mt-4">
          <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">
            {{ s.respostaAdmin ? 'Resposta enviada (editar)' : 'Responder' }}
          </label>
          <textarea
            v-model="drafts[s.id]"
            placeholder="Escreva a resposta para este sócio..."
            rows="3"
            class="w-full resize-none border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
          />
          <p v-if="feedback[s.id]" class="mt-2 text-sm font-medium text-white/70">{{ feedback[s.id] }}</p>
          <button
            type="button"
            class="btn-fire mt-3 !px-6 !py-2.5 !text-sm"
            :disabled="sending === s.id"
            @click="submitResposta(s)"
          >
            <span class="btn-label">{{ sending === s.id ? 'Enviando...' : s.respostaAdmin ? 'Reenviar Resposta' : 'Responder' }}</span>
          </button>
        </div>
      </div>

      <div v-if="sugestoes.length === 0" class="card p-8 text-center text-sm text-white/50">
        Nenhuma sugestão recebida ainda.
      </div>
    </div>
  </div>
</template>
