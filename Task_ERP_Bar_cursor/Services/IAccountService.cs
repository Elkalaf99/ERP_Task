using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface IAccountService
    {
        Task<List<Account>> GetAllAccountsAsync();
        Task<Account?> GetAccountByIdAsync(int id);
        Task<Account> CreateAccountAsync(Account account);
        Task<Account> UpdateAccountAsync(int id, Account account);
        Task<bool> DeleteAccountAsync(int id);
    }
} 