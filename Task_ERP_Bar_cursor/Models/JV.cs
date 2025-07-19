using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class JV
{
    public int JVID { get; set; }

    [Required]
    [StringLength(50)]
    public string JVNumber { get; set; } = string.Empty;

    public DateTime JVDate { get; set; }

    public int? JVTypeID { get; set; }

    public decimal TotalDebit { get; set; }

    public decimal TotalCredit { get; set; }

    [StringLength(50)]
    public string? ReceiptNo { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }

    public int? BranchID { get; set; }
    
    public bool IsActive { get; set; } = true;

    public List<JVDetail> JVDetails { get; set; } = new List<JVDetail>();
    
    // Navigation properties
    public virtual JVType? JVType { get; set; }
    public virtual Branch? Branch { get; set; }
}