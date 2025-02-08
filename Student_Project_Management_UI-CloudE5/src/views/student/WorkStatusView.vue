<script setup>
import { ref } from "vue";
import submissionService from "@/services/submissionService";

const studentId = ref("");
const workStatus = ref(null);
const message = ref("");

const fetchWorkStatus = async () => {
  if (!studentId.value) {
    message.value = "Student ID is required.";
    return;
  }

  const response = await submissionService.getWorkStatus(studentId.value);
  if (response.success) {
    workStatus.value = response.data;
  } else {
    message.value = response.message;
  }
};
</script>

<template>
  <div>
    <h2>Work Status</h2>
    <input v-model="studentId" type="text" placeholder="Student ID" />
    <button @click="fetchWorkStatus">Get Status</button>
    <p v-if="message">{{ message }}</p>
    <ul v-if="workStatus">
      <li v-for="status in workStatus" :key="status.id">
        {{ status.title }} - {{ status.status }}
      </li>
    </ul>
  </div>
</template>
