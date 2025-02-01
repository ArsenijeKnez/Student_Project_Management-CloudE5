<script setup>
import LoginForm from '@/components/LoginForm.vue';
</script>

<template>
  <div class="login-container">
    <h1>Login</h1>
    <LoginForm />
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
</style>
