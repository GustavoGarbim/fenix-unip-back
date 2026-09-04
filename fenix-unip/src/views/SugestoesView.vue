<script setup>
import { reactive, ref } from 'vue'
import { useDemoMode } from '../composables/useDemoMode'
import { post } from '../services/api'

const { triggerDemo } = useDemoMode()

const topics = ['Sugestão', 'Elogio', 'Reclamação', 'Parceria']
const activeTopic = ref('Sugestão')

const form = reactive({
  name: '',
  email: '',
  message: '',
})

const sending = ref(false)
const feedback = ref('')

async function submitSuggestion() {
  sending.value = true
  feedback.value = ''
  try {
    await post(
      '/sugestoes',
      {
        nomeAutor: form.name?.trim() || null,
        email: form.email?.trim() || null,
        categoria: activeTopic.value,
        mensagem: form.message,
      },
      { auth: false }
    )
    triggerDemo('Enviar Sugestão')
    feedback.value = 'Sugestão enviada com sucesso!'
    form.name = ''
    form.email = ''
    form.message = ''
  } catch (err) {
    feedback.value = err.message || 'Não foi possível enviar sua sugestão. Tente novamente.'
  } finally {
    sending.value = false
  }
}
</script>

<template>
  <div class="mx-auto max-w-3xl px-5 py-16 sm:px-8 lg:py-24">
    <span class="section-label">Fala Aí</span>
    <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
      Sua voz <span class="text-fenix-orange">move a Fênix</span>
    </h1>
    <p class="mt-3 max-w-lg text-white/60">
      Ideias, elogios ou pau na máquina: manda sem medo, a diretoria lê tudo.
    </p>

    <div class="mt-10 border-2 border-white/15 bg-white/[0.02] p-6 sm:p-10">
      <div class="flex flex-wrap gap-3">
        <button
          v-for="t in topics"
          :key="t"
          type="button"
          class="-skew-x-6 border-2 px-5 py-2 text-sm font-bold uppercase tracking-wide transition-all duration-200"
          :class="
            activeTopic === t
              ? 'border-fenix-orange bg-fenix-orange text-fenix-black'
              : 'border-white/15 text-white/60 hover:border-white/40 hover:text-white'
          "
          @click="activeTopic = t"
        >
          <span class="btn-label">{{ t }}</span>
        </button>
      </div>

      <form class="mt-8 space-y-5" @submit.prevent="submitSuggestion">
        <div class="grid gap-5 sm:grid-cols-2">
          <div>
            <label class="mb-1.5 block text-xs font-bold uppercase tracking-wider text-white/50">Nome</label>
            <input
              v-model="form.name"
              type="text"
              required
              placeholder="Seu nome"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
          <div>
            <label class="mb-1.5 block text-xs font-bold uppercase tracking-wider text-white/50">E-mail</label>
            <input
              v-model="form.email"
              type="email"
              required
              placeholder="voce@email.com"
              class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
            />
          </div>
        </div>

        <div>
          <label class="mb-1.5 block text-xs font-bold uppercase tracking-wider text-white/50">Mensagem</label>
          <textarea
            v-model="form.message"
            rows="5"
            required
            placeholder="Manda a real..."
            class="w-full resize-none border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
          />
        </div>

        <p v-if="feedback" class="text-sm font-medium text-white/70">{{ feedback }}</p>

        <button type="submit" class="btn-fire w-full !py-3.5 !text-base" :disabled="sending">
          <span class="btn-label">{{ sending ? 'Enviando...' : 'Enviar Sugestão' }}</span>
        </button>
      </form>
    </div>
  </div>
</template>
