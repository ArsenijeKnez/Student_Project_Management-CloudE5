using Common.Dto;
using Microsoft.ServiceFabric.Services.Remoting;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Interface
{
    public interface IProgressService: IService
    {
        Task<StudentProgress> GenerateStudentProgress(string studentId, List<StudentWorkDto> studentWorks);
        Task<ClassProgress> GenerateClassProgress(List<StudentWorkDto> studentWorks);
    }
}
