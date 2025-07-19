using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class AccountService : IAccountService
    {
        private readonly HttpClient _httpClient;

        public AccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Account>> GetAllAccountsAsync()
        {
            try
            {
                var accounts = await _httpClient.GetFromJsonAsync<List<Account>>("api/account");
                return accounts ?? new List<Account>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching accounts: {ex.Message}");
                return new List<Account>();
            }
        }

        public async Task<Account?> GetAccountByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Account>($"api/account/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching account {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Account> CreateAccountAsync(Account account)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/account", account);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Account>() ?? account;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating account: {ex.Message}");
                throw;
            }
        }

        public async Task<Account> UpdateAccountAsync(int id, Account account)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/account/{id}", account);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Account>() ?? account;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating account {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAccountAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/account/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting account {id}: {ex.Message}");
                return false;
            }
        }
    }
} 