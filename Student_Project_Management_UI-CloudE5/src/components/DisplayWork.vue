<script setup>
import { defineProps, ref } from "vue";
import { statusLabels, formatDate } from "@/components/LabelHelper";

const props = defineProps({
  studentWork: {
    type: Object,
    required: true,
  },
});

const studentWork = ref(props.studentWork)
</script>

<template>
  <div v-if="studentWork">
    <p><strong>Title:</strong> {{ studentWork.title }}</p>
    <p><strong>Status:</strong> {{ statusLabels[studentWork.status] || "Unknown" }}</p>
    <p><strong>Submission Date:</strong> {{ formatDate(studentWork.submissionDate) }}</p>
    <p v-if="studentWork.estimatedAnalysisCompletion !== null">
        <strong>Estimated Completion:</strong>
        {{ formatDate(studentWork.estimatedAnalysisCompletion) }}
    </p>

    <div v-if="studentWork.feedback">
      <h3>Feedback</h3>
      <p>{{ studentWork.feedback.comment || "No feedback available" }}</p>
    </div>

   <div v-if="studentWork.versions && studentWork.versions.length">
      <h3>Work Versions</h3>
      <ul>
        <li v-for="(version, index) in studentWork.versions" :key="index">
          <strong>Version {{ index + 1 }}:</strong> {{ formatDate(version.uploadedAt) }}
        </li>
      </ul>
    </div> 

    <p v-if="studentWork.reverted !== null">
      <strong>Reverted Version:</strong> {{ studentWork.reverted }}
    </p>
  </div>
</template>
