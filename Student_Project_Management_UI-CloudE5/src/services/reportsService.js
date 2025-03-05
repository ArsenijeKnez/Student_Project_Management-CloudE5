import axios from 'axios'

const API_URL = 'http://localhost:8366/api/Reports'

export default {
  async getStudentProgress(studentId) {
    try {
      const response = await axios.get(`${API_URL}/progress/${studentId}`)
      return response.data
    } catch (error) {
      console.error('Error fetching student progress:', error)
      throw error
    }
  },

  async getClassProgress() {
    try {
      const response = await axios.get(`${API_URL}/progress/class`)
      return response.data
    } catch (error) {
      console.error('Error fetching class progress:', error)
      throw error
    }
  },
}
