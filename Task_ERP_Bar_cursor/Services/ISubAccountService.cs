using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface ISubAccountService
    {
        Task<List<SubAccount>> GetAllSubAccountsAsync();
        Task<SubAccount?> GetSubAccountByIdAsync(int id);
        Task<SubAccount> CreateSubAccountAsync(SubAccount subAccount);
        Task<SubAccount> UpdateSubAccountAsync(int id, SubAccount subAccount);
        Task<bool> DeleteSubAccountAsync(int id);
    }
} 