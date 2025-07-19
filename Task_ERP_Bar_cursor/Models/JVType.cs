using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class JVType
{
    public int JVTypeID { get; set; }

    [Required]
    [StringLength(50)]
    public string JVTypeNameAr { get; set; } = string.Empty;

    [StringLength(50)]
    public string? JVTypeNameEn { get; set; }

    public int? BranchID { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Branch? Branch { get; set; }
}