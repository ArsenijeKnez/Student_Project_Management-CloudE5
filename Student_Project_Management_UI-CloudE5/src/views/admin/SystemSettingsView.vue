<template>
  <div class="input-group">
    <h1>System Settings</h1>
    <label>
      Max submissions per period:
      <input type="number" v-model="maxSubmissions" />
    </label>
  </div>
</template>

<script setup>
import { ref } from 'vue';
const maxSubmissions = ref(10);
</script>