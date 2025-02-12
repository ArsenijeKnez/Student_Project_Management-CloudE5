using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.Model;
using MongoDB.Driver;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserManagementService.UserDB
{

    public class UserService
    {
        private readonly IMongoCollection<User> _users;

        public UserService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _users = database.GetCollection<User>(collectionName);
        }

        public async Task<User> GetUserByIdAsync(string id)
        {
            return await _users.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<User> GetUserByUsernameOrEmailAsync(string usernameOrEmail)
        {
            return await _users.Find(u => u.Username == usernameOrEmail || u.Email == usernameOrEmail).FirstOrDefaultAsync();
        }

        public async Task AddUserAsync(User user)
        {
            await _users.InsertOneAsync(user);
        }

        public async Task<bool> UpdateUserAsync(string id, User updatedUser)
        {
            var result = await _users.ReplaceOneAsync(u => u.Id == id, updatedUser);
            return result.ModifiedCount > 0;
        }

        public async Task<List<User>> GetAllUsersAsync()
        {
            return await _users.Find(_ => true).ToListAsync();
        }

        public async Task<List<User>> GetAllStudentsAsync()
        {
            return await _users.Find(u => u.Role == "Student").ToListAsync();
        }

        public async Task<bool> ChangeUserRoleAsync(string id, string newRole)
        {
            var update = Builders<User>.Update.Set(u => u.Role, newRole);
            var result = await _users.UpdateOneAsync(u => u.Id == id, update);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteUserAsync(string id)
        {
            var result = await _users.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }
    }

}
