using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{

    public class CreateSubAccountDto
    {
        [Required]
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "Sub account number can only contain letters, numbers, hyphens, and underscores.")]
        public string SubAccountNumber { get; set; } = string.Empty;
        
        [Required]
        [StringLength(100)]
        public string SubAccountNameAr { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? SubAccountNameEn { get; set; }
        
        public int? ParentID { get; set; }
        
        public bool IsMain { get; set; }
        
        public int LevelID { get; set; }
        
        public int? SubAccountTypeID { get; set; }
        
        public int? BranchID { get; set; }
    }
} 