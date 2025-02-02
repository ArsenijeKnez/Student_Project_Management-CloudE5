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
  <form @submit.prevent="handleLogin">
    <InputField v-model="email" label="Email" type="email" required />
    <InputField v-model="password" label="Password" type="password" required />

    <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>

    <button type="submit">Login</button>
  </form>
</template>
