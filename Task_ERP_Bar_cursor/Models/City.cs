using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class City
{
    public int CityID { get; set; }

    [Required]
    [StringLength(100)]
    public string CityNameAr { get; set; } = string.Empty;

    [StringLength(100)]
    public string? CityNameEn { get; set; }

    public int? BranchID { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual Branch? Branch { get; set; }
}