<script setup>
import { ref } from "vue";
import submissionService from "@/services/submissionService";
import FileUpload from "@/components/student/FileUpload.vue";

const props = defineProps({
  studentWorkId: {
    type: String,
    required: true,
  },
});

const file = ref(null);
const message = ref("");

const handleFileSelected = (selectedFile) => {
  file.value = selectedFile;
};

const updateWork = async () => {
    const studentWorkId = props.studentWorkId;
  if (!file.value || !studentWorkId) {
    message.value = "All fields are required.";
    return;
  }

   const response = await submissionService.updateWork(file.value, studentWorkId);
   message.value = response.data.success ? "Update successful!" : response.data.message;
};
</script>

<template>
  <div>
    <h2>Update Work</h2>
    <FileUpload label="Select New File" @fileSelected="handleFileSelected" />
    <button @click="updateWork">Update</button>
    <p>{{ message }}</p>
  </div>
</template>
