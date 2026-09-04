<script setup>
import logo from '../img/fenix-unip.jpg'
import { useDemoMode } from '../composables/useDemoMode'

const { triggerDemo } = useDemoMode()

const student = {
  name: 'Gustavo Garbim',
  ra: 'N123456-7',
  course: 'Engenharia de Software',
  memberSince: 'Março 2024',
  status: 'Ativo',
  plan: 'Plano Anual Fênix',
}

const timeline = [
  { label: 'Mensalidade Ago/26', status: 'paga', date: '05/08/2026' },
  { label: 'Mensalidade Set/26', status: 'pendente', date: 'vence em 05/09/2026' },
  { label: 'Mensalidade Out/26', status: 'futura', date: '05/10/2026' },
]

const perks = [
  'Entrada gratuita em jogos oficiais',
  'Desconto de 20% na loja virtual',
  'Prioridade em inscrições de seletivas',
  'Acesso a eventos exclusivos de sócios',
]

function statusClasses(status) {
  return {
    paga: 'bg-emerald-500/15 text-emerald-400 border-emerald-500/30',
    pendente: 'bg-fenix-gold/15 text-fenix-gold border-fenix-gold/30',
    futura: 'bg-white/5 text-white/40 border-white/10',
  }[status]
}
</script>

<template>
  <div class="mx-auto max-w-7xl px-5 py-16 sm:px-8 lg:py-24">
    <span class="section-label">Portal do Sócio</span>
    <h1 class="mt-5 font-display text-4xl tracking-wide sm:text-5xl">
      Sua conexão com a <span class="text-fenix-orange">Fênix</span>
    </h1>
    <p class="mt-3 max-w-lg text-white/60">Perfil, mensalidade e carteirinha digital em um dashboard exclusivo para sócios.</p>

    <div class="mt-12 grid gap-6 lg:grid-cols-[1.1fr_0.9fr]">
      <div class="animate-fade-up">
        <div class="relative overflow-hidden border-4 border-black bg-fire-gradient p-8 shadow-ember-lg">
          <div class="absolute -right-10 -top-10 h-48 w-48 rounded-full bg-white/10 blur-2xl" />
          <div class="absolute -bottom-16 -left-10 h-56 w-56 rounded-full bg-black/20 blur-3xl" />

          <div class="relative flex items-start justify-between">
            <div class="flex items-center gap-3">
              <img :src="logo" alt="Fênix UNIP" class="h-12 w-12 shrink-0 rounded-full object-contain ring-2 ring-white/40" />
              <div>
                <p class="text-xs font-semibold uppercase tracking-[0.25em] text-white/80">Carteirinha Digital</p>
                <p class="mt-1 font-display text-2xl tracking-wide">FÊNIX UNIP</p>
              </div>
            </div>
            <span class="-rotate-3 border-2 border-white/50 bg-white/15 px-3 py-1 text-xs font-bold uppercase tracking-wide backdrop-blur">
              {{ student.status }}
            </span>
          </div>

          <div class="relative mt-10 flex items-end justify-between gap-6">
            <div>
              <p class="font-display text-2xl tracking-wide">{{ student.name }}</p>
              <p class="mt-1 text-sm text-white/85">RA {{ student.ra }}</p>
              <p class="text-sm text-white/85">{{ student.course }}</p>
              <p class="mt-4 text-xs uppercase tracking-wider text-white/70">{{ student.plan }} &middot; desde {{ student.memberSince }}</p>
            </div>

            <div class="flex h-24 w-24 shrink-0 items-center justify-center border-2 border-black bg-white p-2 shadow-lg">
              <svg viewBox="0 0 100 100" class="h-full w-full text-fenix-black">
                <rect x="4" y="4" width="26" height="26" fill="none" stroke="currentColor" stroke-width="6" />
                <rect x="70" y="4" width="26" height="26" fill="none" stroke="currentColor" stroke-width="6" />
                <rect x="4" y="70" width="26" height="26" fill="none" stroke="currentColor" stroke-width="6" />
                <rect x="12" y="12" width="10" height="10" fill="currentColor" />
                <rect x="78" y="12" width="10" height="10" fill="currentColor" />
                <rect x="12" y="78" width="10" height="10" fill="currentColor" />
                <rect x="40" y="10" width="8" height="8" fill="currentColor" />
                <rect x="54" y="10" width="8" height="8" fill="currentColor" />
                <rect x="40" y="40" width="8" height="8" fill="currentColor" />
                <rect x="52" y="40" width="8" height="8" fill="currentColor" />
                <rect x="64" y="40" width="8" height="8" fill="currentColor" />
                <rect x="40" y="52" width="8" height="8" fill="currentColor" />
                <rect x="64" y="52" width="8" height="8" fill="currentColor" />
                <rect x="40" y="64" width="8" height="8" fill="currentColor" />
                <rect x="52" y="64" width="8" height="8" fill="currentColor" />
                <rect x="76" y="52" width="8" height="8" fill="currentColor" />
                <rect x="76" y="76" width="18" height="8" fill="currentColor" />
                <rect x="52" y="88" width="8" height="8" fill="currentColor" />
              </svg>
            </div>
          </div>
        </div>

        <div class="card mt-6 p-6">
          <p class="font-display text-lg tracking-wide">Benefícios do Plano</p>
          <ul class="mt-4 space-y-3">
            <li v-for="perk in perks" :key="perk" class="flex items-start gap-3 text-sm text-white/70">
              <svg class="mt-0.5 h-5 w-5 shrink-0 text-fenix-orange" fill="none" viewBox="0 0 24 24" stroke="currentColor" stroke-width="2.5">
                <path stroke-linecap="round" stroke-linejoin="round" d="M5 13l4 4L19 7" />
              </svg>
              {{ perk }}
            </li>
          </ul>
        </div>
      </div>

      <div class="animate-fade-up flex flex-col gap-6" style="animation-delay: 0.1s">
        <div class="card p-6">
          <div class="flex items-center justify-between">
            <p class="font-display text-lg tracking-wide">Status da Mensalidade</p>
            <span class="border-2 border-fenix-gold/30 bg-fenix-gold/10 px-3 py-1 text-xs font-bold uppercase text-fenix-gold">
              Pendente
            </span>
          </div>
          <ol class="mt-6 space-y-4">
            <li v-for="item in timeline" :key="item.label" class="flex items-center justify-between border-2 px-4 py-3" :class="statusClasses(item.status)">
              <div>
                <p class="text-sm font-semibold text-white">{{ item.label }}</p>
                <p class="text-xs text-white/50">{{ item.date }}</p>
              </div>
              <span class="text-xs font-bold uppercase tracking-wide">{{ item.status }}</span>
            </li>
          </ol>
          <button type="button" class="btn-fire mt-6 w-full !py-3 !text-base" @click="triggerDemo('Pagar mensalidade')">
            <span class="btn-label">Regularizar Mensalidade</span>
          </button>
        </div>

        <div class="card p-6">
          <p class="font-display text-lg tracking-wide">Acesso Rápido</p>
          <div class="mt-4 grid grid-cols-2 gap-3">
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="triggerDemo('Baixar carteirinha')">
              <span class="btn-label">Baixar Carteirinha</span>
            </button>
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="triggerDemo('Editar perfil')">
              <span class="btn-label">Editar Perfil</span>
            </button>
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="triggerDemo('Histórico de pagamentos')">
              <span class="btn-label">Histórico</span>
            </button>
            <button type="button" class="btn-ghost !py-3 !text-sm" @click="triggerDemo('Fazer Login')">
              <span class="btn-label">Trocar Conta</span>
            </button>
          </div>
        </div>
      </div>
    </div>
  </div>
</template>
