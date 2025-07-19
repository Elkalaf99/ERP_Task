using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.SubAccount
{
    public class SubAccountService : ISubAccountService
    {
        private readonly IGenericRepository<Models.SubAccount> _repository;
        private readonly IGenericRepository<Models.Branch> _branchRepository;
        private readonly IGenericRepository<Models.SubAccounts_Level> _levelRepository;
        private readonly IGenericRepository<Models.SubAccountsType> _typeRepository;
        private readonly IMapper _mapper;

        public SubAccountService(
            IGenericRepository<Models.SubAccount> repository,
            IGenericRepository<Models.Branch> branchRepository,
            IGenericRepository<Models.SubAccounts_Level> levelRepository,
            IGenericRepository<Models.SubAccountsType> typeRepository,
            IMapper mapper)
        {
            _repository = repository;
            _branchRepository = branchRepository;
            _levelRepository = levelRepository;
            _typeRepository = typeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<SubAccountDto>> GetAllAsync()
        {
            try
            {
                var subAccounts = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<SubAccountDto>>(subAccounts);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve sub accounts: {ex.Message}", ex);
            }
        }

        public async Task<SubAccountDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Sub account ID must be greater than zero.", nameof(id));

                var subAccount = await _repository.GetByIdAsync(id);
                return _mapper.Map<SubAccountDto>(subAccount);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve sub account with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<SubAccountDto> CreateAsync(CreateSubAccountDto createSubAccountDto)
        {
            try
            {
                // Validate input
                if (createSubAccountDto == null)
                    throw new ArgumentNullException(nameof(createSubAccountDto), "Sub account data cannot be null.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(createSubAccountDto.SubAccountNumber))
                    throw new InvalidOperationException("Sub account number is required.");

                if (string.IsNullOrWhiteSpace(createSubAccountDto.SubAccountNameAr))
                    throw new InvalidOperationException("Arabic sub account name is required.");

                // Validate sub account number format
                if (!System.Text.RegularExpressions.Regex.IsMatch(createSubAccountDto.SubAccountNumber, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Sub account number can only contain letters, numbers, hyphens, and underscores.");

                // Validate string lengths
                if (createSubAccountDto.SubAccountNumber.Length > 50)
                    throw new InvalidOperationException("Sub account number cannot exceed 50 characters.");

                if (createSubAccountDto.SubAccountNameAr.Length > 100)
                    throw new InvalidOperationException("Arabic sub account name cannot exceed 100 characters.");

                if (!string.IsNullOrWhiteSpace(createSubAccountDto.SubAccountNameEn) && createSubAccountDto.SubAccountNameEn.Length > 100)
                    throw new InvalidOperationException("English sub account name cannot exceed 100 characters.");

                // Check for duplicate sub account number
                var existingSubAccounts = await _repository.GetAllAsync();
                if (existingSubAccounts.Any(sa => sa.SubAccountNumber == createSubAccountDto.SubAccountNumber))
                    throw new InvalidOperationException($"Sub account number '{createSubAccountDto.SubAccountNumber}' already exists.");

                // Validate branch exists if provided
                if (createSubAccountDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(createSubAccountDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {createSubAccountDto.BranchID} does not exist.");
                }

                // Validate level exists
                var level = await _levelRepository.GetByIdAsync(createSubAccountDto.LevelID);
                if (level == null)
                    throw new InvalidOperationException($"Level with ID {createSubAccountDto.LevelID} does not exist.");

                // Validate sub account type exists if provided
                if (createSubAccountDto.SubAccountTypeID.HasValue)
                {
                    var subAccountType = await _typeRepository.GetByIdAsync(createSubAccountDto.SubAccountTypeID.Value);
                    if (subAccountType == null)
                        throw new InvalidOperationException($"Sub account type with ID {createSubAccountDto.SubAccountTypeID} does not exist.");
                }

                // Validate parent exists if provided
                if (createSubAccountDto.ParentID.HasValue)
                {
                    var parent = await _repository.GetByIdAsync(createSubAccountDto.ParentID.Value);
                    if (parent == null)
                        throw new InvalidOperationException($"Parent sub account with ID {createSubAccountDto.ParentID} does not exist.");

                    // Validate hierarchy - parent should be at a lower level
                    if (parent.LevelID >= createSubAccountDto.LevelID)
                        throw new InvalidOperationException("Parent sub account must be at a lower level than the child.");
                }

                var subAccount = _mapper.Map<Models.SubAccount>(createSubAccountDto);
                var createdSubAccount = await _repository.AddAsync(subAccount);
                return _mapper.Map<SubAccountDto>(createdSubAccount);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to create sub account: {ex.Message}", ex);
            }
        }

        public async Task<SubAccountDto?> UpdateAsync(int id, UpdateSubAccountDto updateSubAccountDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("Sub account ID must be greater than zero.", nameof(id));

                if (updateSubAccountDto == null)
                    throw new ArgumentNullException(nameof(updateSubAccountDto), "Sub account update data cannot be null.");

                var existingSubAccount = await _repository.GetByIdAsync(id);
                if (existingSubAccount == null)
                    return null;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(updateSubAccountDto.SubAccountNumber))
                    throw new InvalidOperationException("Sub account number is required.");

                if (string.IsNullOrWhiteSpace(updateSubAccountDto.SubAccountNameAr))
                    throw new InvalidOperationException("Arabic sub account name is required.");

                // Validate sub account number format
                if (!System.Text.RegularExpressions.Regex.IsMatch(updateSubAccountDto.SubAccountNumber, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Sub account number can only contain letters, numbers, hyphens, and underscores.");

                // Validate string lengths
                if (updateSubAccountDto.SubAccountNumber.Length > 50)
                    throw new InvalidOperationException("Sub account number cannot exceed 50 characters.");

                if (updateSubAccountDto.SubAccountNameAr.Length > 100)
                    throw new InvalidOperationException("Arabic sub account name cannot exceed 100 characters.");

                if (!string.IsNullOrWhiteSpace(updateSubAccountDto.SubAccountNameEn) && updateSubAccountDto.SubAccountNameEn.Length > 100)
                    throw new InvalidOperationException("English sub account name cannot exceed 100 characters.");

                // Check for duplicate sub account number (excluding current sub account)
                var existingSubAccounts = await _repository.GetAllAsync();
                if (existingSubAccounts.Any(sa => sa.SubAccountNumber == updateSubAccountDto.SubAccountNumber && sa.SubAccountID != id))
                    throw new InvalidOperationException($"Sub account number '{updateSubAccountDto.SubAccountNumber}' already exists.");

                // Validate branch exists if provided
                if (updateSubAccountDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(updateSubAccountDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {updateSubAccountDto.BranchID} does not exist.");
                }

                // Validate level exists
                var level = await _levelRepository.GetByIdAsync(updateSubAccountDto.LevelID);
                if (level == null)
                    throw new InvalidOperationException($"Level with ID {updateSubAccountDto.LevelID} does not exist.");

                // Validate sub account type exists if provided
                if (updateSubAccountDto.SubAccountTypeID.HasValue)
                {
                    var subAccountType = await _typeRepository.GetByIdAsync(updateSubAccountDto.SubAccountTypeID.Value);
                    if (subAccountType == null)
                        throw new InvalidOperationException($"Sub account type with ID {updateSubAccountDto.SubAccountTypeID} does not exist.");
                }

                // Validate parent exists if provided
                if (updateSubAccountDto.ParentID.HasValue)
                {
                    var parent = await _repository.GetByIdAsync(updateSubAccountDto.ParentID.Value);
                    if (parent == null)
                        throw new InvalidOperationException($"Parent sub account with ID {updateSubAccountDto.ParentID} does not exist.");

                    // Validate hierarchy - parent should be at a lower level
                    if (parent.LevelID >= updateSubAccountDto.LevelID)
                        throw new InvalidOperationException("Parent sub account must be at a lower level than the child.");

                    // Prevent circular reference - parent cannot be the same as current sub account
                    if (updateSubAccountDto.ParentID.Value == id)
                        throw new InvalidOperationException("Sub account cannot be its own parent.");
                }

                _mapper.Map(updateSubAccountDto, existingSubAccount);
                var updatedSubAccount = await _repository.UpdateAsync(existingSubAccount);
                return _mapper.Map<SubAccountDto>(updatedSubAccount);
            }
            catch (ArgumentNullException)
            {
                throw;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to update sub account with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Sub account ID must be greater than zero.", nameof(id));

                var existingSubAccount = await _repository.GetByIdAsync(id);
                if (existingSubAccount == null)
                    return false;


                return await _repository.DeleteAsync(id);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (InvalidOperationException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to delete sub account with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 