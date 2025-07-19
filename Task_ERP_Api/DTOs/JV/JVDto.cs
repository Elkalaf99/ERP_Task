using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class JVDto
    {
        public int JVID { get; set; }
        [Required]
        [StringLength(50)]
        public string JVNo { get; set; } = string.Empty;
        public DateTime JVDate { get; set; }
        public int? JVTypeID { get; set; }
        public decimal TotalDebit { get; set; }
        public decimal TotalCredit { get; set; }
        [StringLength(50)]
        public string? ReceiptNo { get; set; }
        [StringLength(1000)]
        public string? Notes { get; set; }
        public int? BranchID { get; set; }
    }
} 