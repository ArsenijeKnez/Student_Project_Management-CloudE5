<script setup>
import { ref, onMounted, defineProps } from 'vue';
import reportsService from '@/services/reportsService';

const props = defineProps({
  username: {
    type: String,
    required: true,
  },
  studentId: {
    type: String,
    required: true,
  },
});

const studentProgress = ref(null);
const loading = ref(true);
const error = ref(null);

const fetchStudentProgress = async () => {
  try {
    studentProgress.value = await reportsService.getStudentProgress(props.studentId);
  } catch (err) {
    error.value = 'Failed to load student progress';
  } finally {
    loading.value = false;
  }
};

onMounted(fetchStudentProgress);
</script>

<template>
  <div class="progress-container">
    <h2>{{ props.username }} Progress</h2>

    <div v-if="loading">Loading...</div>
    <div v-else-if="error">{{ error }}</div>
    <div v-else>
      <p><strong>Student ID:</strong> {{ studentProgress.studentId }}</p>
      <p><strong>Total Works:</strong> {{ studentProgress.totalWorks }}</p>
      <p><strong>Average Score:</strong> {{ studentProgress.averageScore.toFixed(2) }}</p>

      <h3>Score History</h3>
      <ul>
        <li v-for="(score, date) in studentProgress.scoreHistory" :key="date">
          <strong>{{ new Date(date).toLocaleDateString() }}:</strong> {{ score }}
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped>
.progress-container {
  padding: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
  max-width: 500px;
}
h2, h3 {
  margin-bottom: 8px;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  background: #141414;
  margin: 4px 0;
  padding: 8px;
  border-radius: 4px;
  
}
</style>
