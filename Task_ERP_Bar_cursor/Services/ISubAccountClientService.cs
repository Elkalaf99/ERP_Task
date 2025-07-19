using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface ISubAccountClientService
    {
        Task<List<SubAccountsClient>> GetAllSubAccountClientsAsync();
        Task<SubAccountsClient?> GetSubAccountClientByIdAsync(int id);
        Task<SubAccountsClient> CreateSubAccountClientAsync(SubAccountsClient client);
        Task<SubAccountsClient> UpdateSubAccountClientAsync(int id, SubAccountsClient client);
        Task<bool> DeleteSubAccountClientAsync(int id);
    }
} 