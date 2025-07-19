using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Services.Branch
{
    public interface IBranchService
    {
        Task<IEnumerable<BranchDto>> GetAllAsync();
        Task<BranchDto?> GetByIdAsync(int id);
        Task<BranchDto> CreateAsync(CreateBranchDto createBranchDto);
        Task<BranchDto?> UpdateAsync(int id, UpdateBranchDto updateBranchDto);
        Task<bool> DeleteAsync(int id);
    }
} 