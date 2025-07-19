using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class BranchService : IBranchService
    {
        private readonly HttpClient _httpClient;

        public BranchService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<Branch>> GetAllBranchesAsync()
        {
            try
            {
                var branches = await _httpClient.GetFromJsonAsync<List<Branch>>("Branch");
                return branches ?? new List<Branch>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching branches: {ex.Message}");
                return new List<Branch>();
            }
        }

        public async Task<Branch?> GetBranchByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<Branch>($"Branch/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching branch {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<Branch> CreateBranchAsync(Branch branch)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("Branch", branch);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Branch>() ?? branch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating branch: {ex.Message}");
                throw;
            }
        }

        public async Task<Branch> UpdateBranchAsync(int id, Branch branch)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"Branch/{id}", branch);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<Branch>() ?? branch;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating branch {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteBranchAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"Branch/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting branch {id}: {ex.Message}");
                return false;
            }
        }
    }
} 