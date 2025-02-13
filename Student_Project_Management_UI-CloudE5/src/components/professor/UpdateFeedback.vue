<script setup>
import { defineProps, defineEmits, ref, watch } from "vue";
import feedbackService from "@/services/analysisService";

const props = defineProps({
  feedback: {
    type: Object,
    required: true,
  },
  studentWorkId: {
    type: String,
    required: true,
  },
});

const emit = defineEmits(["update:feedback", "saveFeedback"]);
const localFeedback = ref({ ...props.feedback });

watch(localFeedback, (newValue) => {
  emit("update:feedback", newValue);
}, { deep: true });

const addItem = (list) => {
  list.push("");
};

const removeItem = (list, index) => {
  list.splice(index, 1);
};

const saveFeedback = async () => {
    try {
        console.log(props);
    const result = await feedbackService.updateFeedback(localFeedback.value, props.studentWorkId);
    if(result.status === 200)
        alert("Feedback updated successfully!");
  } catch (error) {
    alert("Failed to update feedback.");
  }
};
</script>

<template>
  <div class="feedback-container">
    <label>
      <strong>Score:</strong>
      <input type="number" v-model="localFeedback.score" step="0.1" min="0" max="100" />
    </label>

    <div class="feedback-section">
      <h3>Errors</h3>
      <ul>
        <li v-for="(error, index) in localFeedback.errors" :key="index">
          <input v-model="localFeedback.errors[index]" type="text" />
          <button @click="removeItem(localFeedback.errors, index)">Remove</button>
        </li>
      </ul>
      <button @click="addItem(localFeedback.errors)">Add Error</button>
    </div>

    <div class="feedback-section">
      <h3>Improvement Suggestions</h3>
      <ul>
        <li v-for="(suggestion, index) in localFeedback.improvementSuggestions" :key="index">
          <input v-model="localFeedback.improvementSuggestions[index]" type="text" />
          <button @click="removeItem(localFeedback.improvementSuggestions, index)">Remove</button>
        </li>
      </ul>
      <button @click="addItem(localFeedback.improvementSuggestions)">Add Suggestion</button>
    </div>

    <div class="feedback-section">
      <h3>Recommendations</h3>
      <ul>
        <li v-for="(recommendation, index) in localFeedback.recommendations" :key="index">
          <input v-model="localFeedback.recommendations[index]" type="text" />
          <button @click="removeItem(localFeedback.recommendations, index)">Remove</button>
        </li>
      </ul>
      <button @click="addItem(localFeedback.recommendations)">Add Recommendation</button>
    </div>

    <button class="save-button" @click="saveFeedback">Save</button>
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
input {
  margin: 4px;
  padding: 4px;
  width: 100%;
}
button {
  margin: 4px;
  padding: 4px 8px;
  cursor: pointer;
}
</style>
