using Common.Model;
using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SubmissionService.SubmissionDB
{
    public class CourseService
    {
        private readonly IMongoCollection<Course> _courses;

        public CourseService(string connectionString, string databaseName, string collectionName)
        {
            var client = new MongoClient(connectionString);
            var database = client.GetDatabase(databaseName);
            _courses = database.GetCollection<Course>(collectionName);
        }

        public async Task<List<Course>> GetAllCoursesAsync()
        {
            return await _courses.Find(Builders<Course>.Filter.Empty).ToListAsync();
        }

        public async Task AddCourseAsync(Course course)
        {
            await _courses.InsertOneAsync(course);
        }

        public async Task<bool> DeleteCourseAsync(string id)
        {
            var result = await _courses.DeleteOneAsync(u => u.Id == id);
            return result.DeletedCount > 0;
        }
    }
}
