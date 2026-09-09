<script setup>
import { computed, onMounted, ref } from 'vue'
import { useRouter } from 'vue-router'
import { useDemoMode } from '../composables/useDemoMode'
import { useAuth } from '../composables/useAuth'
import { useCart } from '../composables/useCart'
import { get, post } from '../services/api'

const { notify } = useDemoMode()
const { isAuthenticated } = useAuth()
const { items: cartItems, addItem, removeItem, updateQuantity, clearCart, totalCount, totalPrice } = useCart()
const router = useRouter()

const categories = ['Todos', 'Moletons', 'Camisas', 'Canecas', 'Acessórios']
const activeCategory = ref('Todos')

const emojiByCategory = {
  Moletons: '🧥',
  Camisas: '👕',
  Canecas: '☕',
  Acessórios: '🎗️',
}

const products = ref([])
const loadingProducts = ref(false)

function mapProduct(p) {
  const category = p.categoria ?? p.Categoria ?? 'Acessórios'
  return {
    id: p.id ?? p.Id,
    name: p.nome ?? p.Nome,
    category,
    price: Number(p.preco ?? p.Preco ?? 0),
    badge: null,
    emoji: emojiByCategory[category] || '🛍️',
    imagemUrl: p.imagemUrl ?? p.ImagemUrl ?? null,
  }
}

async function fetchProducts() {
  loadingProducts.value = true
  try {
    const data = await get('/produtos', { auth: false })
    if (Array.isArray(data)) {
      products.value = data.map(mapProduct)
    }
  } catch {
    products.value = []
  } finally {
    loadingProducts.value = false
  }
}

onMounted(fetchProducts)

const filtered = computed(() =>
  activeCategory.value === 'Todos'
    ? products.value
    : products.value.filter((p) => p.category === activeCategory.value)
)

const isCartOpen = ref(false)
const isCheckingOut = ref(false)

function addToCart(product) {
  addItem(product)
}

function openCart() {
  isCartOpen.value = true
}

function closeCart() {
  isCartOpen.value = false
}

function formatPrice(value) {
  return `R$ ${Number(value).toFixed(2).replace('.', ',')}`
}

async function checkout() {
  if (!isAuthenticated.value) {
    isCartOpen.value = false
    router.push('/login?redirect=/loja')
    return
  }

  isCheckingOut.value = true
  try {
    const payload = {
      Itens: cartItems.value.map((i) => ({ ProdutoId: i.id, Quantidade: i.quantity })),
    }
    await post('/pedidos', payload)
    clearCart()
    isCartOpen.value = false
    notify({
      title: 'Pedido registrado!',
      message:
        'Seu pedido foi registrado com sucesso. Nenhum método de pagamento foi configurado ainda — entre em contato com o suporte da Fênix para finalizar o pagamento.',
    })
  } catch (err) {
    notify({
      title: 'Não foi possível concluir o pedido',
      message: err.message,
    })
  } finally {
    isCheckingOut.value = false
  }
}

function tiltFor(i) {
  return ((i % 3) - 1) * 3
}
</script>

<template>
  <div class="mx-auto max-w-7xl px-5 py-16 sm:px-8 lg:py-24">
    <div class="flex flex-col justify-between gap-6 sm:flex-row sm:items-end">
      <div>
        <span class="section-label">Grife Oficial</span>
        <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
          Vista a <span class="text-fenix-orange">Fênix</span>
        </h1>
        <p class="mt-3 max-w-lg text-white/60">Manto oficial, edições limitadas e o orgulho de ser Fênix em cada peça.</p>
      </div>

      <button
        type="button"
        class="relative flex items-center gap-2 self-start border-2 border-white/15 bg-white/5 px-5 py-3 text-sm font-bold uppercase tracking-wide transition hover:border-fenix-orange/60"
        @click="openCart"
      >
        <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m-10 0a2 2 0 100 4 2 2 0 000-4zm10 0a2 2 0 100 4 2 2 0 000-4z" />
        </svg>
        Carrinho
        <span
          v-if="totalCount > 0"
          class="absolute -right-2 -top-2 flex h-6 w-6 items-center justify-center rounded-full bg-fenix-red text-xs font-bold"
        >
          {{ totalCount }}
        </span>
      </button>
    </div>

    <div class="mt-10 flex flex-wrap gap-3">
      <button
        v-for="c in categories"
        :key="c"
        type="button"
        class="-skew-x-6 border-2 px-5 py-2 text-sm font-bold uppercase tracking-wide transition-all duration-200"
        :class="
          activeCategory === c
            ? 'border-fenix-orange bg-fenix-orange text-fenix-black'
            : 'border-white/15 text-white/60 hover:border-white/40 hover:text-white'
        "
        @click="activeCategory = c"
      >
        <span class="btn-label">{{ c }}</span>
      </button>
    </div>

    <TransitionGroup
      tag="div"
      name="grid-item"
      class="mt-10 grid grid-cols-2 gap-5 lg:grid-cols-4"
    >
      <div
        v-for="(p, i) in filtered"
        :key="p.id"
        class="card group col-span-2 flex flex-col overflow-hidden sm:col-span-1"
        :class="i === 0 ? 'sm:col-span-2' : ''"
      >
        <div
          class="relative flex items-center justify-center overflow-hidden bg-fenix-ember"
          :class="i === 0 ? 'h-64 sm:h-72' : 'h-48'"
        >
          <span
            class="absolute inset-0 flex select-none items-center justify-center overflow-hidden whitespace-nowrap font-display uppercase leading-none text-white/[0.06]"
            :class="i === 0 ? 'text-[6rem] sm:text-[8rem]' : 'text-[3.5rem]'"
            :style="{ transform: `rotate(${tiltFor(i)}deg)` }"
          >
            {{ p.category }}
          </span>
          <div
            class="absolute inset-0"
            :class="i % 2 === 0
              ? 'bg-gradient-to-br from-fenix-red/25 via-transparent to-transparent'
              : 'bg-gradient-to-tl from-fenix-orange/20 via-transparent to-transparent'"
          />
          <span
            class="relative transition-transform duration-300 group-hover:scale-110"
            :class="i === 0 ? 'text-8xl sm:text-9xl' : 'text-7xl'"
            :style="{ transform: `rotate(${i % 2 === 0 ? -4 : 4}deg)` }"
          >
            {{ p.emoji }}
          </span>
          <span
            v-if="p.badge"
            class="absolute left-3 top-3 -skew-x-6 bg-fenix-red px-3 py-1 text-[11px] font-bold uppercase tracking-wide"
          >
            <span class="btn-label">{{ p.badge }}</span>
          </span>
        </div>
        <div class="flex flex-1 flex-col p-5">
          <p class="text-xs uppercase tracking-wider text-white/40">{{ p.category }}</p>
          <h3 class="mt-1 font-display tracking-wide" :class="i === 0 ? 'text-2xl' : 'text-xl'">{{ p.name }}</h3>
          <p class="mt-2 font-display text-2xl text-fenix-gold">
            R$ {{ p.price.toFixed(2).replace('.', ',') }}
          </p>
          <button type="button" class="btn-fire mt-5 w-full !py-3 !text-base" @click="addToCart(p)">
            <span class="btn-label">Garante o Manto</span>
          </button>
        </div>
      </div>
    </TransitionGroup>

    <div v-if="!loadingProducts && filtered.length === 0" class="card mt-10 p-10 text-center text-white/50">
      Nenhum produto disponível nesta categoria.
    </div>

    <Transition name="cart-fade">
      <div
        v-if="isCartOpen"
        class="fixed inset-0 z-50 flex justify-end bg-black/70 backdrop-blur-sm"
        @click.self="closeCart"
      >
        <div class="flex h-full w-full max-w-md flex-col border-l-2 border-white/10 bg-fenix-black p-6 shadow-2xl">
          <div class="flex items-center justify-between">
            <h2 class="font-display text-2xl tracking-wide">
              Seu <span class="text-fenix-orange">Carrinho</span>
            </h2>
            <button
              type="button"
              class="flex h-9 w-9 items-center justify-center border-2 border-white/15 text-white/60 transition hover:border-fenix-orange/60 hover:text-white"
              @click="closeCart"
            >
              ✕
            </button>
          </div>

          <div class="mt-6 flex-1 overflow-y-auto">
            <p v-if="cartItems.length === 0" class="text-white/50">Seu carrinho está vazio.</p>

            <div v-else class="flex flex-col gap-4">
              <div v-for="item in cartItems" :key="item.id" class="card flex items-center gap-4 p-4">
                <div class="flex-1">
                  <h3 class="font-display tracking-wide">{{ item.name }}</h3>
                  <p class="mt-1 text-sm text-white/50">{{ formatPrice(item.price) }} / un.</p>
                </div>

                <div class="flex items-center gap-2">
                  <button
                    type="button"
                    class="flex h-7 w-7 items-center justify-center border-2 border-white/15 text-sm font-bold transition hover:border-fenix-orange/60"
                    @click="updateQuantity(item.id, item.quantity - 1)"
                  >
                    -
                  </button>
                  <span class="w-6 text-center text-sm font-bold">{{ item.quantity }}</span>
                  <button
                    type="button"
                    class="flex h-7 w-7 items-center justify-center border-2 border-white/15 text-sm font-bold transition hover:border-fenix-orange/60"
                    @click="updateQuantity(item.id, item.quantity + 1)"
                  >
                    +
                  </button>
                </div>

                <div class="w-20 text-right font-display text-fenix-gold">
                  {{ formatPrice(item.price * item.quantity) }}
                </div>

                <button
                  type="button"
                  class="text-white/40 transition hover:text-fenix-red"
                  title="Remover item"
                  @click="removeItem(item.id)"
                >
                  🗑️
                </button>
              </div>
            </div>
          </div>

          <div class="mt-6 border-t-2 border-white/10 pt-6">
            <div class="flex items-center justify-between">
              <span class="section-label">Total</span>
              <span class="font-display text-2xl text-fenix-gold">{{ formatPrice(totalPrice) }}</span>
            </div>

            <button
              type="button"
              class="btn-fire mt-5 w-full !py-3 !text-base disabled:cursor-not-allowed disabled:opacity-50"
              :disabled="cartItems.length === 0 || isCheckingOut"
              @click="checkout"
            >
              <span class="btn-label">{{ isCheckingOut ? 'Enviando...' : 'Fechar Pedido' }}</span>
            </button>
          </div>
        </div>
      </div>
    </Transition>
  </div>
</template>

<style scoped>
.grid-item-enter-active,
.grid-item-leave-active {
  transition: all 0.3s ease;
}
.grid-item-enter-from,
.grid-item-leave-to {
  opacity: 0;
  transform: translateY(16px) scale(0.97);
}

.cart-fade-enter-active,
.cart-fade-leave-active {
  transition: opacity 0.2s ease;
}
.cart-fade-enter-from,
.cart-fade-leave-to {
  opacity: 0;
}
</style>
