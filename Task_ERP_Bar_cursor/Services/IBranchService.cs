using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface IBranchService
    {
        Task<List<Branch>> GetAllBranchesAsync();
        Task<Branch?> GetBranchByIdAsync(int id);
        Task<Branch> CreateBranchAsync(Branch branch);
        Task<Branch> UpdateBranchAsync(int id, Branch branch);
        Task<bool> DeleteBranchAsync(int id);
    }
} 