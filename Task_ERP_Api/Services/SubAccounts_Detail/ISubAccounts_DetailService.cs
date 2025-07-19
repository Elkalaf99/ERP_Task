using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.SubAccounts_Detail
{
    public interface ISubAccounts_DetailService
    {
        Task<IEnumerable<SubAccounts_DetailDto>> GetAllAsync();
        Task<SubAccounts_DetailDto?> GetByIdAsync(int id);
        Task<SubAccounts_DetailDto> CreateAsync(CreateSubAccounts_DetailDto createSubAccounts_DetailDto);
        Task<SubAccounts_DetailDto?> UpdateAsync(int id, UpdateSubAccounts_DetailDto updateSubAccounts_DetailDto);
        Task<bool> DeleteAsync(int id);
    }
} 