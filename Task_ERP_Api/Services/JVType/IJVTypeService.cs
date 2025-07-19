using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.JVType
{
    public interface IJVTypeService
    {
        Task<IEnumerable<JVTypeDto>> GetAllAsync();
        Task<JVTypeDto?> GetByIdAsync(int id);
        Task<JVTypeDto> CreateAsync(CreateJVTypeDto createJVTypeDto);
        Task<JVTypeDto?> UpdateAsync(int id, UpdateJVTypeDto updateJVTypeDto);
        Task<bool> DeleteAsync(int id);
    }
} 