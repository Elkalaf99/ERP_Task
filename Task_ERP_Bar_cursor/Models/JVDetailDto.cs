namespace Task_ERP_Bar.Models;

/// <summary>
/// نموذج إنشاء JVDetail - يطابق Curl بالضبط
/// </summary>
public class JVDetailDto
{
    public int jVID { get; set; }
    public int accountID { get; set; }
    public int? subAccountID { get; set; }
    public decimal? debit { get; set; }
    public decimal? credit { get; set; }
    public bool isDocumented { get; set; }
    public string? notes { get; set; }
    public int branchID { get; set; }
}

/// <summary>
/// نموذج استجابة JVDetail - يطابق الباك إند بالضبط
/// </summary>
public class JVDetailResponse
{
    public int jvDetailID { get; set; }
    public int jvid { get; set; }
    public int accountID { get; set; }
    public int? subAccountID { get; set; }
    public decimal? debit { get; set; }
    public decimal? credit { get; set; }
    public bool isDocumented { get; set; }
    public string? notes { get; set; }
    public int? branchID { get; set; }
} 