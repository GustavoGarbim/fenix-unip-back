<script setup>
import { reactive, ref } from 'vue'
import { useRoute, useRouter } from 'vue-router'
import { useAuth } from '../composables/useAuth'

const route = useRoute()
const router = useRouter()
const { login } = useAuth()

const form = reactive({ email: '', senha: '' })
const error = ref('')
const submitting = ref(false)

async function submit() {
  error.value = ''
  submitting.value = true
  try {
    await login(form.email.trim(), form.senha)
    router.push(route.query.redirect || '/portal')
  } catch (err) {
    error.value = err.status === 401 || err.status === 400
      ? 'E-mail ou senha inválidos.'
      : err.message || 'Não foi possível entrar. Tente novamente.'
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
        Entrar na <span class="text-fenix-orange">Fênix</span>
      </h1>
      <p class="mt-3 text-sm text-white/60">Acesse sua conta de sócio da Atlética Fênix UNIP.</p>
    </div>

    <form class="card mt-10 space-y-4 p-6 sm:p-8" @submit.prevent="submit">
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
          autocomplete="current-password"
          placeholder="••••••••"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>

      <p v-if="error" class="text-sm font-medium text-red-400">{{ error }}</p>

      <button type="submit" class="btn-fire w-full !py-3.5 !text-base" :disabled="submitting">
        <span class="btn-label">{{ submitting ? 'Entrando...' : 'Entrar' }}</span>
      </button>

      <div class="flex flex-col items-center gap-2 pt-1 text-center text-xs text-white/50">
        <RouterLink to="/recuperar-senha" class="transition hover:text-fenix-orange">Esqueci minha senha</RouterLink>
        <RouterLink to="/cadastro" class="transition hover:text-fenix-orange">Ainda não é sócio? Cadastre-se</RouterLink>
      </div>
    </form>
  </div>
</template>
