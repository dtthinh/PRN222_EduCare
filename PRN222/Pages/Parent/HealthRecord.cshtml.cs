using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using BOs.Models;
using Repos;
using DAOs;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using System.Linq;

namespace PRN222.Pages.Parent
{
    public class HealthRecordModel : PageModel
    {
        public List<Student> AssignedStudents { get; set; } = new List<Student>();

        public async Task OnGetAsync()
        {
            int? parentId = HttpContext.Session.GetInt32("UserId");
            if (parentId.HasValue)
            {
                AssignedStudents = await StudentDAO.Instance.GetStudentsByParentIdAsync(parentId.Value);
            }
        }

        public async Task<JsonResult> OnGetCheckStudentAsync(string code)
        {
            var student = await StudentDAO.Instance.GetStudentByCodeAsync(code);
            if (student == null)
            {
                return new JsonResult(new { success = false, error = "Student not found." });
            }
            if (student.ParentId != null)
            {
                return new JsonResult(new { success = false, error = "Student is already assigned to a parent." });
            }
            return new JsonResult(new { success = true, student = new {
                StudentCode = student.StudentCode,
                Fullname = student.Fullname,
                Gender = student.Gender,
                DateOfBirth = student.DateOfBirth,
                ClassName = student.Class?.ClassName
            } });
        }

        public int? GetHealthRecordIdForStudent(int studentId)
        {
            int? parentId = HttpContext.Session.GetInt32("UserId");
            if (!parentId.HasValue) return null;
            var record = HealthRecordDAO.Instance.GetHealthRecordsByStudentIdAsync(studentId).Result
                .FirstOrDefault(hr => hr.ParentId == parentId.Value);
            return record?.HealthRecordId;
        }

        public bool HasHealthRecord(int studentId)
        {
            return GetHealthRecordIdForStudent(studentId) != null;
        }

        public class AssignStudentRequest
        {
            public string code { get; set; }
        }
    }
} 