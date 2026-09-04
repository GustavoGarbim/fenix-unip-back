<script setup>
defineProps({
  text: { type: String, default: 'RAÇA • INTEGRAÇÃO • ESPORTE • UNIP •' },
})
</script>

<template>
  <div class="overflow-hidden border-y-2 border-fenix-red/50 bg-fenix-black py-4">
    <div class="marquee-row">
      <span v-for="n in 8" :key="n" class="font-display text-xl uppercase tracking-wider text-white/80 sm:text-2xl">
        {{ text }}
      </span>
    </div>
  </div>
</template>
