using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models
{
    public class CreateAccountRequest
    {
        [Required(ErrorMessage = "رقم الحساب مطلوب")]
        [StringLength(50, ErrorMessage = "رقم الحساب يجب أن يكون أقل من 50 حرف")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "اسم الحساب بالعربية مطلوب")]
        [StringLength(200, ErrorMessage = "اسم الحساب يجب أن يكون أقل من 200 حرف")]
        public string AccountNameAr { get; set; } = string.Empty;

        [StringLength(200, ErrorMessage = "اسم الحساب يجب أن يكون أقل من 200 حرف")]
        public string? AccountNameEn { get; set; }

        [Required(ErrorMessage = "الفرع مطلوب")]
        [Range(1, int.MaxValue, ErrorMessage = "يجب اختيار فرع صحيح")]
        public int BranchID { get; set; }

        public bool IsActive { get; set; } = true;
    }
} 