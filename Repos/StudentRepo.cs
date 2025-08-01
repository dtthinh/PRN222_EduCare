﻿using BOs.Models;
using DAOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Repos
{
    public class StudentRepo : IStudentRepo
    {
        public async Task<Student?> GetStudentByCodeAsync(string studentCode)
        {
            return await StudentDAO.Instance.GetStudentByCodeAsync(studentCode);
        }

        public async Task<Student> CreateStudentAsync(Student student)
        {
            return await StudentDAO.Instance.CreateStudentAsync(student);
        }

        public async Task<Student?> GetStudentByIdAsync(int id)
        {
            return await StudentDAO.Instance.GetStudentByIdAsync(id);
        }

        public async Task<List<Student>> GetAllStudentsAsync()
        {
            return await StudentDAO.Instance.GetAllStudentsAsync();
        }

        public async Task<bool> UpdateStudentAsync(Student student)
        {
            return await StudentDAO.Instance.UpdateStudentAsync(student);
        }

        public async Task<bool> DeleteStudentAsync(int id)
        {
            return await StudentDAO.Instance.DeleteStudentAsync(id);
        }
        public Task RemoveParentFromStudentAsync(int studentId) => StudentDAO.Instance.RemoveParentFromStudentAsync(studentId);
        public Task<List<Student>> GetStudentsByParentIdAsync(int parentId) => StudentDAO.Instance.GetStudentsByParentIdAsync(parentId);

        public Task<List<Student>> GetStudentsByClassIdAsync(int classId) => StudentDAO.Instance.GetStudentsByClassIdAsync(classId);

        public Task<List<Student>> GetStudentsByClassIdsAsync(List<int> classIds) => StudentDAO.Instance.GetStudentsByClassIdsAsync(classIds);

        public Task<List<Student>> GetStudentsWithoutParentsByClassIdAsync(int classId) => StudentDAO.Instance.GetStudentsWithoutParentsByClassIdAsync(classId);

        public Task UpdateParentForStudentAsync(int studentId, int parentId) => StudentDAO.Instance.UpdateParentForStudentAsync(studentId, parentId);
    }

}
