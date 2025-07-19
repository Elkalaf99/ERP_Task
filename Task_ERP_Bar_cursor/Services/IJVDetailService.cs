using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface IJVDetailService
    {
        Task<List<JVDetail>> GetAllJVDetailsAsync();
        Task<JVDetail?> GetJVDetailByIdAsync(int id);
        Task<JVDetail> CreateJVDetailAsync(JVDetail jvDetail);
        Task<JVDetail> UpdateJVDetailAsync(int id, JVDetail jvDetail);
        Task<bool> DeleteJVDetailAsync(int id);
        Task<List<JVDetail>> GetJVDetailsByJVIdAsync(int jvId);
        Task<bool> CreateJVDetailAsync(int jvid, int accountID, int? subAccountID, decimal? debit, decimal? credit, bool isDocumented, string notes, int branchID);
    }
} 