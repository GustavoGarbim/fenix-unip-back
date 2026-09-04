<script setup>
import { computed, onMounted, ref } from 'vue'
import { useDemoMode } from '../composables/useDemoMode'
import { get } from '../services/api'

const { triggerDemo } = useDemoMode()

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

const cartCount = ref(0)

function addToCart(product) {
  cartCount.value += 1
  triggerDemo(`Garantir ${product.name}`)
}

function checkout() {
  triggerDemo('Fechar Pedido')
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
        @click="checkout"
      >
        <svg class="h-5 w-5" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
          <path stroke-linecap="round" stroke-linejoin="round" d="M3 3h2l.4 2M7 13h10l4-8H5.4M7 13L5.4 5M7 13l-2.293 2.293c-.63.63-.184 1.707.707 1.707H17m-10 0a2 2 0 100 4 2 2 0 000-4zm10 0a2 2 0 100 4 2 2 0 000-4z" />
        </svg>
        Carrinho
        <span
          v-if="cartCount > 0"
          class="absolute -right-2 -top-2 flex h-6 w-6 items-center justify-center rounded-full bg-fenix-red text-xs font-bold"
        >
          {{ cartCount }}
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
</style>
