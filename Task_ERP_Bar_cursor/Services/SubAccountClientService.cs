using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class SubAccountClientService : ISubAccountClientService
    {
        private readonly HttpClient _httpClient;

        public SubAccountClientService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<SubAccountsClient>> GetAllSubAccountClientsAsync()
        {
            try
            {
                var clients = await _httpClient.GetFromJsonAsync<List<SubAccountsClient>>("api/subaccountsclient");
                return clients ?? new List<SubAccountsClient>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-account clients: {ex.Message}");
                return new List<SubAccountsClient>();
            }
        }

        public async Task<SubAccountsClient?> GetSubAccountClientByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<SubAccountsClient>($"api/subaccountsclient/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching sub-account client {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<SubAccountsClient> CreateSubAccountClientAsync(SubAccountsClient client)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/subaccountsclient", client);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccountsClient>() ?? client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating sub-account client: {ex.Message}");
                throw;
            }
        }

        public async Task<SubAccountsClient> UpdateSubAccountClientAsync(int id, SubAccountsClient client)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/subaccountsclient/{id}", client);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<SubAccountsClient>() ?? client;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating sub-account client {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteSubAccountClientAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/subaccountsclient/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting sub-account client {id}: {ex.Message}");
                return false;
            }
        }
    }
} 