using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class CreateBranchDto
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "Branch code can only contain letters, numbers, hyphens, and underscores.")]
        public string BranchCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string BranchNameAr { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? BranchNameEn { get; set; }
    }
} 