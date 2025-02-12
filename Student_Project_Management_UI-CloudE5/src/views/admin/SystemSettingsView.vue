<script setup>
import { ref } from 'vue';
import adminSettingsService from "@/services/adminSettingsService";

const maxSubmissions = ref(null);
const message = ref("");

const updateLimit = async () => {
  const response = await adminSettingsService.SetDailySubmissionLimit(maxSubmissions.value);
  message.value = response.success ? "Submission limit updated successfully." : response.message;
};
</script>

<template>
  <div class="input-group">
    <h1>System Settings</h1>
    <label>
      Max student submissions in a day:
      <input type="number" v-model="maxSubmissions" placeholder="submissions"/>
    </label>
    <button @click="updateLimit">Save</button>
    <p v-if="message">{{ message }}</p>
  </div>
</template>
