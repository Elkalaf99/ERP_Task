using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{

    public class CreateJVDto
    {
        public DateTime JVDate { get; set; }
        
        public int? JVTypeID { get; set; }
        
        public decimal TotalDebit { get; set; }
        
        public decimal TotalCredit { get; set; }
        
        [StringLength(50)]
        [RegularExpression(@"^[A-Za-z0-9\-_]+$", ErrorMessage = "Receipt number can only contain letters, numbers, hyphens, and underscores.")]
        public string? ReceiptNo { get; set; }
        
        [StringLength(1000)]
        public string? Notes { get; set; }
        
        public int? BranchID { get; set; }
    }
} 