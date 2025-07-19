using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.SubAccounts_Level
{
    public interface ISubAccounts_LevelService
    {
        Task<IEnumerable<SubAccounts_LevelDto>> GetAllAsync();
        Task<SubAccounts_LevelDto?> GetByIdAsync(int id);
        Task<SubAccounts_LevelDto> CreateAsync(CreateSubAccounts_LevelDto createSubAccounts_LevelDto);
        Task<SubAccounts_LevelDto?> UpdateAsync(int id, UpdateSubAccounts_LevelDto updateSubAccounts_LevelDto);
        Task<bool> DeleteAsync(int id);
    }
} 