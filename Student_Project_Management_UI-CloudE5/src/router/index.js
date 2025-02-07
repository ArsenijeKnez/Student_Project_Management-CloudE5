import { createRouter, createWebHistory } from 'vue-router';
import HomeView from '../views/HomeView.vue';

const router = createRouter({
  history: createWebHistory(import.meta.env.BASE_URL),
  routes: [
    { path: '/', name: 'home', component: HomeView },
    { path: '/login', name: 'login', component: () => import('../views/LoginView.vue') },
    {
      path: '/admin/users',
      name: 'user-management',
      component: () => import('../views/admin/UserManagementView.vue'),
      meta: { requiresAuth: true, role: 'Admin' }, 
    },
    {
      path: '/admin/settings',
      name: 'system-settings',
      component: () => import('../views/admin/SystemSettingsView.vue'),
      meta: { requiresAuth: true, role: 'Admin' },
    },
    {
      path: '/admin/reports',
      name: 'reports',
      component: () => import('../views/admin/ReportsView.vue'),
      meta: { requiresAuth: true, role: 'Admin' },
    },
  ],
});

router.beforeEach((to) => {
  const user = JSON.parse(sessionStorage.getItem('user'));

  if (to.meta.requiresAuth && (!user || user.role !== to.meta.role)) {
    return '/login'; 
  }
});

export default router;
