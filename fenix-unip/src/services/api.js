const BASE_URL = import.meta.env.VITE_API_URL || 'http://localhost:5004/api'

const ADMIN_TOKEN_KEY = 'fenix-admin-token'
const USER_TOKEN_KEY = 'fenix-auth-token'

function getToken() {
  return localStorage.getItem(ADMIN_TOKEN_KEY) || localStorage.getItem(USER_TOKEN_KEY) || null
}

async function request(path, { method = 'GET', body, headers = {}, auth = true } = {}) {
  const finalHeaders = { ...headers }

  if (body !== undefined) {
    finalHeaders['Content-Type'] = 'application/json'
  }

  if (auth) {
    const token = getToken()
    if (token) {
      finalHeaders['Authorization'] = `Bearer ${token}`
    }
  }

  let response
  try {
    response = await fetch(`${BASE_URL}${path}`, {
      method,
      headers: finalHeaders,
      body: body !== undefined ? JSON.stringify(body) : undefined,
    })
  } catch {
    throw new Error('Não foi possível conectar ao servidor. Tente novamente mais tarde.')
  }

  const contentType = response.headers.get('content-type') || ''
  const isJson = contentType.includes('application/json')
  const data = isJson ? await response.json().catch(() => null) : await response.text().catch(() => null)

  if (!response.ok) {
    const message =
      (data && typeof data === 'object' && (data.message || data.title || data.error)) ||
      (typeof data === 'string' && data) ||
      `Erro na requisição (${response.status})`
    const error = new Error(message)
    error.status = response.status
    error.data = data
    throw error
  }

  return data
}

export function get(path, options = {}) {
  return request(path, { ...options, method: 'GET' })
}

export function post(path, body, options = {}) {
  return request(path, { ...options, method: 'POST', body })
}

export function put(path, body, options = {}) {
  return request(path, { ...options, method: 'PUT', body })
}

export function del(path, options = {}) {
  return request(path, { ...options, method: 'DELETE' })
}

export const api = { get, post, put, del }

export { ADMIN_TOKEN_KEY, USER_TOKEN_KEY }

export default api
