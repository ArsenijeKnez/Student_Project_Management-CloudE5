import axios from "axios";

const API_URL = "http://localhost:8366/api/Auth"; 
export default {
  async register(userData) {
    try {
      const formData = new FormData();
      formData.append("username", userData.username);
      formData.append("email", userData.email);
      formData.append("passwordHash", userData.password);
      formData.append("role", userData.role);

      const response = await axios.post(`${API_URL}/register`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      return response.data;
    } catch (error) {
      return error.response?.data || { success: false, message: "Registration failed" };
    }
  },

  async login(userData) {
    try {
      const formData = new FormData();
      formData.append("usernameOrEmail", userData.usernameOrEmail);
      formData.append("password", userData.password);

      const response = await axios.post(`${API_URL}/login`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });

      return response.data;
    } catch (error) {
      return error.response?.data || { success: false, message: "Login failed" };
    }
  },
};
