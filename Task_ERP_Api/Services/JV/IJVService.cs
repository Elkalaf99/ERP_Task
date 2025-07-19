using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.JV
{
    public interface IJVService
    {
        Task<IEnumerable<JVDto>> GetAllAsync();
        Task<JVDto?> GetByIdAsync(int id);
        Task<JVDto> CreateAsync(CreateJVDto createJVDto);
        Task<JVDto?> UpdateAsync(int id, UpdateJVDto updateJVDto);
        Task<bool> DeleteAsync(int id);
    }
} 