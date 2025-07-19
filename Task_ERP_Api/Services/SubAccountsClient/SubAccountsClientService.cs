using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.SubAccountsClient
{
    public class SubAccountsClientService : ISubAccountsClientService
    {
        private readonly IGenericRepository<Models.SubAccountsClient> _repository;
        private readonly IMapper _mapper;

        public SubAccountsClientService(IGenericRepository<Models.SubAccountsClient> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubAccountsClientDto>> GetAllAsync()
        {
            var subAccountsClients = await _repository.GetAllAsync();
            return _mapper.Map<IEnumerable<SubAccountsClientDto>>(subAccountsClients);
        }

        public async Task<SubAccountsClientDto?> GetByIdAsync(int id)
        {
            var subAccountsClient = await _repository.GetByIdAsync(id);
            return _mapper.Map<SubAccountsClientDto>(subAccountsClient);
        }

        public async Task<SubAccountsClientDto> CreateAsync(CreateSubAccountsClientDto createSubAccountsClientDto)
        {
            var subAccountsClient = _mapper.Map<Models.SubAccountsClient>(createSubAccountsClientDto);
            var createdSubAccountsClient = await _repository.AddAsync(subAccountsClient);
            return _mapper.Map<SubAccountsClientDto>(createdSubAccountsClient);
        }

        public async Task<SubAccountsClientDto?> UpdateAsync(int id, UpdateSubAccountsClientDto updateSubAccountsClientDto)
        {
            var existingSubAccountsClient = await _repository.GetByIdAsync(id);
            if (existingSubAccountsClient == null)
                return null;

            _mapper.Map(updateSubAccountsClientDto, existingSubAccountsClient);
            var updatedSubAccountsClient = await _repository.UpdateAsync(existingSubAccountsClient);
            return _mapper.Map<SubAccountsClientDto>(updatedSubAccountsClient);
        }

        public async Task<bool> DeleteAsync(int id)
        {
            return await _repository.DeleteAsync(id);
        }
    }
} 