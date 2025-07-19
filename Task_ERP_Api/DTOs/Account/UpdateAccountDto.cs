using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class UpdateAccountDto
    {
        [Required(ErrorMessage = "Account number is required.")]
        [StringLength(50, ErrorMessage = "Account number cannot exceed 50 characters.")]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "Account number can only contain letters, numbers, hyphens, and underscores.")]
        public string AccountNumber { get; set; } = string.Empty;

        [Required(ErrorMessage = "Arabic account name is required.")]
        [StringLength(100, ErrorMessage = "Arabic account name cannot exceed 100 characters.")]
        public string AccountNameAr { get; set; } = string.Empty;

        [StringLength(100, ErrorMessage = "English account name cannot exceed 100 characters.")]
        public string? AccountNameEn { get; set; }

        public int? BranchID { get; set; }
    }
} 