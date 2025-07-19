using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.SubAccounts_Detail
{
    public class SubAccounts_DetailService : ISubAccounts_DetailService
    {
        private readonly IGenericRepository<Models.SubAccounts_Detail> _repository;
        private readonly IMapper _mapper;

        public SubAccounts_DetailService(IGenericRepository<Models.SubAccounts_Detail> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubAccounts_DetailDto>> GetAllAsync()
        {
            var subAccountsDetails = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubAccounts_DetailDto>>(subAccountsDetails);
        }

        public async Task<SubAccounts_DetailDto?> GetByIdAsync(int id)
        {
            var subAccountsDetail = await _repository.GetByIdAsync(id);
            return _mapper.Map<SubAccounts_DetailDto>(subAccountsDetail);
        }

        public async Task<SubAccounts_DetailDto> CreateAsync(CreateSubAccounts_DetailDto createSubAccounts_DetailDto)
        {
            var subAccountsDetail = _mapper.Map<Models.SubAccounts_Detail>(createSubAccounts_DetailDto);
            var createdSubAccountsDetail = await _repository.AddAsync(subAccountsDetail);
            return _mapper.Map<SubAccounts_DetailDto>(createdSubAccountsDetail);
        }

        public async Task<SubAccounts_DetailDto?> UpdateAsync(int id, UpdateSubAccounts_DetailDto updateSubAccounts_DetailDto)
        {
            var existingSubAccountsDetail = await _repository.GetByIdAsync(id);
            if (existingSubAccountsDetail == null)
                return null;

            _mapper.Map(updateSubAccounts_DetailDto, existingSubAccountsDetail);
            var updatedSubAccountsDetail = await _repository.UpdateAsync(existingSubAccountsDetail);
            return _mapper.Map<SubAccounts_DetailDto>(updatedSubAccountsDetail);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
} 