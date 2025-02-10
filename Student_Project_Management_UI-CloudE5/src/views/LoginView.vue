<script setup>
import { ref } from 'vue';
import { useRouter } from 'vue-router';
import LoginForm from '@/components/LoginForm.vue';
import AuthService from '@/services/authService.js'; 

const router = useRouter();
const errorMessage = ref('');

const handleLogin = async (userData) => {
  const response = await AuthService.login(userData);
  console.log(response);
  if (response.status == 200) {

    sessionStorage.setItem('user', JSON.stringify(response.data));
    router.push('/').then(() => {
  window.location.reload();
  });

  } else {
    errorMessage.value = response.message || 'Login failed';
  }
};
</script>

<template>
  <div class="login-container">
    <h1>Login</h1>
    <LoginForm @submit="handleLogin" />
    <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
  </div>
</template>

<style scoped>
.login-container {
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  height: 100vh;
}

.error-message {
  color: red;
  margin-top: 10px;
}
</style>
