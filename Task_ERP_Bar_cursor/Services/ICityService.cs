using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface ICityService
    {
        Task<List<City>> GetAllCitiesAsync();
        Task<City?> GetCityByIdAsync(int id);
        Task<City> CreateCityAsync(City city);
        Task<City> UpdateCityAsync(int id, City city);
        Task<bool> DeleteCityAsync(int id);
    }
} 