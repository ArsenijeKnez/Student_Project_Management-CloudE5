<script setup>
import { ref, onMounted } from 'vue'
import NewUserForm from '@/components/admin/NewUserForm.vue'
import EditUserForm from '@/components/admin/EditUserForm.vue'
import UserService from '@/services/userService.js'
import AuthService from '@/services/authService.js'

const users = ref([])
const showNewUserForm = ref(false)
const editingUser = ref(null)

const getUsers = async () => {
  try {
    const data = await UserService.getAllUsers()
    users.value = data
  } catch (error) {
    console.error('Failed to load users:', error)
  }
}

onMounted(() => {
  getUsers()
})

const toggleNewUserForm = () => {
  if (editingUser.value) {
    editingUser.value = null
  }
  showNewUserForm.value = !showNewUserForm.value
}

const handleUserCreated = async (newUser) => {
  try {
    const response = await AuthService.register(newUser)
    if (response.data.success) {
      alert('User created successfully!')
      showNewUserForm.value = false
      await getUsers()
    } else {
      alert('Error: ' + response.data.message)
    }
  } catch (error) {
    console.error('Error creating user:', error)
  }
}

const startEditUser = (user) => {
  editingUser.value = { ...user }
  showNewUserForm.value = false
}

const handleUserUpdated = async (updatedData) => {
  console.log(editingUser.value.id)
  try {
    await UserService.updateUser(editingUser.value.id, updatedData)
    alert('User updated successfully!')
    editingUser.value = null
    await getUsers()
  } catch (error) {
    console.error('Error updating user:', error)
  }
}

const cancelEdit = () => {
  editingUser.value = null
}

const deleteUser = async (id) => {
  try {
    await UserService.deleteUser(id)
    alert('User deleted successfully!')
    await getUsers()
  } catch (error) {
    console.error('Error deleting user:', error)
  }
}

const promptChangeRole = async (id) => {
  const newRole = prompt('Enter new role (Admin, Student, Professor):')
  if (newRole && ['Admin', 'Student', 'Professor'].includes(newRole)) {
    try {
      await UserService.changeUserRole(id, newRole)
      alert('User role updated successfully!')
      await getUsers()
    } catch (error) {
      console.error('Error updating role:', error)
    }
  } else {
    alert('Invalid role.')
  }
}
</script>

<template>
  <div>
    <h1>User Management</h1>
    <button @click="toggleNewUserForm">
      {{ showNewUserForm ? 'Cancel' : 'Add User' }}
    </button>

    <NewUserForm v-if="showNewUserForm && !editingUser" @userCreated="handleUserCreated" />

    <EditUserForm
      v-if="editingUser"
      :user="editingUser"
      @userUpdated="handleUserUpdated"
      @cancel="cancelEdit"
    />

    <table v-if="users.length">
      <thead>
        <tr>
          <th>Username</th>
          <th>Email</th>
          <th>Role</th>
          <th>Actions</th>
        </tr>
      </thead>
      <tbody>
        <tr v-for="user in users" :key="user.id">
          <td>{{ user.username }}</td>
          <td>{{ user.email }}</td>
          <td>{{ user.role }}</td>
          <td>
            <button @click="startEditUser(user)">Edit</button>
            <button @click="deleteUser(user.id)">Delete</button>
            <button @click="promptChangeRole(user.id)">Change Role</button>
          </td>
        </tr>
      </tbody>
    </table>
    <p v-else>No users found.</p>
  </div>
</template>
