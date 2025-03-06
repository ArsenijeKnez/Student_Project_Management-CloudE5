<script setup>
import { ref, onMounted, defineProps, computed } from 'vue'
import reportsService from '@/services/reportsService'
import { Chart, registerables } from 'chart.js'
import { LineChart } from 'vue-chart-3'

Chart.register(...registerables)

const props = defineProps({
  username: {
    type: String,
    required: true,
  },
  studentId: {
    type: String,
    required: true,
  },
})

const studentProgress = ref(null)
const loading = ref(true)
const error = ref(null)
const startDate = ref('')
const endDate = ref('')

const fetchStudentProgress = async () => {
  try {
    studentProgress.value = await reportsService.getStudentProgress(props.studentId)
  } catch (err) {
    error.value = 'Failed to load student progress'
  } finally {
    loading.value = false
  }
}

onMounted(fetchStudentProgress)

const filteredScoreHistory = computed(() => {
  if (!studentProgress.value || !studentProgress.value.scoreHistory) {
    return {}
  }

  const start = startDate.value ? new Date(startDate.value) : null
  const end = endDate.value ? new Date(endDate.value) : null

  return Object.fromEntries(
    Object.entries(studentProgress.value.scoreHistory).filter(([date]) => {
      const entryDate = new Date(date)
      return (!start || entryDate >= start) && (!end || entryDate <= end)
    }),
  )
})

const chartData = computed(() => {
  const sortedEntries = Object.entries(filteredScoreHistory.value).sort(
    ([dateA], [dateB]) => new Date(dateA) - new Date(dateB),
  )

  return {
    labels: sortedEntries.map(([date]) => new Date(date).toLocaleDateString()),
    datasets: [
      {
        label: 'Score History',
        data: sortedEntries.map(([, score]) => score),
        borderColor: '#4caf50',
        backgroundColor: 'rgba(76, 175, 80, 0.2)',
        tension: 0.3,
        fill: true,
      },
    ],
  }
})

const chartOptions = {
  responsive: true,
  maintainAspectRatio: false,
  plugins: {
    legend: {
      display: true,
    },
  },
  scales: {
    x: {
      title: {
        display: true,
        text: 'Date',
      },
    },
    y: {
      beginAtZero: true,
      title: {
        display: true,
        text: 'Score',
      },
    },
  },
}
</script>

<template>
  <div class="progress-container">
    <h2>{{ props.username }} Progress</h2>

    <div v-if="loading">Loading...</div>
    <div v-else-if="error">{{ error }}</div>
    <div v-else>
      <p><strong>Total Works:</strong> {{ studentProgress.totalWorks }}</p>
      <p><strong>Average Score:</strong> {{ studentProgress.averageScore.toFixed(2) }}</p>

      <h3>Filter Score History</h3>
      <div class="date-filters">
        <label>Start Date: <input type="date" v-model="startDate" /></label>
        <label>End Date: <input type="date" v-model="endDate" /></label>
      </div>

      <h3>Score History</h3>
      <ul>
        <li v-for="(score, date) in filteredScoreHistory" :key="date">
          <strong>{{ new Date(date).toLocaleDateString() }}:</strong> {{ score }}
        </li>
      </ul>

      <h3>Grade Progress Over Time</h3>
      <div class="chart-container">
        <LineChart :chart-data="chartData" :chart-options="chartOptions" />
      </div>
    </div>
  </div>
</template>

<style scoped>
.progress-container {
  padding: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
  max-width: 600px;
}
h2,
h3 {
  margin-bottom: 8px;
}
ul {
  list-style-type: none;
  padding: 0;
}
li {
  background: #141414;
  color: white;
  margin: 4px 0;
  padding: 8px;
  border-radius: 4px;
}
.chart-container {
  width: 100%;
  height: 100%;
  margin-top: 20px;
}
.date-filters {
  display: flex;
  gap: 10px;
  margin-bottom: 15px;
}
.date-filters label {
  font-weight: bold;
}
</style>
