﻿using BOs.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public interface IStudentService
    {
        Task<Student?> GetStudentByIdAsync(int id);
        Task<List<Student>> GetAllStudentsAsync();
        Task<Student> CreateStudentAsync(Student student);
        Task<bool> UpdateStudentAsync(Student student);
        Task<bool> DeleteStudentAsync(int id);
        Task<Student?> GetStudentByCodeAsync(string studentCode);
        Task<List<Student>> GetStudentsByParentIdAsync(int parentId);
        Task<List<Student>> GetStudentsByClassIdAsync(int classId);
        Task<List<Student>> GetStudentsByClassIdsAsync(List<int> classIds);
        Task<List<Student>> GetStudentsWithoutParentsByClassIdAsync(int classId);
        Task UpdateParentForStudentAsync(int studentId, int parentId);
        Task RemoveParentFromStudentAsync(int studentId);
    }
}
