using BOs.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PRN222.Pages.Parent
{
    public class RequestMedicineModel: PageModel
    {
        private readonly IParentMedicationRequestService _requestService;
        private readonly IStudentService _studentService;

        public RequestMedicineModel(IParentMedicationRequestService requestService, IStudentService studentService)
        {
            _requestService = requestService;
            _studentService = studentService;
        }

        [BindProperty]
        public ParentMedicationRequest MedicationRequest { get; set; }

        // KHẮC PHỤC LỖI: Khởi tạo ngay lập tức để không bao giờ bị null
        public SelectList StudentOptions { get; set; } = new SelectList(Enumerable.Empty<SelectListItem>());

        public async Task<IActionResult> OnGetAsync()
        {
            int? parentId = HttpContext.Session.GetInt32("UserId");

            if (!parentId.HasValue)
            {
                TempData["ErrorMessage"] = "Vui lòng đăng nhập để sử dụng chức năng này.";
                return RedirectToPage("/Credential/Login");
            }

            // Luôn khởi tạo MedicationRequest để form có thể hiển thị
            MedicationRequest = new ParentMedicationRequest
            {
                Medications = new List<ParentMedicationDetail> { new ParentMedicationDetail() }
            };

            var students = await _studentService.GetStudentsByParentIdAsync(parentId.Value);

            if (students != null && students.Any())
            {
                // Chỉ cập nhật StudentOptions nếu tìm thấy học sinh
                StudentOptions = new SelectList(students, "StudentId", "Fullname");
            }
            else
            {
                TempData["ErrorMessage"] = "Không tìm thấy thông tin học sinh nào được liên kết với tài khoản của bạn. Bạn chưa thể tạo yêu cầu.";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            int? parentId = HttpContext.Session.GetInt32("UserId");

            if (!parentId.HasValue)
            {
                TempData["ErrorMessage"] = "Phiên làm việc đã hết hạn. Vui lòng đăng nhập lại.";
                return RedirectToPage("/Credential/Login");
            }

            if (MedicationRequest.Medications != null)
            {
                MedicationRequest.Medications = MedicationRequest.Medications
                                             .Where(m => !string.IsNullOrWhiteSpace(m.Name))
                                             .ToList();
            }
            ModelState.Clear();
            if (!ModelState.IsValid || MedicationRequest.Medications == null || !MedicationRequest.Medications.Any())
            {
                if (MedicationRequest.Medications == null || !MedicationRequest.Medications.Any())
                {
                    ModelState.AddModelError(string.Empty, "Bạn phải thêm ít nhất một loại thuốc.");
                }

                // Tải lại danh sách học sinh nếu có lỗi
                var students = await _studentService.GetStudentsByParentIdAsync(parentId.Value);
                if (students != null)
                {
                    StudentOptions = new SelectList(students, "StudentId", "Fullname", MedicationRequest.StudentId);
                }
                return Page();
            }

            MedicationRequest.ParentId = parentId.Value;
            MedicationRequest.DateCreated = DateTime.Now;
            MedicationRequest.Status = "Pending";

            var isCreated = await _requestService.CreateAsync(MedicationRequest);

            if (isCreated)
            {
                TempData["SuccessMessage"] = "Yêu cầu gửi thuốc đã được tạo thành công và đang chờ duyệt.";
                return RedirectToPage("/Index");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Đã xảy ra lỗi khi tạo yêu cầu. Vui lòng thử lại.");
                var students = await _studentService.GetStudentsByParentIdAsync(parentId.Value);
                if (students != null)
                {
                    StudentOptions = new SelectList(students, "StudentId", "Fullname", MedicationRequest.StudentId);
                }
                return Page();
            }
        }
    }
}