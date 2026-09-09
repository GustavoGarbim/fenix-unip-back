<script setup>
import { reactive, ref } from 'vue'
import { useModalidades } from '../../composables/useModalidades'

const { modalidades, createModalidade, updateModalidade, deleteModalidade } = useModalidades()

const emptyForm = { nome: '', descricao: '', categoria: '', tecnico: '', imagemUrl: '', ativo: true }
const form = reactive({ ...emptyForm })
const editingId = ref(null)
const saving = ref(false)
const feedback = ref('')

function startEdit(modalidade) {
  editingId.value = modalidade.id
  Object.assign(form, {
    nome: modalidade.nome,
    descricao: modalidade.descricao,
    categoria: modalidade.categoria,
    tecnico: modalidade.tecnico,
    imagemUrl: modalidade.imagemUrl,
    ativo: modalidade.ativo,
  })
  feedback.value = ''
}

function cancelEdit() {
  editingId.value = null
  Object.assign(form, emptyForm)
}

async function submit() {
  if (!form.nome.trim()) return
  saving.value = true
  feedback.value = ''
  try {
    if (editingId.value) {
      await updateModalidade(editingId.value, form)
      feedback.value = 'Modalidade atualizada com sucesso.'
    } else {
      await createModalidade(form)
      feedback.value = 'Modalidade criada com sucesso.'
    }
    cancelEdit()
  } catch (err) {
    feedback.value = err.message || 'Não foi possível salvar a modalidade.'
  } finally {
    saving.value = false
  }
}

async function removeModalidade(modalidade) {
  if (!window.confirm(`Excluir a modalidade "${modalidade.nome}"?`)) return
  try {
    await deleteModalidade(modalidade.id)
    if (editingId.value === modalidade.id) cancelEdit()
  } catch (err) {
    feedback.value = err.message || 'Não foi possível excluir a modalidade.'
  }
}
</script>

<template>
  <div>
    <span class="section-label">Esportiva</span>
    <h2 class="mt-3 font-display text-2xl tracking-wide sm:text-3xl">
      Gerenciar <span class="text-fenix-orange">Modalidades</span>
    </h2>

    <div class="mt-8 grid gap-6 lg:grid-cols-[0.9fr_1.1fr]">
      <div class="card animate-fade-up p-6 sm:p-8">
        <p class="font-display text-lg tracking-wide">{{ editingId ? `Editando modalidade #${editingId}` : 'Nova Modalidade' }}</p>
        <form class="mt-5 space-y-4" @submit.prevent="submit">
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Nome</label>
            <input
              v-model="form.nome"
              type="text"
              required
              placeholder="Futsal"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Descrição</label>
            <textarea
              v-model="form.descricao"
              rows="3"
              placeholder="Descrição da modalidade"
              class="w-full resize-none border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div class="grid grid-cols-2 gap-4">
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Categoria</label>
              <input
                v-model="form.categoria"
                type="text"
                placeholder="Coletivo"
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
            <div>
              <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Técnico</label>
              <input
                v-model="form.tecnico"
                type="text"
                placeholder="Nome do técnico"
                class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
              />
            </div>
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">URL da imagem</label>
            <input
              v-model="form.imagemUrl"
              type="text"
              placeholder="https://..."
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <label class="flex items-center gap-2 text-sm text-white/70">
            <input v-model="form.ativo" type="checkbox" class="h-4 w-4 accent-fenix-orange" />
            Modalidade ativa
          </label>

          <p v-if="feedback" class="text-sm font-medium text-white/70">{{ feedback }}</p>

          <div class="flex gap-3">
            <button type="submit" class="btn-fire flex-1 !py-3 !text-base" :disabled="saving">
              <span class="btn-label">{{ saving ? 'Salvando...' : editingId ? 'Salvar Alterações' : 'Criar Modalidade' }}</span>
            </button>
            <button v-if="editingId" type="button" class="btn-ghost !py-3 !text-base" @click="cancelEdit">
              <span class="btn-label">Cancelar</span>
            </button>
          </div>
        </form>
      </div>

      <div class="animate-fade-up" style="animation-delay: 0.1s">
        <p class="font-display text-lg tracking-wide">Modalidades Cadastradas</p>
        <div class="mt-5 space-y-3">
          <div
            v-for="m in modalidades"
            :key="m.id"
            class="card flex items-center justify-between gap-4 p-4 sm:p-5"
          >
            <div class="min-w-0 flex-1">
              <p class="truncate font-semibold text-white">{{ m.nome }}</p>
              <p class="text-xs text-white/50">
                {{ m.categoria || 'Sem categoria' }} <span v-if="m.tecnico">&middot; Técnico: {{ m.tecnico }}</span>
                <span v-if="!m.ativo" class="text-fenix-red"> &middot; Inativa</span>
              </p>
            </div>
            <div class="flex shrink-0 gap-2">
              <button type="button" class="btn-ghost !px-4 !py-2 !text-xs" @click="startEdit(m)">
                <span class="btn-label">Editar</span>
              </button>
              <button
                type="button"
                class="flex h-9 w-9 items-center justify-center rounded-full bg-black/60 text-white/70 transition hover:bg-red-500/20 hover:text-red-400"
                title="Excluir modalidade"
                @click="removeModalidade(m)"
              >
                <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
                  <path stroke-linecap="round" stroke-linejoin="round" d="M6 7h12M9 7V5a1 1 0 011-1h4a1 1 0 011 1v2m-7 0v12a1 1 0 001 1h6a1 1 0 001-1V7" />
                </svg>
              </button>
            </div>
          </div>

          <div v-if="modalidades.length === 0" class="card p-8 text-center text-sm text-white/50">
            Nenhuma modalidade cadastrada ainda.
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
