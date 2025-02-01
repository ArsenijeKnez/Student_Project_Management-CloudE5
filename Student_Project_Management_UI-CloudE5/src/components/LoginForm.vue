<script setup>
import { ref } from 'vue';
import InputField from '@/components/InputField.vue';

const email = ref('');
const password = ref('');
const errorMessage = ref('');

const handleLogin = () => {
  if (!email.value || !password.value) {
    errorMessage.value = 'Please fill in all fields.';
    return;
  }
  if (!email.value.includes('@')) {
    errorMessage.value = 'Enter a valid email address.';
    return;
  }
  alert('correct!');
};
</script>

<template>
  <form class="login-form" @submit.prevent="handleLogin">
    <InputField v-model="email" label="Email" type="email" required />
    <InputField v-model="password" label="Password" type="password" required />

    <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>

    <button type="submit">Login</button>
  </form>
</template>

<style scoped>
.login-form {
  display: flex;
  flex-direction: column;
  gap: 1rem;
  width: 100%;
  max-width: 300px;
}

button {
  background-color: #42b883;
  color: white;
  border: none;
  padding: 0.75rem;
  cursor: pointer;
}

button:hover {
  background-color: #358a68;
}

.error-message {
  color: red;
  font-size: 0.9rem;
}
</style>
