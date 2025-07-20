using System.Net.Http.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public class CityService : ICityService
    {
        private readonly HttpClient _httpClient;

        public CityService(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<List<City>> GetAllCitiesAsync()
        {
            try
            {
                var cities = await _httpClient.GetFromJsonAsync<List<City>>("city");
                return cities ?? new List<City>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching cities: {ex.Message}");
                return new List<City>();
            }
        }

        public async Task<City?> GetCityByIdAsync(int id)
        {
            try
            {
                return await _httpClient.GetFromJsonAsync<City>($"city/{id}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error fetching city {id}: {ex.Message}");
                return null;
            }
        }

        public async Task<City> CreateCityAsync(City city)
        {
            try
            {
                var response = await _httpClient.PostAsJsonAsync("city", city);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<City>() ?? city;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating city: {ex.Message}");
                throw;
            }
        }

        public async Task<City> UpdateCityAsync(int id, City city)
        {
            try
            {
                var response = await _httpClient.PutAsJsonAsync($"city/{id}", city);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadFromJsonAsync<City>() ?? city;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating city {id}: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteCityAsync(int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"city/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting city {id}: {ex.Message}");
                return false;
            }
        }
    }
} 