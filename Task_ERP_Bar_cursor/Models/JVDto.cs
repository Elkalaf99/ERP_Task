using System.ComponentModel.DataAnnotations;

namespace Task_ERP_Bar.Models;

/// <summary>
/// نموذج نقل البيانات لإنشاء JV - يطابق الباك إند بالضبط
/// </summary>
public class JVDto
{
    /// <summary>
    /// معرف القيد (0 للإنشاء)
    /// </summary>
    public int jvID { get; set; } = 0;

    /// <summary>
    /// رقم القيد
    /// </summary>
    public string jvNo { get; set; } = string.Empty;

    /// <summary>
    /// تاريخ القيد
    /// </summary>
    public string jvDate { get; set; } = string.Empty;

    /// <summary>
    /// نوع القيد
    /// </summary>
    public int? jvTypeID { get; set; }

    /// <summary>
    /// إجمالي المدين
    /// </summary>
    public decimal totalDebit { get; set; }

    /// <summary>
    /// إجمالي الدائن
    /// </summary>
    public decimal totalCredit { get; set; }

    /// <summary>
    /// رقم الإيصال
    /// </summary>
    public string? receiptNo { get; set; }

    /// <summary>
    /// الملاحظات
    /// </summary>
    public string? notes { get; set; }

    /// <summary>
    /// معرف الفرع
    /// </summary>
    public int? branchID { get; set; }

    /// <summary>
    /// تفاصيل القيد (اختياري)
    /// </summary>
    public List<object> jvDetails { get; set; } = new List<object>();
}

/// <summary>
/// نموذج استجابة JV - يطابق الباك إند بالضبط
/// </summary>
public class JVResponse
{
    public int jvID { get; set; }
    public string jvNo { get; set; } = string.Empty;
    public DateTime jvDate { get; set; }
    public int? jvTypeID { get; set; }
    public decimal totalDebit { get; set; }
    public decimal totalCredit { get; set; }
    public string? receiptNo { get; set; }
    public string? notes { get; set; }
    public int? branchID { get; set; }
    public List<JVDetailResponse> jvDetails { get; set; } = new List<JVDetailResponse>();
} 