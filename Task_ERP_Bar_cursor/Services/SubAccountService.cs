using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class SubAccountService : ISubAccountService
    {
        private readonly HttpClient _httpClient;

        public SubAccountService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubAccount>> GetAllSubAccountsAsync()
        {
            try
            {
                var subAccounts = await _httpClient.GetFromJsonAsync<List<SubAccount>>("api/subaccount");
                return subAccounts ?? new List<SubAccount>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-accounts: {ex.Message}");
                return new List<SubAccount>();
            }
        }

        public async Task<SubAccount?> GetSubAccountByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SubAccount>($"api/subaccount/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-account {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<SubAccount> CreateSubAccountAsync(SubAccount subAccount)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/subaccount", subAccount);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccount>() ?? subAccount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sub-account: {ex.Message}");
                throw;
            }
        }

        public async Task<SubAccount> UpdateSubAccountAsync(int id, SubAccount subAccount)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/subaccount/{id}", subAccount);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccount>() ?? subAccount;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sub-account {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteSubAccountAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/subaccount/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sub-account {id}: {ex.Message}");
                return false;
            }
        }
    }
} 