<script>
import api from '@/services/userService'

export default {
  data() {
    return {
      users: [],
      selectedUser: '',
      restrictionKey: '',
      restrictions: [],
    }
  },
  async created() {
    await this.fetchUsers()
  },
  methods: {
    async fetchUsers() {
      try {
        this.users = await api.getAllUsers()
      } catch (error) {
        console.error('Error fetching users:', error)
      }
    },
    async fetchUserRestrictions() {
      if (!this.selectedUser) return
      try {
        this.restrictions = await api.getUserRestrictions(this.selectedUser)
      } catch (error) {
        console.error('Error fetching restrictions:', error)
      }
    },
    async addRestriction() {
      if (!this.selectedUser || !this.restrictionKey) return
      try {
        await api.addUserRestriction(this.restrictionKey, this.selectedUser)
        this.restrictionKey = ''
        await this.fetchUserRestrictions()
      } catch (error) {
        console.error('Error adding restriction:', error)
      }
    },
    async removeRestriction() {
      if (!this.selectedUser || !this.restrictionKey) return
      try {
        await api.removeUserRestriction(this.restrictionKey, this.selectedUser)
        this.restrictionKey = ''
        await this.fetchUserRestrictions()
      } catch (error) {
        console.error('Error removing restriction:', error)
      }
    },
  },
  watch: {
    selectedUser() {
      this.fetchUserRestrictions()
    },
  },
}
</script>

<template>
  <div>
    <h2>User Restrictions</h2>

    <label for="user">Select User:</label>
    <select v-model="selectedUser">
      <option v-for="user in users" :key="user.id" :value="user.id">
        {{ user.username }}
      </option>
    </select>

    <label for="restriction">Restriction Key:</label>
    <input type="text" v-model="restrictionKey" placeholder="Enter restriction key" />

    <button @click="addRestriction">Add Restriction</button>
    <button @click="removeRestriction">Remove Restriction</button>

    <h3>Current Restrictions</h3>
    <ul>
      <li v-for="restriction in restrictions" :key="restriction">
        {{ restriction }}
      </li>
    </ul>
  </div>
</template>
