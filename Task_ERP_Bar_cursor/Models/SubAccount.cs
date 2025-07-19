using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class SubAccount
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

    public int? BranchID { get; set; }

    public int? LevelID { get; set; }
    
    public int? ParentID { get; set; }
    
    public int? SubAccountTypeID { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Branch? Branch { get; set; }
    public virtual SubAccounts_Level? Level { get; set; }
    public virtual SubAccount? Parent { get; set; }
    public virtual SubAccountsType? SubAccountType { get; set; }
}