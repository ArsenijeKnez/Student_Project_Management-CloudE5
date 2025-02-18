<script setup>
import { ref, computed } from 'vue';
import { useRouter } from 'vue-router';

const router = useRouter();
const user = ref(JSON.parse(sessionStorage.getItem('user')) || null);

const isAdmin = computed(() => user.value?.role === 'Admin');
const isStudent = computed(() => user.value?.role === 'Student');
const isProfessor = computed(() => user.value?.role === 'Professor');

const logout = () => {
  sessionStorage.removeItem('user');
  user.value = null;
  router.push('/login');
};
</script>

<template>
  <header>
    <img alt="Vue logo" class="logo" src="@/assets/logo.svg" width="125" height="125" />
    <nav>
      <RouterLink to="/">Home</RouterLink>
      <RouterLink to="/login" v-if="!user">Login</RouterLink>
      <template v-if="isAdmin">
        <RouterLink to="/admin/users">Manage Users</RouterLink>
        <RouterLink to="/admin/settings">System Settings</RouterLink>
      </template>
      <template v-if="isStudent">
        <RouterLink to="/student/upload">Upload Work</RouterLink>
        <RouterLink to="/student/status">Work Status</RouterLink>
        <RouterLink to="/student/progress">Progress</RouterLink>
      </template>
      <template v-if="isProfessor">
        <RouterLink to="/professor/students">View Students</RouterLink>
      </template>
      <button @click="logout" v-if="user">Logout</button>
    </nav>
  </header>

  <RouterView />
</template>

<style scoped>

header {
  position: fixed;
  top: 0;
  left: 0;
  padding: 1rem;
  display: flex;
  flex-direction: column;
  align-items: center;
  position:absolute;
  width: 180px;
  z-index: 1000;
}

.logo {
  width: 80px;
  height: 80px;
  margin-bottom: 1rem;
}

nav {
  display: flex;
  flex-direction: column;
  text-align: center;
}

nav a {
  text-decoration: none;
  padding: 0.5rem 1rem;
  border-bottom: 1px solid var(--color-border);
}

nav a:last-of-type {
  border-bottom: none;
}

nav a.router-link-exact-active {
  font-weight: bold;
  color: var(--color-text);
}

main {
  display: flex;
  justify-content: center;
  align-items: center;
  height: 100vh;
  margin-left: 180px;
  width: 100%;
  text-align: center;
}
</style>
