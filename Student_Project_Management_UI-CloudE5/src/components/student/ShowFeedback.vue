<script setup>
import { defineProps } from "vue";

const props = defineProps({
  feedback: {
    type: Object,
    required: true,
  },

});
</script>

<template>
  <div class="feedback-container">
    <label>
      <strong>Score:</strong>
      <span>{{ feedback.score }}</span>
    </label>

    <div class="feedback-section">
      <h3>Errors</h3>
      <ul>
        <li v-for="(error, index) in feedback.errors" :key="index">
          {{ error }}
        </li>
      </ul>
    </div>

    <div class="feedback-section">
      <h3>Improvement Suggestions</h3>
      <ul>
        <li v-for="(suggestion, index) in feedback.improvementSuggestions" :key="index">
          {{ suggestion }}
        </li>
      </ul>
    </div>

    <div class="feedback-section">
      <h3>Recommendations</h3>
      <ul>
        <li v-for="(recommendation, index) in feedback.recommendations" :key="index">
          {{ recommendation }}
        </li>
      </ul>
    </div>
  </div>
</template>

<style scoped>
.feedback-container {
  padding: 16px;
  border: 1px solid #ccc;
  border-radius: 8px;
  max-width: 500px;
}
.feedback-section {
  margin-top: 12px;
}
ul {
  padding-left: 16px;
}
</style>
