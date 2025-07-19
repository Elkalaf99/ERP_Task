using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.SubAccountsType
{
    public interface ISubAccountsTypeService
    {
        Task<IEnumerable<SubAccountsTypeDto>> GetAllAsync();
        Task<SubAccountsTypeDto?> GetByIdAsync(int id);
        Task<SubAccountsTypeDto> CreateAsync(CreateSubAccountsTypeDto createSubAccountsTypeDto);
        Task<SubAccountsTypeDto?> UpdateAsync(int id, UpdateSubAccountsTypeDto updateSubAccountsTypeDto);
        Task<bool> DeleteAsync(int id);
    }
} 