using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class Branch
{
    public int BranchID { get; set; }

    [Required]
    [StringLength(10)]
    public string BranchCode { get; set; } = string.Empty;

    [Required]
    [StringLength(100)]
    public string BranchNameAr { get; set; } = string.Empty;

    [StringLength(100)]
    public string? BranchNameEn { get; set; }
    
    public bool IsActive { get; set; } = true;
}