import axios from "axios";

const API_URL = "http://localhost:8366/api/Analysis";

export default {
  async updateFeedback(feedback, studentWorkId) {
    try {
      const response = await axios.put(
        `${API_URL}/append`,
        feedback, 
        {
          params: { studentWorkId },
          headers: {
            "Content-Type": "application/json",
          },
        }
      );

      return response.data;
    } catch (error) {
      console.error("Error updating feedback:", error);
      throw error;
    }
  },
};
