using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class BranchDto
    {
        public int BranchID { get; set; }
        
        [Required]
        [StringLength(50)]
        public string BranchCode { get; set; } = string.Empty;
        
        [Required]
        [StringLength(50)]
        public string BranchNameAr { get; set; } = string.Empty;
        
        [StringLength(50)]
        public string? BranchNameEn { get; set; }
    }
} 