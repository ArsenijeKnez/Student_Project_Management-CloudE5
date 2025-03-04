<script setup>
import StudentProgress from '@/components/StudentProgress.vue';
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
    <div>
      <h1>Progress</h1>
      <StudentProgress v-if="user"  :username="user.username" :student-id="user.id"/>
    </div>
  </template>
  