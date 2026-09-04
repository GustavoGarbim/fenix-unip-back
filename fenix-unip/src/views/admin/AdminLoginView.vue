<script setup>
import { reactive, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useAdminAuth } from '../../composables/useAdminAuth'

const router = useRouter()
const { login } = useAdminAuth()

const form = reactive({ username: '', password: '' })
const error = ref('')

async function submit() {
  const ok = await login(form.username.trim(), form.password)
  if (ok) {
    error.value = ''
    router.push('/admin')
  } else {
    error.value = 'Usuário ou senha inválidos.'
  }
}
</script>

<template>
  <div class="mx-auto flex min-h-[70vh] max-w-md flex-col justify-center px-5 py-16 sm:px-8">
    <div class="text-center">
      <span class="section-label">Área Restrita</span>
      <h1 class="mt-5 font-display text-3xl tracking-wide sm:text-4xl">
        Painel <span class="text-fenix-orange">Administrativo</span>
      </h1>
      <p class="mt-3 text-sm text-white/60">Acesso exclusivo para a diretoria da Fênix.</p>
    </div>

    <form class="card mt-10 space-y-4 p-6 sm:p-8" @submit.prevent="submit">
      <div>
        <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">E-mail</label>
        <input
          v-model="form.username"
          type="email"
          required
          autocomplete="username"
          placeholder="admin@fenixunip.com.br"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>
      <div>
        <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Senha</label>
        <input
          v-model="form.password"
          type="password"
          required
          autocomplete="current-password"
          placeholder="••••••••"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>

      <p v-if="error" class="text-sm font-medium text-red-400">{{ error }}</p>

      <button type="submit" class="btn-fire w-full !py-3.5 !text-base"><span class="btn-label">Entrar</span></button>

      <p class="pt-1 text-center text-xs text-white/30">Acesso com as credenciais cadastradas pela diretoria.</p>
    </form>
  </div>
</template>
