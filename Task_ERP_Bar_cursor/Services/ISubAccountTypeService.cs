using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface ISubAccountTypeService
    {
        Task<List<SubAccountsType>> GetAllSubAccountTypesAsync();
        Task<SubAccountsType?> GetSubAccountTypeByIdAsync(int id);
        Task<SubAccountsType> CreateSubAccountTypeAsync(SubAccountsType type);
        Task<SubAccountsType> UpdateSubAccountTypeAsync(int id, SubAccountsType type);
        Task<bool> DeleteSubAccountTypeAsync(int id);
    }
} 