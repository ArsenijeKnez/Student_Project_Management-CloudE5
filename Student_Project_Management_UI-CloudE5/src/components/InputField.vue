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
