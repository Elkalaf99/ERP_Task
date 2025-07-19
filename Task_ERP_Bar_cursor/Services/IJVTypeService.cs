using Task_ERP_Bar.Models;

namespace Task_ERP_Bar.Services
{
    public interface IJVTypeService
    {
        Task<List<JVType>> GetAllJVTypesAsync();
        Task<JVType?> GetJVTypeByIdAsync(int id);
        Task<JVType> CreateJVTypeAsync(JVType jvType);
        Task<JVType> UpdateJVTypeAsync(int id, JVType jvType);
        Task<bool> DeleteJVTypeAsync(int id);
    }
} 