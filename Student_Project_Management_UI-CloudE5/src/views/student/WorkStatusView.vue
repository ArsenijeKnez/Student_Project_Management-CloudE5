<script setup>
import { ref, onMounted } from "vue";
import submissionService from "@/services/submissionService";

const workStatus = ref([]);
const message = ref("");

const statusLabels = {
  0: "Submitted",
  1: "UnderAnalysis",
  2: "FeedbackReady",
  3: "Rejected",
};

const formatDate = (dateString) => {
  if (!dateString) return "N/A";
  return new Date(dateString).toLocaleString();
};

const fetchWorkStatus = async () => {
  const user = JSON.parse(sessionStorage.getItem("user"));
  const studentId = user?.id;

  if (!studentId) {
    message.value = "Student ID is required.";
    return;
  }

  try {
    const response = await submissionService.getWorkStatus(studentId);
    if (response.status === 200) {
      workStatus.value = response.data;
    } else {
      message.value = response.message;
    }
  } catch (error) {
    message.value = "Failed to fetch work status.";
  }
};

onMounted(fetchWorkStatus);
</script>

<template>
  <div>
    <h2>Work Status</h2>
    <p v-if="message">{{ message }}</p>
    <ul v-if="workStatus.length">
      <li v-for="status in workStatus" :key="status.submissionDate">
        <strong>{{ status.title }}</strong> - 
        <span>{{ statusLabels[status.status] }}</span> - 
        <span>Submitted: {{ formatDate(status.submissionDate) }}</span>
      </li>
    </ul>
  </div>
</template>
