<script setup>
defineProps({
  modelValue: String,
  label: String,
  type: String,
});

defineEmits(['update:modelValue']);
</script>

<template>
  <div class="input-group">
    <label>{{ label }}</label>
    <input
      :type="type"
      :value="modelValue"
      @input="$emit('update:modelValue', $event.target.value)"
    />
  </div>
</template>

<style scoped>
.input-group {
  display: flex;
  flex-direction: column;
}

input {
  padding: 0.5rem;
  border: 1px solid #ccc;
  border-radius: 5px;
}
</style>
