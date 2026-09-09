<script setup>
import { onMounted, ref } from 'vue'
import { useTryouts } from '../../composables/useTryouts'

const { tryouts, fetchTryouts, updateStatus } = useTryouts()

onMounted(fetchTryouts)

const feedback = ref('')
const statusOptions = ['Pendente', 'Aprovado', 'Recusado']

function statusClasses(status) {
  const s = (status || '').toLowerCase()
  if (s === 'aprovado') return 'border-emerald-500/30 bg-emerald-500/10 text-emerald-400'
  if (s === 'recusado') return 'border-red-500/30 bg-red-500/10 text-red-400'
  return 'border-fenix-gold/30 bg-fenix-gold/10 text-fenix-gold'
}

function formatDate(iso) {
  if (!iso) return '—'
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return '—'
  return d.toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
}

async function changeStatus(tryout, status) {
  if (status === tryout.status) return
  feedback.value = ''
  try {
    await updateStatus(tryout.id, status)
  } catch (err) {
    feedback.value = err.message || 'Não foi possível atualizar o status.'
  }
}
</script>

<template>
  <div>
    <span class="section-label">Seletivas</span>
    <h2 class="mt-3 font-display text-2xl tracking-wide sm:text-3xl">
      Candidatos <span class="text-fenix-orange">Inscritos</span>
    </h2>

    <p v-if="feedback" class="mt-4 text-sm font-medium text-red-400">{{ feedback }}</p>

    <div class="mt-8 space-y-4">
      <div v-for="t in tryouts" :key="t.id" class="card animate-fade-up p-5 sm:p-6">
        <div class="flex flex-wrap items-start justify-between gap-4">
          <div class="min-w-0">
            <p class="font-display text-lg tracking-wide">{{ t.nomeCandidato }}</p>
            <p class="text-xs text-white/50">{{ t.modalidadeNome || 'Modalidade #' + t.modalidadeId }} &middot; {{ formatDate(t.dataInscricao) }}</p>
          </div>
          <span class="shrink-0 -skew-x-6 border-2 px-3 py-1 text-[11px] font-bold uppercase tracking-wide" :class="statusClasses(t.status)">
            <span class="btn-label">{{ t.status }}</span>
          </span>
        </div>

        <dl class="mt-4 grid grid-cols-1 gap-x-6 gap-y-2 text-sm text-white/70 sm:grid-cols-2">
          <div><dt class="inline text-white/40">E-mail: </dt><dd class="inline">{{ t.email || '—' }}</dd></div>
          <div><dt class="inline text-white/40">Telefone: </dt><dd class="inline">{{ t.telefone || '—' }}</dd></div>
          <div><dt class="inline text-white/40">RA: </dt><dd class="inline">{{ t.ra || '—' }}</dd></div>
          <div><dt class="inline text-white/40">Curso: </dt><dd class="inline">{{ t.curso || '—' }}</dd></div>
        </dl>
        <p v-if="t.mensagem" class="mt-3 border-l-2 border-white/15 pl-3 text-sm italic text-white/60">{{ t.mensagem }}</p>

        <div class="mt-4 flex flex-wrap gap-2">
          <button
            v-for="s in statusOptions"
            :key="s"
            type="button"
            class="-skew-x-6 border-2 px-4 py-1.5 text-xs font-bold uppercase tracking-wide transition"
            :class="
              t.status === s
                ? 'border-fenix-orange bg-fenix-orange text-fenix-black'
                : 'border-white/15 text-white/60 hover:border-white/40 hover:text-white'
            "
            @click="changeStatus(t, s)"
          >
            <span class="btn-label">{{ s }}</span>
          </button>
        </div>
      </div>

      <div v-if="tryouts.length === 0" class="card p-8 text-center text-sm text-white/50">
        Nenhuma inscrição de seletiva recebida ainda.
      </div>
    </div>
  </div>
</template>
