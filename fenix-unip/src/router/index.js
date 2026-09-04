import { createRouter, createWebHistory } from 'vue-router'
import HomeView from '../views/HomeView.vue'
import LojaView from '../views/LojaView.vue'
import PortalView from '../views/PortalView.vue'
import EsportivaView from '../views/EsportivaView.vue'
import SugestoesView from '../views/SugestoesView.vue'
import JornalView from '../views/JornalView.vue'
import AdminLoginView from '../views/admin/AdminLoginView.vue'
import AdminDashboardView from '../views/admin/AdminDashboardView.vue'
import { useAdminAuth } from '../composables/useAdminAuth'

const router = createRouter({
  history: createWebHistory(),
  scrollBehavior() {
    return { top: 0, behavior: 'smooth' }
  },
  routes: [
    { path: '/', name: 'home', component: HomeView, meta: { label: 'Início' } },
    { path: '/loja', name: 'loja', component: LojaView, meta: { label: 'Loja' } },
    { path: '/portal', name: 'portal', component: PortalView, meta: { label: 'Portal do Sócio' } },
    { path: '/esportiva', name: 'esportiva', component: EsportivaView, meta: { label: 'Gestão Esportiva' } },
    { path: '/jornal', name: 'jornal', component: JornalView, meta: { label: 'Jornal' } },
    { path: '/sugestoes', name: 'sugestoes', component: SugestoesView, meta: { label: 'Sugestões' } },
    { path: '/admin/login', name: 'admin-login', component: AdminLoginView },
    { path: '/admin', name: 'admin', component: AdminDashboardView, meta: { requiresAdmin: true } },
  ],
})

router.beforeEach((to) => {
  if (to.meta.requiresAdmin) {
    const { isAdmin } = useAdminAuth()
    if (!isAdmin.value) return { path: '/admin/login' }
  }
})

export default router
