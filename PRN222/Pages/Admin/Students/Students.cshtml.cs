using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using BOs.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Linq;

namespace PRN222.Pages.Admin.Students
{
    public class StudentsModel : PageModel
    {
        private readonly IStudentService _studentService;

        public StudentsModel(IStudentService studentService)
        {
            _studentService = studentService;
        }

        public Dictionary<Class, List<Student>> StudentsByClass { get; set; } = new();

        public async Task OnGetAsync()
        {
            var allStudents = await _studentService.GetAllStudentsAsync() ?? new List<Student>();

            StudentsByClass = allStudents
                .Where(s => s.Class != null)
                .GroupBy(s => s.Class, new ClassComparer())
                .ToDictionary(g => g.Key, g => g.ToList());
        }
    }

    public class ClassComparer : IEqualityComparer<Class>
    {
        public bool Equals(Class x, Class y)
        {
            if (ReferenceEquals(x, y)) return true;
            if (ReferenceEquals(x, null) || ReferenceEquals(y, null)) return false;
            return x.ClassId == y.ClassId;
        }

        public int GetHashCode(Class obj)
        {
            if (ReferenceEquals(obj, null)) return 0;
            return obj.ClassId.GetHashCode();
        }
    }
}
