using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class SubAccountsClient
{
    public int SubAccountClientID { get; set; }

    [Required]
    [StringLength(100)]
    public string ClientNameAr { get; set; } = string.Empty;

    [StringLength(100)]
    public string? ClientNameEn { get; set; }

    [StringLength(50)]
    public string? ClientCode { get; set; }

    [StringLength(200)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [StringLength(50)]
    public string? Email { get; set; }

    public int? SubAccountID { get; set; }

    public int? CityID { get; set; }

    public int? BranchID { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual SubAccount? SubAccount { get; set; }
    public virtual City? City { get; set; }
    public virtual Branch? Branch { get; set; }
}