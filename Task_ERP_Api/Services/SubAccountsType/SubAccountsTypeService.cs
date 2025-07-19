using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.SubAccountsType
{
    public class SubAccountsTypeService : ISubAccountsTypeService
    {
        private readonly IGenericRepository<Models.SubAccountsType> _repository;
        private readonly IMapper _mapper;

        public SubAccountsTypeService(IGenericRepository<Models.SubAccountsType> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubAccountsTypeDto>> GetAllAsync()
        {
            var subAccountsTypes = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubAccountsTypeDto>>(subAccountsTypes);
        }

        public async Task<SubAccountsTypeDto?> GetByIdAsync(int id)
        {
            var subAccountsType = await _repository.GetByIdAsync(id);
            return _mapper.Map<SubAccountsTypeDto>(subAccountsType);
        }

        public async Task<SubAccountsTypeDto> CreateAsync(CreateSubAccountsTypeDto createSubAccountsTypeDto)
        {
            var subAccountsType = _mapper.Map<Models.SubAccountsType>(createSubAccountsTypeDto);
            var createdSubAccountsType = await _repository.AddAsync(subAccountsType);
            return _mapper.Map<SubAccountsTypeDto>(createdSubAccountsType);
        }

        public async Task<SubAccountsTypeDto?> UpdateAsync(int id, UpdateSubAccountsTypeDto updateSubAccountsTypeDto)
        {
            var existingSubAccountsType = await _repository.GetByIdAsync(id);
            if (existingSubAccountsType == null)
                return null;

            _mapper.Map(updateSubAccountsTypeDto, existingSubAccountsType);
            var updatedSubAccountsType = await _repository.UpdateAsync(existingSubAccountsType);
            return _mapper.Map<SubAccountsTypeDto>(updatedSubAccountsType);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
} 