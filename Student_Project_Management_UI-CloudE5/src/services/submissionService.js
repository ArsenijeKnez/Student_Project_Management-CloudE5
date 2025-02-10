import axios from "axios";

const API_URL = "http://localhost:8366/api/Submission";

export default {
  async uploadWork(workData) {
    try {
      const formData = new FormData();
      formData.append("file", workData.file);
      formData.append("studentId", workData.studentId);
      formData.append("title", workData.title);
      console.log(workData);
      const response = await axios.post(`${API_URL}/work`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      return response;
    } catch (error) {
      return error.response?.data || { success: false, message: "File upload failed" };
    }
  },

  async updateWork(file, studentWorkId) {
    try {
      const formData = new FormData();
      formData.append("file", file);

      const response = await axios.post(`${API_URL}/updateWork?studentWorkId=${studentWorkId}`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      return response;
    } catch (error) {
      return error.response?.data || { success: false, message: "Update failed" };
    }
  },

  async getWorkStatus(studentId) {
    try {
      const response = await axios.get(`${API_URL}/work/${studentId}/status`);
      return response;
    } catch (error) {
      return error.response?.data || { success: false, message: "Failed to retrieve work status" };
    }
  },

  async getFeedback(studentWorkId) {
    try {
      const response = await axios.get(`${API_URL}/work/${studentWorkId}/feedback`);
      return response;
    } catch (error) {
      return error.response?.data || { success: false, message: "Failed to retrieve feedback" };
    }
  },
};
