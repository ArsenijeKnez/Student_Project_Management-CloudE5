<script setup>
import { ref } from "vue";

defineProps({
  label: String,
});

const file = ref(null);
const emit = defineEmits(["fileSelected"]); 

const selectFile = (event) => {
  file.value = event.target.files[0];
  if (file.value) {
    emit("fileSelected", file.value); 
  }
};
</script>

<template>
  <div class="file-upload">
    <label>{{ label }}</label>
    <input type="file" @change="selectFile" />
  </div>
</template>

<style scoped>
.file-upload {
  display: flex;
  flex-direction: column;
  gap: 5px;
}
</style>
