<script setup>
import { ref, defineEmits } from 'vue';
import InputField from '@/components/InputField.vue';

const emit = defineEmits(['userCreated']);
const errorMessage = ref('');

const username = ref('');
const email = ref('');
const password = ref('');
const firstName = ref('');
const lastName = ref('');
const role = ref('');

const handleNewUser = () => {
  if (!username.value || !email.value || !password.value || !firstName.value || !lastName.value || !role.value) {
    errorMessage.value = 'Please fill in all fields.';
    return;
  }
  if (!email.value.includes('@')) {
    errorMessage.value = 'Enter a valid email address.';
    return;
  }
  if (!['Admin', 'Student', 'Professor'].includes(role.value)) {
    errorMessage.value = 'Role must be Admin, Student, or Professor.';
    return;
  }
  
  const newUser = {
    username: username.value,
    email: email.value,
    password: password.value,
    firstName: firstName.value,
    lastName: lastName.value,
    role: role.value
  };
  
  emit('userCreated', newUser);
};
</script>

<template>
  <form @submit.prevent="handleNewUser">
    <InputField v-model="username" label="Username" type="text" required />
    <InputField v-model="email" label="Email" type="email" required />
    <InputField v-model="password" label="Password" type="password" required />
    <InputField v-model="firstName" label="First Name" type="text" required />
    <InputField v-model="lastName" label="Last Name" type="text" required />
    
    <label for="role">Role</label>
    <select v-model="role" id="role" required>
      <option value="" disabled>Select a role</option>
      <option value="Admin">Admin</option>
      <option value="Student">Student</option>
      <option value="Professor">Professor</option>
    </select>

    <p v-if="errorMessage" class="error-message">{{ errorMessage }}</p>
    <button type="submit">Create</button>
  </form>
</template>

