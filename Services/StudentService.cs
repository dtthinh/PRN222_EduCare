﻿using BOs.Models;
using Repos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class StudentService : IStudentService
    {
        private readonly IStudentRepo _studentRepo;

        public StudentService(IStudentRepo studentRepo)
        {
            _studentRepo = studentRepo;
        }

        public async Task<Student?> GetStudentByCodeAsync(string studentCode)
        {
            return await _studentRepo.GetStudentByCodeAsync(studentCode);
        }

        public async Task<bool> LinkStudentToParentAsync(string studentCode, int parentId)
        {
            var linked = await _studentRepo.AssignParentToStudentAsync(studentCode, parentId);
            if (linked)
            {
                // Get the student to retrieve their ID
                var student = await _studentRepo.GetStudentByCodeAsync(studentCode);
                if (student != null)
                {
                    // Update all future health checks to include the new parent
                    var healthCheckService = new HealthCheckService();
                    await healthCheckService.UpdateParentForFutureHealthChecksAsync(student.StudentId, parentId);
                }
            }
            return linked;
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            return await _studentRepo.CreateStudentAsync(student);
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await _studentRepo.GetStudentByIdAsync(id);
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await _studentRepo.GetAllStudentsAsync();
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            return await _studentRepo.UpdateStudentAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await _studentRepo.DeleteStudentAsync(id);
        }

        public async Task<List<Student>> GetStudentsByParentIdAsync(int parentId)
        {
            return await _studentRepo.GetStudentsByParentIdAsync(parentId);
        }

        public async Task<List<Student>> GetStudentsByClassIdAsync(int classId)
        {
            return await _studentRepo.GetStudentsByClassIdAsync(classId);
        }

        public async Task<List<Student>> GetStudentsByClassIdsAsync(List<int> classIds)
        {
            return await _studentRepo.GetStudentsByClassIdsAsync(classIds);
        }
    }
}
