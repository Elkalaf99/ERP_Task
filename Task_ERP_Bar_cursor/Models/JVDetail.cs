using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public partial class JVDetail
{
    public int JVDetailID { get; set; }

    public int JVID { get; set; }

    public int AccountID { get; set; }

    public int? SubAccountID { get; set; }

    // للحفاظ على التوافق مع الواجهة الحالية
    public decimal? Debit { get; set; }
    public decimal? Credit { get; set; }

    // للحفاظ على التوافق مع الباك إند
    public decimal? DebitAmount 
    { 
        get => Debit; 
        set => Debit = value; 
    }
    
    public decimal? CreditAmount 
    { 
        get => Credit; 
        set => Credit = value; 
    }

    public bool IsDocumented { get; set; }

    [StringLength(1000)]
    public string? Notes { get; set; }

    // للحفاظ على التوافق مع الباك إند
    public string? Description 
    { 
        get => Notes; 
        set => Notes = value; 
    }

    public int? BranchID { get; set; }
    
    // Navigation properties
    public virtual JV? JV { get; set; }
    public virtual Account? Account { get; set; }
    public virtual SubAccount? SubAccount { get; set; }
    public virtual Branch? Branch { get; set; }
}