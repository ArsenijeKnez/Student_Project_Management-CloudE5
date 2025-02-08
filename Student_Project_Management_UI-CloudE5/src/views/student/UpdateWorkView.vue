<script setup>
import { ref } from "vue";
import submissionService from "@/services/submissionService";
import FileUpload from "@/components/student/FileUpload.vue";

const studentWorkId = ref("");
const file = ref(null);
const message = ref("");

const handleFileSelected = (selectedFile) => {
  file.value = selectedFile;
};

const updateWork = async () => {
  if (!file.value || !studentWorkId.value) {
    message.value = "All fields are required.";
    return;
  }

  const response = await submissionService.updateWork(file.value, studentWorkId.value);
  message.value = response.success ? "Update successful!" : response.message;
};
</script>

<template>
  <div>
    <h2>Update Work</h2>
    <input v-model="studentWorkId" type="text" placeholder="Student Work ID" />
    <FileUpload label="Select New File" @fileSelected="handleFileSelected" />
    <button @click="updateWork">Update</button>
    <p>{{ message }}</p>
  </div>
</template>
