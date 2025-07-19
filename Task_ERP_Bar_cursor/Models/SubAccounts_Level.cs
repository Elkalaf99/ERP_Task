using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class SubAccounts_Level
{
    public int LevelID { get; set; }

    [Required]
    [StringLength(100)]
    public string LevelNameAr { get; set; } = string.Empty;

    [StringLength(100)]
    public string? LevelNameEn { get; set; }

    public int? BranchID { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Branch? Branch { get; set; }
}