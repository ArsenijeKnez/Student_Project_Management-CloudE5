<script setup>
import { ref } from 'vue'
import submissionService from '@/services/submissionService'
import FileUpload from '@/components/student/FileUpload.vue'

const title = ref('')
const file = ref(null)
const message = ref('')

const handleFileSelected = (selectedFile) => {
  file.value = selectedFile
}

const uploadWork = async () => {
  const user = JSON.parse(sessionStorage.getItem('user'))
  const studentId = user?.id

  if (!file.value || !studentId || !title.value) {
    message.value = 'All fields are required.'
    return
  }

  const response = await submissionService.uploadWork({
    studentId: studentId,
    title: title.value,
    file: file.value,
  })
  console.log(response)
  message.value = response.data.success ? 'Upload successful!' : response.data.message
}
</script>

<template>
  <div>
    <h2>Upload Work</h2>
    <input v-model="title" type="text" placeholder="Title" />
    <FileUpload label="Select File" @fileSelected="handleFileSelected" />
    <button @click="uploadWork">Submit</button>
    <p>{{ message }}</p>
  </div>
</template>
