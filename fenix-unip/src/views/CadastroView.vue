<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAuth } from '../composables/useAuth'

const router = useRouter()
const { register } = useAuth()

const form = reactive({
  nome: '',
  email: '',
  senha: '',
  ra: '',
  curso: '',
  telefone: '',
})

const error = ref('')
const submitting = ref(false)

async function submit() {
  error.value = ''
  if (form.senha.length < 6) {
    error.value = 'A senha deve ter no mínimo 6 caracteres.'
    return
  }

  submitting.value = true
  try {
    await register({
      Nome: form.nome.trim(),
      Email: form.email.trim(),
      Senha: form.senha,
      RA: form.ra || null,
      Curso: form.curso || null,
      Telefone: form.telefone || null,
    })
    router.push('/portal')
  } catch (err) {
    error.value = err.message || 'Não foi possível concluir o cadastro. Tente novamente.'
  } finally {
    submitting.value = false
  }
}
</script>

<template>
  <div class="mx-auto flex min-h-[70vh] max-w-md flex-col justify-center px-5 py-16 sm:px-8">
    <div class="text-center">
      <span class="section-label">Portal do Sócio</span>
      <h1 class="mt-5 font-display text-3xl tracking-wide sm:text-4xl">
        Seja <span class="text-fenix-orange">Fênix</span>
      </h1>
      <p class="mt-3 text-sm text-white/60">Cadastre-se e faça parte da Atlética Fênix UNIP.</p>
    </div>

    <form class="card mt-10 space-y-4 p-6 sm:p-8" @submit.prevent="submit">
      <div>
        <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Nome completo</label>
        <input
          v-model="form.nome"
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
          autocomplete="username"
          placeholder="voce@email.com"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>
      <div>
        <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Senha</label>
        <input
          v-model="form.senha"
          type="password"
          required
          minlength="6"
          autocomplete="new-password"
          placeholder="Mínimo 6 caracteres"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>
      <div class="grid grid-cols-2 gap-4">
        <div>
          <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">RA (opcional)</label>
          <input
            v-model="form.ra"
            type="text"
            placeholder="N123456-7"
            class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
          />
        </div>
        <div>
          <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Curso (opcional)</label>
          <input
            v-model="form.curso"
            type="text"
            placeholder="Ex: ADS"
            class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
          />
        </div>
      </div>
      <div>
        <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Telefone (opcional)</label>
        <input
          v-model="form.telefone"
          type="tel"
          placeholder="(11) 90000-0000"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>

      <p v-if="error" class="text-sm font-medium text-red-400">{{ error }}</p>

      <button type="submit" class="btn-fire w-full !py-3.5 !text-base" :disabled="submitting">
        <span class="btn-label">{{ submitting ? 'Cadastrando...' : 'Cadastrar' }}</span>
      </button>

      <p class="pt-1 text-center text-xs text-white/50">
        Já é sócio?
        <RouterLink to="/login" class="transition hover:text-fenix-orange">Entrar</RouterLink>
      </p>
    </form>
  </div>
</template>
