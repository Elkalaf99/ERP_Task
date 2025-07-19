using System.Net.Http.Json;
using System.Text.Json;
using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface IApiService
    {
        Task<List<T>> GetAllAsync<T>(string endpoint);
        Task<T?> GetByIdAsync<T>(string endpoint, int id);
        Task<T?> CreateAsync<T>(string endpoint, T data);
        Task<T?> UpdateAsync<T>(string endpoint, int id, T data);
        Task<bool> DeleteAsync(string endpoint, int id);
        Task<ApiResponse<T>> GetApiResponseAsync<T>(string endpoint);
        Task<ApiResponse<T>> PostApiResponseAsync<T>(string endpoint, T data);
    }

    public class ApiService : IApiService
    {
        private readonly HttpClient _httpClient;
        private readonly JsonSerializerOptions _jsonOptions;

        public ApiService(HttpClient httpClient)
        {
            _httpClient = httpClient;
            
            // لا نعيد تعيين BaseAddress لأنه تم تعيينه بالفعل في Program.cs
            // _httpClient.BaseAddress = new Uri(_baseUrl);
            
            // إضافة Accept header إذا لم يكن موجوداً
            if (!_httpClient.DefaultRequestHeaders.Contains("Accept"))
            {
                _httpClient.DefaultRequestHeaders.Add("Accept", "application/json");
            }
            
            _jsonOptions = new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            };
        }

        public async Task<List<T>> GetAllAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                var result = JsonSerializer.Deserialize<List<T>>(content, _jsonOptions);
                return result ?? new List<T>();
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error in GetAllAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> GetByIdAsync<T>(string endpoint, int id)
        {
            try
            {
                var response = await _httpClient.GetAsync($"{endpoint}/{id}");
                
                if (response.StatusCode == System.Net.HttpStatusCode.NotFound)
                    return default;
                    
                response.EnsureSuccessStatusCode();
                
                var content = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(content, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error in GetByIdAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetByIdAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> CreateAsync<T>(string endpoint, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(endpoint, content);
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error in CreateAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in CreateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<T?> UpdateAsync<T>(string endpoint, int id, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PutAsync($"{endpoint}/{id}", content);
                response.EnsureSuccessStatusCode();
                
                var responseContent = await response.Content.ReadAsStringAsync();
                return JsonSerializer.Deserialize<T>(responseContent, _jsonOptions);
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error in UpdateAsync: {ex.Message}");
                throw;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateAsync: {ex.Message}");
                throw;
            }
        }

        public async Task<bool> DeleteAsync(string endpoint, int id)
        {
            try
            {
                var response = await _httpClient.DeleteAsync($"{endpoint}/{id}");
                return response.IsSuccessStatusCode;
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"HTTP Error in DeleteAsync: {ex.Message}");
                return false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in DeleteAsync: {ex.Message}");
                return false;
            }
        }

        public async Task<ApiResponse<T>> GetApiResponseAsync<T>(string endpoint)
        {
            try
            {
                var response = await _httpClient.GetAsync(endpoint);
                var content = await response.Content.ReadAsStringAsync();
                
                return new ApiResponse<T>
                {
                    IsSuccess = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode,
                    Data = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<T>(content, _jsonOptions) : default,
                    Message = response.IsSuccessStatusCode ? "Success" : content
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }

        public async Task<ApiResponse<T>> PostApiResponseAsync<T>(string endpoint, T data)
        {
            try
            {
                var json = JsonSerializer.Serialize(data, _jsonOptions);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");
                
                var response = await _httpClient.PostAsync(endpoint, content);
                var responseContent = await response.Content.ReadAsStringAsync();
                
                return new ApiResponse<T>
                {
                    IsSuccess = response.IsSuccessStatusCode,
                    StatusCode = (int)response.StatusCode,
                    Data = response.IsSuccessStatusCode ? JsonSerializer.Deserialize<T>(responseContent, _jsonOptions) : default,
                    Message = response.IsSuccessStatusCode ? "Success" : responseContent
                };
            }
            catch (Exception ex)
            {
                return new ApiResponse<T>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = ex.Message
                };
            }
        }
    }

    public class ApiResponse<T>
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }
        public string Message { get; set; } = string.Empty;
        public T? Data { get; set; }
    }
} 