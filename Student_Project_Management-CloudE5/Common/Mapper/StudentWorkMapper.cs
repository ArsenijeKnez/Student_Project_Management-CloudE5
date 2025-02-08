﻿using Common.Dto;
using Common.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Mapper
{
    public static class StudentWorkMapper
    {
        public static StudentWorkDto ToDto(StudentWork entity)
        {
            if (entity == null) return null;

            return new StudentWorkDto
            {
                Id = entity.Id,
                StudentId = entity.StudentId,
                Title = entity.Title,
                Versions = entity.Versions,
                Status = entity.Status,
                SubmissionDate = entity.SubmissionDate,
                EstimatedAnalysisCompletion = entity.EstimatedAnalysisCompletion,
                Feedback = entity.Feedback
            };
        }

        public static StudentWork ToEntity(StudentWorkDto dto)
        {
            if (dto == null) return null;

            return new StudentWork
            {
                Id = dto.Id,
                StudentId = dto.StudentId,
                Title = dto.Title,
                Versions = dto.Versions,
                Status = dto.Status,
                SubmissionDate = dto.SubmissionDate,
                EstimatedAnalysisCompletion = dto.EstimatedAnalysisCompletion,
                Feedback = dto.Feedback
            };
        }

    }
}
