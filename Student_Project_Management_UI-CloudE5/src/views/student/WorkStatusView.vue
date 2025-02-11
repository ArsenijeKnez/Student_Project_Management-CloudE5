<script setup>
import { ref, onMounted } from "vue";
import { useRouter } from "vue-router";
import submissionService from "@/services/submissionService";
import { statusLabels, formatDate } from "@/components/LabelHelper";

const router = useRouter();
const workStatus = ref([]);
const message = ref("");


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
      message.value = response.message || "Failed to fetch work status.";
    }
  } catch (error) {
    message.value = "Failed to fetch work status.";
  }
};

const showWork = (id) => {
  router.push({ name: "update-work", query: { id: id }  });
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
        <span>Submitted: {{ formatDate(status.submissionDate) }}</span> -
        <span>
          <button @click="showWork(status.workId)">Show</button>
        </span>
      </li>
    </ul>
  </div>
</template>
