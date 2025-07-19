using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface ISubAccountsDetailService
    {
        Task<List<SubAccounts_Detail>> GetAllSubAccountsDetailsAsync();
        Task<SubAccounts_Detail?> GetSubAccountsDetailByIdAsync(int id);
        Task<SubAccounts_Detail> CreateSubAccountsDetailAsync(SubAccounts_Detail subAccountsDetail);
        Task<SubAccounts_Detail> UpdateSubAccountsDetailAsync(int id, SubAccounts_Detail subAccountsDetail);
        Task<bool> DeleteSubAccountsDetailAsync(int id);
    }
} 