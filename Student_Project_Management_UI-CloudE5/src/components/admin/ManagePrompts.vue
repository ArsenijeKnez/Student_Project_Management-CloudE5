<script setup>
import { ref, onMounted } from 'vue'
import adminSettingsService from '@/services/adminSettingsService'

const prompts = ref({
  error: '',
  improvement: '',
  score: '',
})
const loading = ref(false)
const message = ref('')

const fetchPrompts = async () => {
  try {
    loading.value = true
    const response = adminSettingsService.fetchPrompts()
    prompts.value = response
  } catch (error) {
    message.value = 'Failed to fetch prompts'
  } finally {
    loading.value = false
  }
}

const updatePrompts = async () => {
  try {
    loading.value = true
    await adminSettingsService.SetAnalysisInterval(prompts.value)
    message.value = 'Prompts updated successfully'
  } catch (error) {
    message.value = 'Failed to update prompts'
  } finally {
    loading.value = false
  }
}

onMounted(fetchPrompts)
</script>

<template>
  <div class="prompt-editor">
    <h2>Edit Prompts</h2>
    <div v-if="message">{{ message }}</div>
    <div v-if="loading">Loading...</div>
    <div v-else>
      <label>Error Prompt</label>
      <textarea v-model="prompts.error" />

      <label>Improvement Prompt</label>
      <textarea v-model="prompts.improvement" />

      <label>Score Prompt</label>
      <textarea v-model="prompts.score" />

      <button @click="updatePrompts">Save Changes</button>
    </div>
  </div>
</template>

<style scoped>
textarea {
  width: 100%;
  height: 80px;
  margin-bottom: 10px;
  padding: 8px;
  border-radius: 4px;
  border: 1px solid #ccc;
}
</style>
