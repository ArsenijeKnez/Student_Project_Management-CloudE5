<script setup>
import { ref } from 'vue'
import adminSettingsService from '@/services/adminSettingsService'
import ManagePrompts from '@/components/admin/ManagePrompts.vue'

const maxSubmissions = ref(null)
const analysisInterval = ref('')
const message = ref('')

const updateLimit = async () => {
  const response = await adminSettingsService.SetDailySubmissionLimit(maxSubmissions.value)
  message.value = response.success ? 'Submission limit updated successfully.' : response.message
}

const updateInterval = async () => {
  const response = await adminSettingsService.SetAnalysisInterval(analysisInterval.value)
  message.value = response.success ? 'Analysis interval updated successfully.' : response.message
}
</script>

<template>
  <h1>System Settings</h1>
  <div class="input-group">
    <label>
      Max student submissions in a day:
      <input type="number" v-model="maxSubmissions" placeholder="Submissions" />
      <button @click="updateLimit">Save</button>
    </label>
    <label>
      Automatic submission analysis interval:
      <input type="text" v-model="analysisInterval" placeholder="hh:mm:ss" />
      <button @click="updateInterval">Save</button>
    </label>
    <p v-if="message">{{ message }}</p>
    <ManagePrompts />
  </div>
</template>
