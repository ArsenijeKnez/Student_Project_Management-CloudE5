<script setup>
import { ref, onMounted, computed } from 'vue'
import reportsService from '@/services/reportsService'
import { Chart, registerables } from 'chart.js'
import { LineChart } from 'vue-chart-3'

Chart.register(...registerables)

const classProgress = ref(null)
const loading = ref(true)
const error = ref(null)

const fetchClassProgress = async () => {
  try {
    const response = await reportsService.getClassProgress()
    classProgress.value = response
    loading.value = false
  } catch (err) {
    error.value = 'Failed to load class progress'
  } finally {
    loading.value = false
  }
}

onMounted(fetchClassProgress)

const chartData = computed(() => {
  if (!classProgress.value || !classProgress.value.studentProgressList) return null

  return {
    labels: classProgress.value.studentProgressList.map((sp) => sp.studentId),
    datasets: [
      {
        label: 'Average Score',
        data: classProgress.value.studentProgressList.map((sp) => sp.averageScore),
        borderColor: 'blue',
        backgroundColor: 'rgba(0, 0, 255, 0.2)',
        fill: true,
      },
    ],
  }
})

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  scales: {
    y: {
      beginAtZero: true,
      max: 100,
    },
  },
}
</script>

<template>
  <div>
    <h2>Class Progress</h2>

    <div v-if="loading">Loading class progress...</div>
    <div v-else-if="error" class="error">{{ error }}</div>

    <div v-else>
      <p>Total Students: {{ classProgress.totalStudents }}</p>
      <p>Average Class Score: {{ classProgress.averageClassScore }}</p>

      <div v-if="chartData">
        <LineChart :chart-data="chartData" :chart-options="chartOptions" />
      </div>
    </div>
  </div>
</template>

<style scoped>
.error {
  color: red;
  font-weight: bold;
}
</style>
