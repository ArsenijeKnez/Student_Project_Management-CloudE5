<script setup>
import { ref } from 'vue';

const usernameOrEmail = ref('');
const password = ref('');
const emit = defineEmits(['submit']);

const handleLogin = () => {
  if (!usernameOrEmail.value || !password.value) return;
  emit('submit', { usernameOrEmail: usernameOrEmail.value, password: password.value });
};
</script>

<template>
  <form @submit.prevent="handleLogin">
    <input v-model="usernameOrEmail" type="text" placeholder="Username or Email" required />
    <input v-model="password" type="password" placeholder="Password" required />
    <button type="submit">Login</button>
  </form>
</template>

<style scoped>
form {
  display: flex;
  flex-direction: column;
  gap: 25px;
}
</style>
