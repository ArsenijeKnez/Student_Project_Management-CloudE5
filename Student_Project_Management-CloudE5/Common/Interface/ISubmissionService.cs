﻿using Common.Dto;
using Common.Model;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface 
{
    public interface ISubmissionService : IService
    {
        Task<ResultMessage> UploadWork(string studentId, string fileUrl, string title);
        Task<ResultMessage> UpdateWork(string fileUrl, string studentWorkId);
        Task<List<StudentWorkStatus>> GetWorkStatus(string studentId);
        Task<List<StudentWorkDto>> GetWorksOfStudent(string studentId);
        Task<List<StudentWorkDto>> GetAllStudentWorks();
        Task<FeedbackDto> GetFeedback(string studentWorkId);
        Task<StudentWorkDto> GetStudentWork(string studentWorkId);
        Task<ResultMessage> RevertVersion(string studentWorkId, int version);
        Task<ResultMessage> SetDailySubmissionLimit(int limit);
        Task<ResultMessage> SetProcessingInterval(TimeSpan interval);
        Task<ResultMessage> UpdateFeedback(FeedbackDto feedbackDto, string studentWorkId);
        Task<List<Course>> GetCourses();
        Task<ResultMessage> AddCourse(string name);
        Task<ResultMessage> DeleteCourse(string courseId);
    } 
}
