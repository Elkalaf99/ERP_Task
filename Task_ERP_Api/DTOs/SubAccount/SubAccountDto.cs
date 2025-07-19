using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class SubAccountDto
    {
        public int SubAccountID { get; set; }
        
        [Required]
        [StringLength(50)]
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