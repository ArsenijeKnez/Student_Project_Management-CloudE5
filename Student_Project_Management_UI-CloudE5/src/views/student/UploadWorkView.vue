<script setup>
import { ref } from "vue";
import submissionService from "@/services/submissionService";
import FileUpload from "@/components/student/FileUpload.vue";

const studentId = ref("");
const title = ref("");
const file = ref(null);
const message = ref("");

const handleFileSelected = (selectedFile) => {
  file.value = selectedFile;
};

const uploadWork = async () => {
  if (!file.value || !studentId.value || !title.value) {
    console.log(file.value);
    message.value = "All fields are required.";
    return;
  }

  const response = await submissionService.uploadWork({
    studentId: studentId.value,
    title: title.value,
    file: file.value,
  });

  message.value = response.success ? "Upload successful!" : response.message;
};
</script>

<template>
  <div>
    <h2>Upload Work</h2>
    <input v-model="studentId" type="text" placeholder="Student ID" />
    <input v-model="title" type="text" placeholder="Title" />
    <FileUpload label="Select File" @fileSelected="handleFileSelected" />
    <button @click="uploadWork">Submit</button>
    <p>{{ message }}</p>
  </div>
</template>
