<template>
  <div>
    <h1>User Management</h1>
    <button @click="addUser">Add User</button>
    <table>
  <thead>
    <tr>
      <th>Name</th>
      <th>Role</th>
      <th>Actions</th>
    </tr>
  </thead>
  <tbody>
    <tr v-for="user in users" :key="user.id">
      <td>{{ user.name }}</td>
      <td>{{ user.role }}</td>
      <td>
        <button @click="editUser(user.id)">Edit</button>
        <button @click="deleteUser(user.id)">Delete</button>
      </td>
    </tr>
  </tbody>
</table>

  </div>
</template>

<script setup>
import { ref } from 'vue';

const users = ref([
  
]);

const addUser = () => alert('Add User');
const editUser = (id) => alert('Edit User ' + id);
const deleteUser = (id) => alert('Delete User ' + id);
</script>
