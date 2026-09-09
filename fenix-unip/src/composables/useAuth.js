import { computed, ref } from 'vue'
import { post, USER_TOKEN_KEY } from '../services/api'

const USER_STORAGE_KEY = 'fenix-user'

function loadStoredUser() {
  try {
    const raw = localStorage.getItem(USER_STORAGE_KEY)
    return raw ? JSON.parse(raw) : null
  } catch {
    return null
  }
}

const user = ref(loadStoredUser())

function persistUser(value) {
  user.value = value
  if (value) {
    localStorage.setItem(USER_STORAGE_KEY, JSON.stringify(value))
  } else {
    localStorage.removeItem(USER_STORAGE_KEY)
  }
}

function applyAuthResponse(data) {
  const token = data?.Token ?? data?.token
  if (token) {
    localStorage.setItem(USER_TOKEN_KEY, token)
  }
  persistUser({
    id: data?.Id ?? data?.id,
    nome: data?.Nome ?? data?.nome,
    email: data?.Email ?? data?.email,
  })
  return data
}

export function useAuth() {
  const isAuthenticated = computed(() => !!user.value && !!localStorage.getItem(USER_TOKEN_KEY))

  async function register(payload) {
    const data = await post('/auth/register', payload, { auth: false })
    applyAuthResponse(data)
    return data
  }

  async function login(email, senha) {
    const data = await post('/auth/login', { email, senha }, { auth: false })
    applyAuthResponse(data)
    return data
  }

  function logout() {
    persistUser(null)
    localStorage.removeItem(USER_TOKEN_KEY)
  }

  async function forgotPassword(email) {
    const data = await post('/auth/forgot-password', { email }, { auth: false })
    return data?.message ?? data?.Message ?? ''
  }

  async function resetPassword(token, novaSenha) {
    const data = await post('/auth/reset-password', { token, novaSenha }, { auth: false })
    return data?.message ?? data?.Message ?? ''
  }

  return { user, isAuthenticated, register, login, logout, forgotPassword, resetPassword }
}
