using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

public class Account
{
    public int AccountID { get; set; }
    public string AccountNumber { get; set; } = string.Empty;
    public string AccountNameAr { get; set; } = string.Empty;
    public string AccountNameEn { get; set; } = string.Empty;
    public int BranchID { get; set; }
}