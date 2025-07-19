using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.JVDetail
{
    public interface IJVDetailService
    {
        Task<IEnumerable<JVDetailDto>> GetAllAsync();
        Task<JVDetailDto?> GetByIdAsync(int id);
        Task<JVDetailDto> CreateAsync(CreateJVDetailDto createJVDetailDto);
        Task<JVDetailDto?> UpdateAsync(int id, UpdateJVDetailDto updateJVDetailDto);
        Task<bool> DeleteAsync(int id);
    }
} 