using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class SubAccounts_Detail
{
    public int SubAccountDetailID { get; set; }

    public int SubAccountID { get; set; }

    public int AccountID { get; set; }

    public int? BranchID { get; set; }
    
    public DateTime StartDate { get; set; }
    
    public DateTime? EndDate { get; set; }
    
    public decimal Budget { get; set; }
    
    [StringLength(500)]
    public string? Description { get; set; }
    
    public bool IsActive { get; set; } = true;
    
    // Navigation properties
    public virtual SubAccount? SubAccount { get; set; }
    public virtual Account? Account { get; set; }
    public virtual Branch? Branch { get; set; }
}