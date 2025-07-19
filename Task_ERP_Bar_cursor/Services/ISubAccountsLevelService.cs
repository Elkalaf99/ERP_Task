using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface ISubAccountsLevelService
    {
        Task<List<SubAccounts_Level>> GetAllSubAccountsLevelsAsync();
        Task<SubAccounts_Level?> GetSubAccountsLevelByIdAsync(int id);
        Task<SubAccounts_Level> CreateSubAccountsLevelAsync(SubAccounts_Level subAccountsLevel);
        Task<SubAccounts_Level> UpdateSubAccountsLevelAsync(int id, SubAccounts_Level subAccountsLevel);
        Task<bool> DeleteSubAccountsLevelAsync(int id);
    }
} 