<script setup>
import { ref, watch, defineProps, defineEmits } from 'vue';
import InputField from '@/components/InputField.vue';

const props = defineProps({
  user: {
    type: Object,
    required: true,
  },
});

const emit = defineEmits(['userUpdated', 'cancel']);

const id = ref(props.user.id)
const username = ref(props.user.username);
const email = ref(props.user.email);
const password = ref(props.user.password); 
const firstName = ref(props.user.firstName);
const lastName = ref(props.user.lastName);
const role = ref(props.user.role);
const errorMessage = ref('');

watch(
  () => props.user,
  (newUser) => {
    username.value = newUser.username;
    email.value = newUser.email;
    firstName.value = newUser.firstName;
    password.value = newUser.password
    lastName.value = newUser.lastName;
    role.value = newUser.role;
  }
);

const handleUpdateUser = () => {
  if (!username.value || !email.value || !firstName.value || !password.value || !lastName.value || !role.value) {
    errorMessage.value = 'Please fill in all required fields.';
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
  
  const updatedUser = {
    id: id.value,
    username: username.value,
    email: email.value,
    password: password.value,
    firstName: firstName.value,
    lastName: lastName.value,
    role: role.value,
  };
  
  emit('userUpdated', updatedUser);
};
</script>

<template>
  <form @submit.prevent="handleUpdateUser">
    <InputField v-model="username" label="Username" type="text" required />
    <InputField v-model="email" label="Email" type="email" required />
    <InputField v-model="password" label="Password" type="password" />
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
    <button type="submit">Update</button>
    <button type="button" @click="$emit('cancel')">Cancel</button>
  </form>
</template>


