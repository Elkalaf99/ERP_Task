using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.Account
{
    public class AccountService : IAccountService
    {
        private readonly IGenericRepository<Models.Account> _repository;
        private readonly IGenericRepository<Models.Branch> _branchRepository;
        private readonly IMapper _mapper;

        public AccountService(
            IGenericRepository<Models.Account> repository, 
            IGenericRepository<Models.Branch> branchRepository,
            IMapper mapper)
        {
            _repository = repository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<AccountDto>> GetAllAsync()
        {
            try
            {
                var accounts = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<AccountDto>>(accounts);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve accounts: {ex.Message}", ex);
            }
        }

        public async Task<AccountDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Account ID must be greater than zero.", nameof(id));

                var account = await _repository.GetByIdAsync(id);
                return _mapper.Map<AccountDto>(account);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve account with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<AccountDto> CreateAsync(CreateAccountDto createAccountDto)
        {
            try
            {
                // Validate input
                if (createAccountDto == null)
                    throw new ArgumentNullException(nameof(createAccountDto), "Account data cannot be null.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(createAccountDto.AccountNumber))
                    throw new InvalidOperationException("Account number is required.");

                if (string.IsNullOrWhiteSpace(createAccountDto.AccountNameAr))
                    throw new InvalidOperationException("Arabic account name is required.");

                // Validate account number format (assuming it should be alphanumeric)
                if (!System.Text.RegularExpressions.Regex.IsMatch(createAccountDto.AccountNumber, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Account number can only contain letters, numbers, hyphens, and underscores.");

                // Check for duplicate account number
                var existingAccounts = await _repository.GetAllAsync();
                if (existingAccounts.Any(a => a.AccountNumber == createAccountDto.AccountNumber))
                    throw new InvalidOperationException($"Account number '{createAccountDto.AccountNumber}' already exists.");

                // Validate branch exists if provided
                if (createAccountDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(createAccountDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {createAccountDto.BranchID} does not exist.");
                }

                var account = _mapper.Map<Models.Account>(createAccountDto);
                var createdAccount = await _repository.AddAsync(account);
                return _mapper.Map<AccountDto>(createdAccount);
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
                throw new InvalidOperationException($"Failed to create account: {ex.Message}", ex);
            }
        }

        public async Task<AccountDto?> UpdateAsync(int id, UpdateAccountDto updateAccountDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("Account ID must be greater than zero.", nameof(id));

                if (updateAccountDto == null)
                    throw new ArgumentNullException(nameof(updateAccountDto), "Account update data cannot be null.");

                var existingAccount = await _repository.GetByIdAsync(id);
                if (existingAccount == null)
                    return null;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(updateAccountDto.AccountNumber))
                    throw new InvalidOperationException("Account number is required.");

                if (string.IsNullOrWhiteSpace(updateAccountDto.AccountNameAr))
                    throw new InvalidOperationException("Arabic account name is required.");

                // Validate account number format
                if (!System.Text.RegularExpressions.Regex.IsMatch(updateAccountDto.AccountNumber, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Account number can only contain letters, numbers, hyphens, and underscores.");

                // Check for duplicate account number (excluding current account)
                var existingAccounts = await _repository.GetAllAsync();
                if (existingAccounts.Any(a => a.AccountNumber == updateAccountDto.AccountNumber && a.AccountID != id))
                    throw new InvalidOperationException($"Account number '{updateAccountDto.AccountNumber}' already exists.");

                // Validate branch exists if provided
                if (updateAccountDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(updateAccountDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {updateAccountDto.BranchID} does not exist.");
                }

                _mapper.Map(updateAccountDto, existingAccount);
                var updatedAccount = await _repository.UpdateAsync(existingAccount);
                return _mapper.Map<AccountDto>(updatedAccount);
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
                throw new InvalidOperationException($"Failed to update account with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Account ID must be greater than zero.", nameof(id));

                var existingAccount = await _repository.GetByIdAsync(id);
                if (existingAccount == null)
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
                throw new InvalidOperationException($"Failed to delete account with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 