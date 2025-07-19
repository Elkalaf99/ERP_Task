using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.JVDetail
{
    public class JVDetailService : IJVDetailService
    {
        private readonly IGenericRepository<Models.JVDetail> _repository;
        private readonly IGenericRepository<Models.JV> _jvRepository;
        private readonly IGenericRepository<Models.Account> _accountRepository;
        private readonly IGenericRepository<Models.SubAccount> _subAccountRepository;
        private readonly IGenericRepository<Models.Branch> _branchRepository;
        private readonly IMapper _mapper;

        public JVDetailService(
            IGenericRepository<Models.JVDetail> repository,
            IGenericRepository<Models.JV> jvRepository,
            IGenericRepository<Models.Account> accountRepository,
            IGenericRepository<Models.SubAccount> subAccountRepository,
            IGenericRepository<Models.Branch> branchRepository,
            IMapper mapper)
        {
            _repository = repository;
            _jvRepository = jvRepository;
            _accountRepository = accountRepository;
            _subAccountRepository = subAccountRepository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JVDetailDto>> GetAllAsync()
        {
            try
            {
                var jvDetails = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<JVDetailDto>>(jvDetails);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve JV details: {ex.Message}", ex);
            }
        }

        public async Task<JVDetailDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("JV detail ID must be greater than zero.", nameof(id));

                var jvDetail = await _repository.GetByIdAsync(id);
                return _mapper.Map<JVDetailDto>(jvDetail);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve JV detail with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<JVDetailDto> CreateAsync(CreateJVDetailDto createJVDetailDto)
        {
            try
            {
                // Validate input
                if (createJVDetailDto == null)
                    throw new ArgumentNullException(nameof(createJVDetailDto), "JV detail data cannot be null.");

                // Validate JV exists
                var jv = await _jvRepository.GetByIdAsync(createJVDetailDto.JVID);
                if (jv == null)
                    throw new InvalidOperationException($"Journal voucher with ID {createJVDetailDto.JVID} does not exist.");

                // Validate account exists
                var account = await _accountRepository.GetByIdAsync(createJVDetailDto.AccountID);
                if (account == null)
                    throw new InvalidOperationException($"Account with ID {createJVDetailDto.AccountID} does not exist.");

                // Validate sub-account exists if provided
                if (createJVDetailDto.SubAccountID.HasValue)
                {
                    var subAccount = await _subAccountRepository.GetByIdAsync(createJVDetailDto.SubAccountID.Value);
                    if (subAccount == null)
                        throw new InvalidOperationException($"Sub-account with ID {createJVDetailDto.SubAccountID} does not exist.");
                }

                // Validate branch exists if provided
                if (createJVDetailDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(createJVDetailDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {createJVDetailDto.BranchID} does not exist.");
                }

                // Validate financial logic - either debit or credit must be provided, but not both
                if (!createJVDetailDto.Debit.HasValue && !createJVDetailDto.Credit.HasValue)
                    throw new InvalidOperationException("Either debit or credit amount must be provided.");

                if (createJVDetailDto.Debit.HasValue && createJVDetailDto.Credit.HasValue)
                    throw new InvalidOperationException("Cannot have both debit and credit amounts. Only one should be provided.");

                // Validate amounts are positive
                if (createJVDetailDto.Debit.HasValue && createJVDetailDto.Debit.Value < 0)
                    throw new InvalidOperationException("Debit amount cannot be negative.");

                if (createJVDetailDto.Credit.HasValue && createJVDetailDto.Credit.Value < 0)
                    throw new InvalidOperationException("Credit amount cannot be negative.");

                // Validate amounts are not zero (only if both are zero)
                if (createJVDetailDto.Debit.HasValue && createJVDetailDto.Debit.Value == 0 && 
                    createJVDetailDto.Credit.HasValue && createJVDetailDto.Credit.Value == 0)
                    throw new InvalidOperationException("Both debit and credit amounts cannot be zero.");

                // Allow zero values if the other amount is null
                if (createJVDetailDto.Debit.HasValue && createJVDetailDto.Debit.Value == 0 && 
                    !createJVDetailDto.Credit.HasValue)
                    throw new InvalidOperationException("Debit amount cannot be zero when credit is not provided.");

                if (createJVDetailDto.Credit.HasValue && createJVDetailDto.Credit.Value == 0 && 
                    !createJVDetailDto.Debit.HasValue)
                    throw new InvalidOperationException("Credit amount cannot be zero when debit is not provided.");

                // TODO: Validate that the JV is not already posted/finalized
                // if (jv.IsPosted)
                //     throw new InvalidOperationException("Cannot add details to a posted journal voucher.");

                var jvDetail = _mapper.Map<Models.JVDetail>(createJVDetailDto);
                var createdJVDetail = await _repository.AddAsync(jvDetail);
                return _mapper.Map<JVDetailDto>(createdJVDetail);
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
                // Log the inner exception details for debugging
                var innerException = ex.InnerException;
                var errorMessage = $"Failed to create JV detail: {ex.Message}";
                
                if (innerException != null)
                {
                    errorMessage += $" | Inner Exception: {innerException.Message}";
                }
                
                throw new InvalidOperationException(errorMessage, ex);
            }
        }

        public async Task<JVDetailDto?> UpdateAsync(int id, UpdateJVDetailDto updateJVDetailDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("JV detail ID must be greater than zero.", nameof(id));

                if (updateJVDetailDto == null)
                    throw new ArgumentNullException(nameof(updateJVDetailDto), "JV detail update data cannot be null.");

                var existingJVDetail = await _repository.GetByIdAsync(id);
                if (existingJVDetail == null)
                    return null;

                // Validate JV exists
                var jv = await _jvRepository.GetByIdAsync(updateJVDetailDto.JVID);
                if (jv == null)
                    throw new InvalidOperationException($"Journal voucher with ID {updateJVDetailDto.JVID} does not exist.");

                // Validate account exists
                var account = await _accountRepository.GetByIdAsync(updateJVDetailDto.AccountID);
                if (account == null)
                    throw new InvalidOperationException($"Account with ID {updateJVDetailDto.AccountID} does not exist.");

                // Validate sub-account exists if provided
                if (updateJVDetailDto.SubAccountID.HasValue)
                {
                    var subAccount = await _subAccountRepository.GetByIdAsync(updateJVDetailDto.SubAccountID.Value);
                    if (subAccount == null)
                        throw new InvalidOperationException($"Sub-account with ID {updateJVDetailDto.SubAccountID} does not exist.");
                }

                // Validate branch exists if provided
                if (updateJVDetailDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(updateJVDetailDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {updateJVDetailDto.BranchID} does not exist.");
                }

                // Validate financial logic - either debit or credit must be provided, but not both
                if (!updateJVDetailDto.Debit.HasValue && !updateJVDetailDto.Credit.HasValue)
                    throw new InvalidOperationException("Either debit or credit amount must be provided.");

                if (updateJVDetailDto.Debit.HasValue && updateJVDetailDto.Credit.HasValue)
                    throw new InvalidOperationException("Cannot have both debit and credit amounts. Only one should be provided.");

                // Validate amounts are positive
                if (updateJVDetailDto.Debit.HasValue && updateJVDetailDto.Debit.Value < 0)
                    throw new InvalidOperationException("Debit amount cannot be negative.");

                if (updateJVDetailDto.Credit.HasValue && updateJVDetailDto.Credit.Value < 0)
                    throw new InvalidOperationException("Credit amount cannot be negative.");

                // Validate amounts are not zero (only if both are zero)
                if (updateJVDetailDto.Debit.HasValue && updateJVDetailDto.Debit.Value == 0 && 
                    updateJVDetailDto.Credit.HasValue && updateJVDetailDto.Credit.Value == 0)
                    throw new InvalidOperationException("Both debit and credit amounts cannot be zero.");

                // TODO: Validate that the JV is not already posted/finalized
                // if (jv.IsPosted)
                //     throw new InvalidOperationException("Cannot modify details of a posted journal voucher.");

                _mapper.Map(updateJVDetailDto, existingJVDetail);
                var updatedJVDetail = await _repository.UpdateAsync(existingJVDetail);
                return _mapper.Map<JVDetailDto>(updatedJVDetail);
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
                throw new InvalidOperationException($"Failed to update JV detail with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("JV detail ID must be greater than zero.", nameof(id));

                var existingJVDetail = await _repository.GetByIdAsync(id);
                if (existingJVDetail == null)
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
                throw new InvalidOperationException($"Failed to delete JV detail with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 