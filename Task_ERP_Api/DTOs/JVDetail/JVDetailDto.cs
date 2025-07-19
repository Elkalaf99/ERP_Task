using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Api.DTOs
{
    public class JVDetailDto
    {
        public int JVDetailID { get; set; }
        public int JVID { get; set; }
        public int AccountID { get; set; }
        public int? SubAccountID { get; set; }
        public decimal? Debit { get; set; }
        public decimal? Credit { get; set; }
        public bool IsDocumented { get; set; }
        [StringLength(1000)]
        public string? Notes { get; set; }
        public int? BranchID { get; set; }
    }
} 