using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{

    public class JVTypeDto
    {
        public int JVTypeID { get; set; }
        
        [Required]
        [StringLength(100)]
        public string JVTypeNameAr { get; set; } = string.Empty;
        
        [StringLength(100)]
        public string? JVTypeNameEn { get; set; }
        
        public int? BranchID { get; set; }
    }
} 