import axios from 'axios';

const API_URL = '/api/users';
const ADMIN_API_URL = '/api/admin/users';

export default {
    async getUser(id) {
        try {
            const response = await axios.get(`${API_URL}/${id}`);
            return response.data;
        } catch (error) {
            throw error.response ? error.response.data : error;
        }
    },

    async updateUser(id, updatedUser) {
        try {
            await axios.put(`${API_URL}/${id}`, updatedUser);
        } catch (error) {
            throw error.response ? error.response.data : error;
        }
    },

    async getAllUsers() {
        try {
            const response = await axios.get(`${ADMIN_API_URL}`);
            return response.data;
        } catch (error) {
            throw error.response ? error.response.data : error;
        }
    },

    async changeUserRole(id, newRole) {
        try {
            await axios.put(`${ADMIN_API_URL}/${id}/role`, newRole, {
                headers: { 'Content-Type': 'application/json' }
            });
        } catch (error) {
            throw error.response ? error.response.data : error;
        }
    },

    async deleteUser(id) {
        try {
            await axios.delete(`${ADMIN_API_URL}/${id}`);
        } catch (error) {
            throw error.response ? error.response.data : error;
        }
    }
};
