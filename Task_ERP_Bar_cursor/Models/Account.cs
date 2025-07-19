using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class Account
{
    public int AccountID { get; set; }

    [Required]
    [StringLength(50)]
    public string AccountNumber { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string AccountNameAr { get; set; } = string.Empty;

    [StringLength(100)]
    public string? AccountNameEn { get; set; }

    public int? BranchID { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Branch? Branch { get; set; }
}