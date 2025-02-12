<script setup>
import { ref, onMounted } from 'vue';
import UserService from '@/services/userService.js';

const users = ref([]);

const getStudets = async () => {
  try {
    const data = await UserService.getAllStudents();
    users.value = data;
  } catch (error) {
    console.error('Failed to load students:', error);
  }
};

onMounted(() => {
    getStudets();
});

</script>


<template>
    <div>
      <h1>Users</h1>
      <table v-if="users && users.length">
        <thead>
          <tr>
            <th>Name</th>
            <th>Last Name</th>
            <th>Email</th>
            <th>Actions</th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="user in users" :key="user.id">
            <td>{{ user.firstName }}</td>
            <td>{{ user.lastName }}</td>
            <td>{{ user.email }}</td>
            <td>
              <button @click="viewWork(user.id)">View Work</button>
              <button @click="generateReport(user.id)">Generate Report</button>
            </td>
          </tr>
        </tbody>
      </table>
      <p v-else>No students found.</p>
    </div>
  </template>