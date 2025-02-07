import axios from "axios";

const API_URL = "http://localhost:8366/api/Auth"; 
export default {
  async register(userData) {
    try {
      const formData = new FormData();
      formData.append("Username", userData.username);
      formData.append("Email", userData.email);
      formData.append("Password", userData.password);
      formData.append("FirstName", userData.firstName);
      formData.append("LastName", userData.lastName);
      formData.append("Role", userData.role);

      const response = await axios.post(`${API_URL}/register`, formData, {
        headers: { "Content-Type": "multipart/form-data" },
      });
      console.log(response);
      return response;
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

      return response;
    } catch (error) {
      return error.response?.data || { success: false, message: "Login failed" };
    }
  },
};
