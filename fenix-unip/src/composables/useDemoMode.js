import { reactive } from 'vue'

const state = reactive({
  toasts: [],
})

let nextId = 1

export function useDemoMode() {
  function triggerDemo(actionLabel = '') {
    const id = nextId++
    state.toasts.push({
      id,
      title: 'Modo Demonstração',
      message:
        'Esta é uma interface de validação visual. Na versão final, esta ação será processada por uma API back-end.',
      actionLabel,
    })
    window.setTimeout(() => dismissToast(id), 4200)
  }

  function dismissToast(id) {
    const idx = state.toasts.findIndex((t) => t.id === id)
    if (idx !== -1) state.toasts.splice(idx, 1)
  }

  return { state, triggerDemo, dismissToast }
}
