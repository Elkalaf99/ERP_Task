using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.City
{
    public interface ICityService
    {
        Task<IEnumerable<CityDto>> GetAllAsync();
        Task<CityDto?> GetByIdAsync(int id);
        Task<CityDto> CreateAsync(CreateCityDto createCityDto);
        Task<CityDto?> UpdateAsync(int id, UpdateCityDto updateCityDto);
        Task<bool> DeleteAsync(int id);
    }
} 