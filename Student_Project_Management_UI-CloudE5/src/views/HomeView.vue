<script setup>
import { ref, onMounted } from 'vue';

const user = ref(null);

onMounted(() => {
  const storedUser = sessionStorage.getItem('user');
  if (storedUser) {
    user.value = JSON.parse(storedUser); 
  }
});
</script>

<template>
  <div class="home">
    <h1>Welcome to Student_Project_Management!</h1>
    
    <div v-if="user">
      <p><strong>Username:</strong> {{ user.username }}</p>
      <p><strong>Email:</strong> {{ user.email }}</p>
      <p><strong>First Name:</strong> {{ user.firstName }}</p>
      <p><strong>Last Name:</strong> {{ user.lastName }}</p>
      <p><strong>Role:</strong> {{ user.role }}</p>
    </div>
    
    <div v-else>
      <p>Please log in.</p>
    </div>
  </div>
</template>
