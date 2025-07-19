using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class SubAccountsDetailService : ISubAccountsDetailService
    {
        private readonly HttpClient _httpClient;

        public SubAccountsDetailService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubAccounts_Detail>> GetAllSubAccountsDetailsAsync()
        {
            try
            {
                var subAccountsDetails = await _httpClient.GetFromJsonAsync<List<SubAccounts_Detail>>("api/subaccounts_detail");
                return subAccountsDetails ?? new List<SubAccounts_Detail>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-accounts details: {ex.Message}");
                return new List<SubAccounts_Detail>();
            }
        }

        public async Task<SubAccounts_Detail?> GetSubAccountsDetailByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SubAccounts_Detail>($"api/subaccounts_detail/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-accounts detail {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<SubAccounts_Detail> CreateSubAccountsDetailAsync(SubAccounts_Detail subAccountsDetail)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/subaccounts_detail", subAccountsDetail);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccounts_Detail>() ?? subAccountsDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sub-accounts detail: {ex.Message}");
                throw;
            }
        }

        public async Task<SubAccounts_Detail> UpdateSubAccountsDetailAsync(int id, SubAccounts_Detail subAccountsDetail)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/subaccounts_detail/{id}", subAccountsDetail);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccounts_Detail>() ?? subAccountsDetail;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sub-accounts detail {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteSubAccountsDetailAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/subaccounts_detail/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sub-accounts detail {id}: {ex.Message}");
                return false;
            }
        }
    }
} 