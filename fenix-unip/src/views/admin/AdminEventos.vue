<script setup>
import { onMounted, reactive, ref } from 'vue'
import { useEventos } from '../../composables/useEventos'
import { useModalidades } from '../../composables/useModalidades'

const { eventos, createEvento, updateEvento, deleteEvento } = useEventos()
const { modalidades, fetchModalidades } = useModalidades()

onMounted(fetchModalidades)

const emptyForm = { modalidadeId: '', titulo: '', descricao: '', dataHora: '', local: '', tipoEvento: '' }
const form = reactive({ ...emptyForm })
const editingId = ref(null)
const saving = ref(false)
const feedback = ref('')

function toDatetimeLocal(iso) {
  if (!iso) return ''
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return ''
  const pad = (n) => String(n).padStart(2, '0')
  return `${d.getFullYear()}-${pad(d.getMonth() + 1)}-${pad(d.getDate())}T${pad(d.getHours())}:${pad(d.getMinutes())}`
}

function startEdit(evento) {
  editingId.value = evento.id
  Object.assign(form, {
    modalidadeId: evento.modalidadeId,
    titulo: evento.titulo,
    descricao: evento.descricao,
    dataHora: toDatetimeLocal(evento.dataHora),
    local: evento.local,
    tipoEvento: evento.tipoEvento,
  })
  feedback.value = ''
}

function cancelEdit() {
  editingId.value = null
  Object.assign(form, emptyForm)
}

async function submit() {
  if (!form.titulo.trim() || !form.modalidadeId || !form.dataHora || !form.tipoEvento.trim()) return
  saving.value = true
  feedback.value = ''
  try {
    const payload = { ...form, dataHora: new Date(form.dataHora).toISOString() }
    if (editingId.value) {
      await updateEvento(editingId.value, payload)
      feedback.value = 'Evento atualizado com sucesso.'
    } else {
      await createEvento(payload)
      feedback.value = 'Evento criado com sucesso.'
    }
    cancelEdit()
  } catch (err) {
    feedback.value = err.message || 'Não foi possível salvar o evento.'
  } finally {
    saving.value = false
  }
}

async function removeEvento(evento) {
  if (!window.confirm(`Excluir o evento "${evento.titulo}"?`)) return
  try {
    await deleteEvento(evento.id)
    if (editingId.value === evento.id) cancelEdit()
  } catch (err) {
    feedback.value = err.message || 'Não foi possível excluir o evento.'
  }
}

function formatDate(iso) {
  if (!iso) return '—'
  const d = new Date(iso)
  if (Number.isNaN(d.getTime())) return '—'
  return d.toLocaleString('pt-BR', { day: '2-digit', month: '2-digit', year: 'numeric', hour: '2-digit', minute: '2-digit' })
}
</script>

<template>
  <div>
    <span class="section-label">Agenda</span>
    <h2 class="mt-3 font-display text-2xl tracking-wide sm:text-3xl">
      Gerenciar <span class="text-fenix-orange">Eventos</span>
    </h2>

    <div class="mt-8 grid gap-6 lg:grid-cols-[0.9fr_1.1fr]">
      <div class="card animate-fade-up p-6 sm:p-8">
        <p class="font-display text-lg tracking-wide">{{ editingId ? `Editando evento #${editingId}` : 'Novo Evento' }}</p>
        <form class="mt-5 space-y-4" @submit.prevent="submit">
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Modalidade</label>
            <select
              v-model="form.modalidadeId"
              required
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            >
              <option value="" disabled class="bg-fenix-black">Selecione uma modalidade</option>
              <option v-for="m in modalidades" :key="m.id" :value="m.id" class="bg-fenix-black">{{ m.nome }}</option>
            </select>
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Título</label>
            <input
              v-model="form.titulo"
              type="text"
              required
              placeholder="Fênix x Rival"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Descrição</label>
            <textarea
              v-model="form.descricao"
              rows="3"
              placeholder="Descrição do evento"
              class="w-full resize-none border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Data e hora</label>
              <input
                v-model="form.dataHora"
                type="datetime-local"
                required
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Tipo</label>
              <input
                v-model="form.tipoEvento"
                type="text"
                required
                placeholder="Jogo, Treino..."
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Local</label>
            <input
              v-model="form.local"
              type="text"
              placeholder="Ginásio UNIP"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>

          <p v-if="feedback" class="text-sm font-medium text-white/70">{{ feedback }}</p>

          <div class="flex gap-3">
            <button type="submit" class="btn-fire flex-1 !py-3 !text-base" :disabled="saving">
              <span class="btn-label">{{ saving ? 'Salvando...' : editingId ? 'Salvar Alterações' : 'Criar Evento' }}</span>
            </button>
            <button v-if="editingId" type="button" class="btn-ghost !py-3 !text-base" @click="cancelEdit">
              <span class="btn-label">Cancelar</span>
            </button>
          </div>
        </form>
      </div>

      <div class="animate-fade-up" style="animation-delay: 0.1s">
        <p class="font-display text-lg tracking-wide">Eventos Cadastrados</p>
        <div class="mt-5 space-y-3">
          <div
            v-for="e in eventos"
            :key="e.id"
            class="card flex items-center justify-between gap-4 p-4 sm:p-5"
          >
            <div class="min-w-0 flex-1">
              <p class="truncate font-semibold text-white">{{ e.titulo }}</p>
              <p class="text-xs text-white/50">
                {{ e.modalidadeNome || 'Modalidade #' + e.modalidadeId }} &middot; {{ formatDate(e.dataHora) }} &middot; {{ e.local || 'Local a definir' }}
              </p>
              <span class="mt-1 inline-block -skew-x-6 border-2 border-white/15 px-2 py-0.5 text-[10px] font-bold uppercase tracking-wide text-white/70">
                {{ e.tipoEvento }}
              </span>
            </div>
            <div class="flex shrink-0 gap-2">
              <button type="button" class="btn-ghost !px-4 !py-2 !text-xs" @click="startEdit(e)">
                <span class="btn-label">Editar</span>
              </button>
              <button
                type="button"
                class="flex h-9 w-9 items-center justify-center rounded-full bg-black/60 text-white/70 transition hover:bg-red-500/20 hover:text-red-400"
                title="Excluir evento"
                @click="removeEvento(e)"
              >
                <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M6 7h12M9 7V5a1 1 0 011-1h4a1 1 0 011 1v2m-7 0v12a1 1 0 001 1h6a1 1 0 001-1V7" />
                </svg>
              </button>
            </div>
          </div>

          <div v-if="eventos.length === 0" class="card p-8 text-center text-sm text-white/50">
            Nenhum evento cadastrado ainda.
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
