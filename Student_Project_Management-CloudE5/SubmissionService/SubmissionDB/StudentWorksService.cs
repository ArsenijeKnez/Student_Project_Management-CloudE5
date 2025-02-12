using Common.Enum;
using Common.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.SubmissionDB
{
    public class StudentWorksService
    {
        private readonly IMongoCollection<StudentWork> _submissions;

        public StudentWorksService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _submissions = database.GetCollection<StudentWork>(collectionName);
        }

        public async Task<StudentWork> GetWorkByIdAsync(string id)
        {
            return await _submissions.Find(u => u.Id == id).FirstOrDefaultAsync();
        }

        public async Task<List<StudentWork>> GetWorksByStudentIdAsync(string studentId)
        {
            return await _submissions.Find(w => w.StudentId == studentId).ToListAsync();
        }

        public async Task<List<StudentWork>> GetWorksByStatusAsync(WorkStatus status)
        {
            return await _submissions.Find(u => u.Status == status).ToListAsync();
        }


        public async Task AddWorkAsync(StudentWork work)
        {
            await _submissions.InsertOneAsync(work);
        }

        public async Task<bool> UpdateWorkAsync(string id, StudentWork updatedWork)
        {
            var result = await _submissions.ReplaceOneAsync(u => u.Id == id, updatedWork);
            return result.ModifiedCount > 0;
        }

        public async Task<bool> DeleteWorkAsync(string id)
        {
            var result = await _submissions.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
