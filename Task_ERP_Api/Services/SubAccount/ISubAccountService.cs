using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.SubAccount
{
    public interface ISubAccountService
    {
        Task<IEnumerable<SubAccountDto>> GetAllAsync();
        Task<SubAccountDto?> GetByIdAsync(int id);
        Task<SubAccountDto> CreateAsync(CreateSubAccountDto createSubAccountDto);
        Task<SubAccountDto?> UpdateAsync(int id, UpdateSubAccountDto updateSubAccountDto);
        Task<bool> DeleteAsync(int id);
    }
} 