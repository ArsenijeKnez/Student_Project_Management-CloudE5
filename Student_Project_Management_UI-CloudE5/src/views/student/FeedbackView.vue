<script setup>
import { ref } from "vue";
import submissionService from "@/services/submissionService";

      //MAKE INTO A COMPONENT!!!!!!!!!!!
      
const studentWorkId = ref("");
const feedback = ref(null);
const message = ref("");

const fetchFeedback = async () => {
  if (!studentWorkId.value) {
    message.value = "Student Work ID is required.";
    return;
  }

  const response = await submissionService.getFeedback(studentWorkId.value);
  if (response.success) {
    feedback.value = response.data;
  } else {
    message.value = response.message;
  }
};
</script>

<template>
  <div>
    <h2>Feedback</h2>
    <input v-model="studentWorkId" type="text" placeholder="Student Work ID" />
    <button @click="fetchFeedback">Get Feedback</button>
    <p v-if="message">{{ message }}</p>
    <p v-if="feedback">{{ feedback.comments }}</p>
  </div>
</template>
