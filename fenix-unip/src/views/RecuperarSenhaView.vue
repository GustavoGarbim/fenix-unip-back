<script setup>
import { computed, reactive, ref } from 'vue'
import { useRoute } from 'vue-router'
import { useAuth } from '../composables/useAuth'

const route = useRoute()
const { forgotPassword, resetPassword } = useAuth()

const token = computed(() => route.query.token || '')

const emailForm = reactive({ email: '' })
const resetForm = reactive({ novaSenha: '', confirmarSenha: '' })

const submitting = ref(false)
const error = ref('')
const success = ref('')

async function submitForgot() {
  error.value = ''
  success.value = ''
  submitting.value = true
  try {
    await forgotPassword(emailForm.email.trim())
    success.value = 'Se este e-mail estiver cadastrado, você receberá instruções para redefinir sua senha.'
  } catch (err) {
    error.value = err.message || 'Não foi possível processar sua solicitação. Tente novamente.'
  } finally {
    submitting.value = false
  }
}

async function submitReset() {
  error.value = ''
  success.value = ''

  if (resetForm.novaSenha.length < 6) {
    error.value = 'A nova senha deve ter no mínimo 6 caracteres.'
    return
  }
  if (resetForm.novaSenha !== resetForm.confirmarSenha) {
    error.value = 'As senhas não coincidem.'
    return
  }

  submitting.value = true
  try {
    const message = await resetPassword(token.value, resetForm.novaSenha)
    success.value = message || 'Senha redefinida com sucesso!'
  } catch (err) {
    error.value = err.message || 'Token inválido ou expirado.'
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
        Recuperar <span class="text-fenix-orange">Senha</span>
      </h1>
      <p class="mt-3 text-sm text-white/60">
        {{ token ? 'Defina sua nova senha de acesso.' : 'Informe seu e-mail para receber instruções de recuperação.' }}
      </p>
    </div>

    <!-- Modo: solicitar recuperação -->
    <form v-if="!token" class="card mt-10 space-y-4 p-6 sm:p-8" @submit.prevent="submitForgot">
      <div>
        <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">E-mail</label>
        <input
          v-model="emailForm.email"
          type="email"
          required
          placeholder="voce@email.com"
          class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
        />
      </div>

      <p v-if="error" class="text-sm font-medium text-red-400">{{ error }}</p>
      <p v-if="success" class="text-sm font-medium text-fenix-gold">{{ success }}</p>

      <button type="submit" class="btn-fire w-full !py-3.5 !text-base" :disabled="submitting">
        <span class="btn-label">{{ submitting ? 'Enviando...' : 'Enviar instruções' }}</span>
      </button>

      <p class="pt-1 text-center text-xs text-white/50">
        <RouterLink to="/login" class="transition hover:text-fenix-orange">Voltar para o login</RouterLink>
      </p>
    </form>

    <!-- Modo: redefinir senha -->
    <form v-else class="card mt-10 space-y-4 p-6 sm:p-8" @submit.prevent="submitReset">
      <template v-if="!success">
        <div>
          <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Nova senha</label>
          <input
            v-model="resetForm.novaSenha"
            type="password"
            required
            minlength="6"
            placeholder="Mínimo 6 caracteres"
            class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
          />
        </div>
        <div>
          <label class="mb-1.5 block text-xs font-semibold uppercase tracking-wider text-white/50">Confirmar nova senha</label>
          <input
            v-model="resetForm.confirmarSenha"
            type="password"
            required
            minlength="6"
            placeholder="Repita a nova senha"
            class="w-full border-2 border-white/15 bg-white/5 px-4 py-3 text-sm text-white placeholder-white/30 outline-none transition focus:border-fenix-orange/60 focus:bg-white/10"
          />
        </div>

        <p v-if="error" class="text-sm font-medium text-red-400">{{ error }}</p>

        <button type="submit" class="btn-fire w-full !py-3.5 !text-base" :disabled="submitting">
          <span class="btn-label">{{ submitting ? 'Salvando...' : 'Redefinir senha' }}</span>
        </button>
      </template>

      <template v-else>
        <p class="text-sm font-medium text-fenix-gold">{{ success }}</p>
        <RouterLink to="/login" class="btn-fire block w-full !py-3.5 !text-base text-center">
          <span class="btn-label">Ir para o login</span>
        </RouterLink>
      </template>
    </form>
  </div>
</template>
