import { ref } from 'vue'
import { post, ADMIN_TOKEN_KEY } from '../services/api'

const isAdmin = ref(!!localStorage.getItem(ADMIN_TOKEN_KEY))

export function useAdminAuth() {
  async function login(email, senha) {
    try {
      const data = await post('/auth/admin/login', { email, senha }, { auth: false })
      if (data?.token) {
        localStorage.setItem(ADMIN_TOKEN_KEY, data.token)
        isAdmin.value = true
        return true
      }
      return false
    } catch {
      isAdmin.value = false
      return false
    }
  }

  function logout() {
    isAdmin.value = false
    localStorage.removeItem(ADMIN_TOKEN_KEY)
  }

  return { isAdmin, login, logout }
}
