<script setup>
import { ref, onMounted } from 'vue'
import { useRouter } from 'vue-router'
import UserService from '@/services/userService.js'
import ClassProgress from '@/components/professor/ClassProgress.vue'

const users = ref([])
const router = useRouter()

const getStudets = async () => {
  try {
    const data = await UserService.getAllStudents()
    users.value = data
  } catch (error) {
    console.error('Failed to load students:', error)
  }
}

onMounted(() => {
  getStudets()
})

const viewWork = (id, username) => {
  router.push({ name: 'work-review', query: { id: id, username: username } })
}

const generateReport = (id, username) => {
  router.push({ name: 'individual-reports', query: { id: id, username: username } })
}
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
            <button @click="viewWork(user.id, user.username)">View Work</button>
            <button @click="generateReport(user.id, user.username)">Generate Report</button>
          </td>
        </tr>
      </tbody>
    </table>
    <p v-else>No students found.</p>
    <ClassProgress />
  </div>
</template>
