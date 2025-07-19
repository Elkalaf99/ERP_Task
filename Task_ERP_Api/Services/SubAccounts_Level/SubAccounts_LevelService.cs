using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.SubAccounts_Level
{
    public class SubAccounts_LevelService : ISubAccounts_LevelService
    {
        private readonly IGenericRepository<Models.SubAccounts_Level> _repository;
        private readonly IMapper _mapper;

        public SubAccounts_LevelService(IGenericRepository<Models.SubAccounts_Level> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubAccounts_LevelDto>> GetAllAsync()
        {
            var subAccountsLevels = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubAccounts_LevelDto>>(subAccountsLevels);
        }

        public async Task<SubAccounts_LevelDto?> GetByIdAsync(int id)
        {
            var subAccountsLevel = await _repository.GetByIdAsync(id);
            return _mapper.Map<SubAccounts_LevelDto>(subAccountsLevel);
        }

        public async Task<SubAccounts_LevelDto> CreateAsync(CreateSubAccounts_LevelDto createSubAccounts_LevelDto)
        {
            var subAccountsLevel = _mapper.Map<Models.SubAccounts_Level>(createSubAccounts_LevelDto);
            var createdSubAccountsLevel = await _repository.AddAsync(subAccountsLevel);
            return _mapper.Map<SubAccounts_LevelDto>(createdSubAccountsLevel);
        }

        public async Task<SubAccounts_LevelDto?> UpdateAsync(int id, UpdateSubAccounts_LevelDto updateSubAccounts_LevelDto)
        {
            var existingSubAccountsLevel = await _repository.GetByIdAsync(id);
            if (existingSubAccountsLevel == null)
                return null;

            _mapper.Map(updateSubAccounts_LevelDto, existingSubAccountsLevel);
            var updatedSubAccountsLevel = await _repository.UpdateAsync(existingSubAccountsLevel);
            return _mapper.Map<SubAccounts_LevelDto>(updatedSubAccountsLevel);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
} 