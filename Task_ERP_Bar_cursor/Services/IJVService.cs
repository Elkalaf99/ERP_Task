using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface IJVService
    {
        Task<List<JV>> GetAllJVsAsync();
        Task<JV?> GetJVByIdAsync(int jvId);
        Task<JV> CreateJVAsync(JV jv);
        Task<JV> UpdateJVAsync(JV jv);
        Task<bool> DeleteJVAsync(int jvId);
        Task<string> GenerateJVNumberAsync();
        Task<List<Account>> GetAccountsAsync();
        Task<List<SubAccount>> GetSubAccountsAsync();
        Task<List<JVType>> GetJVTypesAsync();
        Task<List<Branch>> GetBranchesAsync();
        Task<JV> CreateJVWithoutDetailsAsync(JV jv);
    }
} 