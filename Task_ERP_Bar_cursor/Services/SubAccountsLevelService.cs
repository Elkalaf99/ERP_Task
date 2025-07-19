using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class SubAccountsLevelService : ISubAccountsLevelService
    {
        private readonly HttpClient _httpClient;

        public SubAccountsLevelService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubAccounts_Level>> GetAllSubAccountsLevelsAsync()
        {
            try
            {
                var subAccountsLevels = await _httpClient.GetFromJsonAsync<List<SubAccounts_Level>>("api/subaccounts_level");
                return subAccountsLevels ?? new List<SubAccounts_Level>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-accounts levels: {ex.Message}");
                return new List<SubAccounts_Level>();
            }
        }

        public async Task<SubAccounts_Level?> GetSubAccountsLevelByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SubAccounts_Level>($"api/subaccounts_level/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-accounts level {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<SubAccounts_Level> CreateSubAccountsLevelAsync(SubAccounts_Level subAccountsLevel)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/subaccounts_level", subAccountsLevel);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccounts_Level>() ?? subAccountsLevel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sub-accounts level: {ex.Message}");
                throw;
            }
        }

        public async Task<SubAccounts_Level> UpdateSubAccountsLevelAsync(int id, SubAccounts_Level subAccountsLevel)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/subaccounts_level/{id}", subAccountsLevel);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccounts_Level>() ?? subAccountsLevel;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sub-accounts level {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteSubAccountsLevelAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/subaccounts_level/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sub-accounts level {id}: {ex.Message}");
                return false;
            }
        }
    }
} 