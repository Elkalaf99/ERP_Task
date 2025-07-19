using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.SubAccountsClient
{
    public interface ISubAccountsClientService
    {
        Task<IEnumerable<SubAccountsClientDto>> GetAllAsync();
        Task<SubAccountsClientDto?> GetByIdAsync(int id);
        Task<SubAccountsClientDto> CreateAsync(CreateSubAccountsClientDto createSubAccountsClientDto);
        Task<SubAccountsClientDto?> UpdateAsync(int id, UpdateSubAccountsClientDto updateSubAccountsClientDto);
        Task<bool> DeleteAsync(int id);
    }
} 