using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.JV
{
    public class JVService : IJVService
    {
        private readonly IGenericRepository<Models.JV> _repository;
        private readonly IGenericRepository<Models.Branch> _branchRepository;
        private readonly IGenericRepository<Models.JVType> _jvTypeRepository;
        private readonly IMapper _mapper;

        public JVService(
            IGenericRepository<Models.JV> repository, 
            IGenericRepository<Models.Branch> branchRepository,
            IGenericRepository<Models.JVType> jvTypeRepository,
            IMapper mapper)
        {
            _repository = repository;
            _branchRepository = branchRepository;
            _jvTypeRepository = jvTypeRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JVDto>> GetAllAsync()
        {
            try
            {
                var jvs = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<JVDto>>(jvs);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve journal vouchers: {ex.Message}", ex);
            }
        }

        public async Task<JVDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("JV ID must be greater than zero.", nameof(id));

                var jv = await _repository.GetByIdAsync(id);
                return _mapper.Map<JVDto>(jv);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve journal voucher with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<JVDto> CreateAsync(CreateJVDto createJVDto)
        {
            try
            {
                // Validate input
                if (createJVDto == null)
                    throw new ArgumentNullException(nameof(createJVDto), "Journal voucher data cannot be null.");

                // Validate JV date
                if (createJVDto.JVDate > DateTime.Now)
                    throw new InvalidOperationException("Journal voucher date cannot be in the future.");

                // Validate JV type exists if provided
                if (createJVDto.JVTypeID.HasValue)
                {
                    var jvType = await _jvTypeRepository.GetByIdAsync(createJVDto.JVTypeID.Value);
                    if (jvType == null)
                        throw new InvalidOperationException($"Journal voucher type with ID {createJVDto.JVTypeID} does not exist.");
                }

                // Validate branch exists if provided
                if (createJVDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(createJVDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {createJVDto.BranchID} does not exist.");
                }

                // Validate financial balance (debit must equal credit)
                if (createJVDto.TotalDebit != createJVDto.TotalCredit)
                    throw new InvalidOperationException($"Journal voucher must be balanced. Total debit ({createJVDto.TotalDebit}) must equal total credit ({createJVDto.TotalCredit}).");

                // Validate amounts are positive
                if (createJVDto.TotalDebit < 0)
                    throw new InvalidOperationException("Total debit amount cannot be negative.");

                if (createJVDto.TotalCredit < 0)
                    throw new InvalidOperationException("Total credit amount cannot be negative.");

                // Validate receipt number format if provided
                if (!string.IsNullOrWhiteSpace(createJVDto.ReceiptNo))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(createJVDto.ReceiptNo, @"^[A-Za-z0-9\-_]+$"))
                        throw new InvalidOperationException("Receipt number can only contain letters, numbers, hyphens, and underscores.");
                }

                // Generate JVNo: JV-YYYY-XXXX (sequential per year)
                var year = (createJVDto.JVDate == default ? DateTime.Now : createJVDto.JVDate).Year;
                var allJVs = await _repository.GetAllAsync();
                var yearPrefix = $"JV-{year}-";
                var lastSeq = allJVs
                    .Where(jv => jv.JVNo.StartsWith(yearPrefix))
                    .Select(jv =>
                    {
                        var parts = jv.JVNo.Split('-');
                        if (parts.Length == 3 && int.TryParse(parts[2], out int seq))
                            return seq;
                        return 0;
                    })
                    .DefaultIfEmpty(0)
                    .Max();
                var newSeq = lastSeq + 1;
                var newJVNo = $"JV-{year}-{newSeq.ToString("D4")}";

                var jv = _mapper.Map<Models.JV>(createJVDto);
                jv.JVNo = newJVNo; // Assign generated JVNo

                var createdJV = await _repository.AddAsync(jv);
                return _mapper.Map<JVDto>(createdJV);
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
                throw new InvalidOperationException($"Failed to create journal voucher: {ex.Message}", ex);
            }
        }

        public async Task<JVDto?> UpdateAsync(int id, UpdateJVDto updateJVDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("JV ID must be greater than zero.", nameof(id));

                if (updateJVDto == null)
                    throw new ArgumentNullException(nameof(updateJVDto), "Journal voucher update data cannot be null.");

                var existingJV = await _repository.GetByIdAsync(id);
                if (existingJV == null)
                    return null;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(updateJVDto.JVNo))
                    throw new InvalidOperationException("Journal voucher number is required.");

                // Validate JV number format
                if (!System.Text.RegularExpressions.Regex.IsMatch(updateJVDto.JVNo, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Journal voucher number can only contain letters, numbers, hyphens, and underscores.");

                // Check for duplicate JV number (excluding current JV)
                var existingJVs = await _repository.GetAllAsync();
                if (existingJVs.Any(jv => jv.JVNo == updateJVDto.JVNo && jv.JVID != id))
                    throw new InvalidOperationException($"Journal voucher number '{updateJVDto.JVNo}' already exists.");

                // Validate JV date
                if (updateJVDto.JVDate > DateTime.Now)
                    throw new InvalidOperationException("Journal voucher date cannot be in the future.");

                // Validate JV type exists if provided
                if (updateJVDto.JVTypeID.HasValue)
                {
                    var jvType = await _jvTypeRepository.GetByIdAsync(updateJVDto.JVTypeID.Value);
                    if (jvType == null)
                        throw new InvalidOperationException($"Journal voucher type with ID {updateJVDto.JVTypeID} does not exist.");
                }

                // Validate branch exists if provided
                if (updateJVDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(updateJVDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {updateJVDto.BranchID} does not exist.");
                }

                // Validate financial balance (debit must equal credit)
                if (updateJVDto.TotalDebit != updateJVDto.TotalCredit)
                    throw new InvalidOperationException($"Journal voucher must be balanced. Total debit ({updateJVDto.TotalDebit}) must equal total credit ({updateJVDto.TotalCredit}).");

                // Validate amounts are positive
                if (updateJVDto.TotalDebit < 0)
                    throw new InvalidOperationException("Total debit amount cannot be negative.");

                if (updateJVDto.TotalCredit < 0)
                    throw new InvalidOperationException("Total credit amount cannot be negative.");

                // Validate receipt number format if provided
                if (!string.IsNullOrWhiteSpace(updateJVDto.ReceiptNo))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(updateJVDto.ReceiptNo, @"^[A-Za-z0-9\-_]+$"))
                        throw new InvalidOperationException("Receipt number can only contain letters, numbers, hyphens, and underscores.");
                }

                _mapper.Map(updateJVDto, existingJV);
                var updatedJV = await _repository.UpdateAsync(existingJV);
                return _mapper.Map<JVDto>(updatedJV);
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
                throw new InvalidOperationException($"Failed to update journal voucher with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("JV ID must be greater than zero.", nameof(id));

                var existingJV = await _repository.GetByIdAsync(id);
                if (existingJV == null)
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
                throw new InvalidOperationException($"Failed to delete journal voucher with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 