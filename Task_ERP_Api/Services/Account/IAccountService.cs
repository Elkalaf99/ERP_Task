using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;

namespace Task_ERP_Api.Services.Account
{
    public interface IAccountService
    {
        Task<IEnumerable<AccountDto>> GetAllAsync();
        Task<AccountDto?> GetByIdAsync(int id);
        Task<AccountDto> CreateAsync(CreateAccountDto createAccountDto);
        Task<AccountDto?> UpdateAsync(int id, UpdateAccountDto updateAccountDto);
        Task<bool> DeleteAsync(int id);
    }
} 