<script setup>
import { useDemoMode } from '../composables/useDemoMode'

const { state, dismissToast } = useDemoMode()
</script>

<template>
  <div class="pointer-events-none fixed inset-x-0 top-4 z-[100] flex flex-col items-center gap-3 px-4 sm:top-6">
    <TransitionGroup name="toast">
      <div
        v-for="toast in state.toasts"
        :key="toast.id"
        class="pointer-events-auto flex w-full max-w-md items-start gap-3 rounded-2xl border border-fenix-orange/30 bg-fenix-black/90 p-4 shadow-ember-lg backdrop-blur-md"
      >
        <div class="mt-0.5 flex h-9 w-9 shrink-0 items-center justify-center rounded-full bg-fenix-red">
          <svg class="h-5 w-5 text-white" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M13 10V3L4 14h7v7l9-11h-7z" />
          </svg>
        </div>
        <div class="min-w-0 flex-1">
          <p class="font-display text-base tracking-wide text-fenix-gold">{{ toast.title }}</p>
          <p class="mt-0.5 text-sm leading-snug text-white/80">{{ toast.message }}</p>
        </div>
        <button
          type="button"
          class="ml-1 shrink-0 rounded-full p-1 text-white/40 transition hover:bg-white/10 hover:text-white"
          @click="dismissToast(toast.id)"
        >
          <svg class="h-4 w-4" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2">
            <path stroke-linecap="round" stroke-linejoin="round" d="M6 18L18 6M6 6l12 12" />
          </svg>
        </button>
      </div>
    </TransitionGroup>
  </div>
</template>

<style scoped>
.toast-enter-active,
.toast-leave-active {
  transition: all 0.35s cubic-bezier(0.34, 1.56, 0.64, 1);
}
.toast-enter-from {
  opacity: 0;
  transform: translateY(-24px) scale(0.95);
}
.toast-leave-to {
  opacity: 0;
  transform: translateY(-12px) scale(0.95);
}
.toast-move {
  transition: transform 0.35s ease;
}
</style>
