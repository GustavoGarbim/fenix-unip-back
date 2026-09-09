import { computed, ref, watch } from 'vue'

const CART_STORAGE_KEY = 'fenix-cart'

function loadStoredItems() {
  try {
    const raw = localStorage.getItem(CART_STORAGE_KEY)
    const parsed = raw ? JSON.parse(raw) : []
    return Array.isArray(parsed) ? parsed : []
  } catch {
    return []
  }
}

const items = ref(loadStoredItems())

watch(
  items,
  (value) => {
    try {
      localStorage.setItem(CART_STORAGE_KEY, JSON.stringify(value))
    } catch {
      // ignore storage errors (e.g. private mode / quota exceeded)
    }
  },
  { deep: true }
)

export function useCart() {
  function addItem(product) {
    const existing = items.value.find((i) => i.id === product.id)
    if (existing) {
      existing.quantity += 1
    } else {
      items.value.push({
        id: product.id,
        name: product.name,
        price: product.price,
        quantity: 1,
      })
    }
  }

  function removeItem(id) {
    const idx = items.value.findIndex((i) => i.id === id)
    if (idx !== -1) items.value.splice(idx, 1)
  }

  function updateQuantity(id, quantity) {
    if (quantity <= 0) {
      removeItem(id)
      return
    }
    const existing = items.value.find((i) => i.id === id)
    if (existing) existing.quantity = quantity
  }

  function clearCart() {
    items.value = []
  }

  const totalCount = computed(() => items.value.reduce((sum, i) => sum + i.quantity, 0))
  const totalPrice = computed(() => items.value.reduce((sum, i) => sum + i.price * i.quantity, 0))

  return {
    items,
    addItem,
    removeItem,
    updateQuantity,
    clearCart,
    totalCount,
    totalPrice,
  }
}
