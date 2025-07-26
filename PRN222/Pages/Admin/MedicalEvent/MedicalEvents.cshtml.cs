using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Services;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

public class MedicalEventsModel : PageModel
{
    private readonly IMedicalEventService _service;

    public MedicalEventsModel(IMedicalEventService service)
    {
        _service = service;
    }

    public Dictionary<Class, List<MedicalEvent>> EventsByClass { get; set; }

    public async Task OnGetAsync()
    {
        var allEvents = await _service.GetAllMedicalEventsAsync() ?? new List<MedicalEvent>();

        EventsByClass = allEvents
            .Where(e => e.Student?.Class != null)
            .GroupBy(e => e.Student.Class, new ClassComparer()) // Sử dụng ClassComparer để GroupBy object
            .ToDictionary(g => g.Key, g => g.ToList());
    }
}

// Lớp Helper để so sánh 2 đối tượng Class, cần cho GroupBy
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