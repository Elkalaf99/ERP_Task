using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class SubAccountTypeService : ISubAccountTypeService
    {
        private readonly HttpClient _httpClient;

        public SubAccountTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubAccountsType>> GetAllSubAccountTypesAsync()
        {
            try
            {
                var types = await _httpClient.GetFromJsonAsync<List<SubAccountsType>>("api/subaccountstype");
                return types ?? new List<SubAccountsType>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-account types: {ex.Message}");
                return new List<SubAccountsType>();
            }
        }

        public async Task<SubAccountsType?> GetSubAccountTypeByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SubAccountsType>($"api/subaccountstype/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-account type {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<SubAccountsType> CreateSubAccountTypeAsync(SubAccountsType type)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/subaccountstype", type);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccountsType>() ?? type;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sub-account type: {ex.Message}");
                throw;
            }
        }

        public async Task<SubAccountsType> UpdateSubAccountTypeAsync(int id, SubAccountsType type)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/subaccountstype/{id}", type);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccountsType>() ?? type;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sub-account type {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteSubAccountTypeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/subaccountstype/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sub-account type {id}: {ex.Message}");
                return false;
            }
        }
    }
} 