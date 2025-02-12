import axios from "axios";

const API_URL = "http://localhost:8366/api/AdminSettings"; 
export default {
    async SetDailySubmissionLimit(limit) {
      try {
        const response = await axios.post(`${API_URL}/setDailySubmissionLimit?limit=${limit}`);
        return response.data;
      } catch (error) {
        return error.response?.data || { success: false, message: "Failed to set daily submission limit" };
      }
    },
    async SetAnalysisInterval(interval) {
      try {
        const response = await axios.put(`${API_URL}/setAnalysisInterval?interval=${interval}`);
        return response.data;
      } catch (error) {
        return error.response?.data || { success: false, message: "Failed to set analysis interval" };
      }
    },
  };
