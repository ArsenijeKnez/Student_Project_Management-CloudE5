<script setup>
import StudentProgress from '@/components/StudentProgress.vue';
import { ref,  } from 'vue';
import { useRoute } from "vue-router";

const route = useRoute();
const studentId = ref(route.query.id);
const username = ref(route.query.username);
</script>

<template>
    <div>
      <StudentProgress v-if="studentId && username"  :username="username" :student-id="studentId"/>
    </div>
  </template>
  