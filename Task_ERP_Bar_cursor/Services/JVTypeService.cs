using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class JVTypeService : IJVTypeService
    {
        private readonly HttpClient _httpClient;

        public JVTypeService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<JVType>> GetAllJVTypesAsync()
        {
            try
            {
                var jvTypes = await _httpClient.GetFromJsonAsync<List<JVType>>("api/jvtype");
                return jvTypes ?? new List<JVType>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV types: {ex.Message}");
                return new List<JVType>();
            }
        }

        public async Task<JVType?> GetJVTypeByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<JVType>($"api/jvtype/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching JV type {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<JVType> CreateJVTypeAsync(JVType jvType)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("api/jvtype", jvType);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<JVType>() ?? jvType;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating JV type: {ex.Message}");
                throw;
            }
        }

        public async Task<JVType> UpdateJVTypeAsync(int id, JVType jvType)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"api/jvtype/{id}", jvType);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<JVType>() ?? jvType;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating JV type {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteJVTypeAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"api/jvtype/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting JV type {id}: {ex.Message}");
                return false;
            }
        }
    }
} 