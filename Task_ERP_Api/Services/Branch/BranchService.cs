using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.Branch
{
    public class BranchService : IBranchService
    {
        private readonly IGenericRepository<Models.Branch> _repository;
        private readonly IMapper _mapper;

        public BranchService(IGenericRepository<Models.Branch> repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<BranchDto>> GetAllAsync()
        {
            try
            {
                var branches = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<BranchDto>>(branches);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve branches: {ex.Message}", ex);
            }
        }

        public async Task<BranchDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Branch ID must be greater than zero.", nameof(id));

                var branch = await _repository.GetByIdAsync(id);
                return _mapper.Map<BranchDto>(branch);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve branch with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<BranchDto> CreateAsync(CreateBranchDto createBranchDto)
        {
            try
            {
                // Validate input
                if (createBranchDto == null)
                    throw new ArgumentNullException(nameof(createBranchDto), "Branch data cannot be null.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(createBranchDto.BranchCode))
                    throw new InvalidOperationException("Branch code is required.");

                if (string.IsNullOrWhiteSpace(createBranchDto.BranchNameAr))
                    throw new InvalidOperationException("Arabic branch name is required.");

                // Validate branch code format (alphanumeric + hyphens/underscores)
                if (!System.Text.RegularExpressions.Regex.IsMatch(createBranchDto.BranchCode, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Branch code can only contain letters, numbers, hyphens, and underscores.");

                // Check for duplicate branch code
                var existingBranches = await _repository.GetAllAsync();
                if (existingBranches.Any(b => b.BranchCode == createBranchDto.BranchCode))
                    throw new InvalidOperationException($"Branch code '{createBranchDto.BranchCode}' already exists.");

                // Validate string lengths
                if (createBranchDto.BranchCode.Length > 50)
                    throw new InvalidOperationException("Branch code cannot exceed 50 characters.");

                if (createBranchDto.BranchNameAr.Length > 50)
                    throw new InvalidOperationException("Arabic branch name cannot exceed 50 characters.");

                if (!string.IsNullOrWhiteSpace(createBranchDto.BranchNameEn) && createBranchDto.BranchNameEn.Length > 50)
                    throw new InvalidOperationException("English branch name cannot exceed 50 characters.");

                var branch = _mapper.Map<Models.Branch>(createBranchDto);
                var createdBranch = await _repository.AddAsync(branch);
                return _mapper.Map<BranchDto>(createdBranch);
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
                throw new InvalidOperationException($"Failed to create branch: {ex.Message}", ex);
            }
        }

        public async Task<BranchDto?> UpdateAsync(int id, UpdateBranchDto updateBranchDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("Branch ID must be greater than zero.", nameof(id));

                if (updateBranchDto == null)
                    throw new ArgumentNullException(nameof(updateBranchDto), "Branch update data cannot be null.");

                var existingBranch = await _repository.GetByIdAsync(id);
                if (existingBranch == null)
                    return null;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(updateBranchDto.BranchCode))
                    throw new InvalidOperationException("Branch code is required.");

                if (string.IsNullOrWhiteSpace(updateBranchDto.BranchNameAr))
                    throw new InvalidOperationException("Arabic branch name is required.");

                // Validate branch code format
                if (!System.Text.RegularExpressions.Regex.IsMatch(updateBranchDto.BranchCode, @"^[A-Za-z0-9\-_]+$"))
                    throw new InvalidOperationException("Branch code can only contain letters, numbers, hyphens, and underscores.");

                // Check for duplicate branch code (excluding current branch)
                var existingBranches = await _repository.GetAllAsync();
                if (existingBranches.Any(b => b.BranchCode == updateBranchDto.BranchCode && b.BranchID != id))
                    throw new InvalidOperationException($"Branch code '{updateBranchDto.BranchCode}' already exists.");

                // Validate string lengths
                if (updateBranchDto.BranchCode.Length > 50)
                    throw new InvalidOperationException("Branch code cannot exceed 50 characters.");

                if (updateBranchDto.BranchNameAr.Length > 50)
                    throw new InvalidOperationException("Arabic branch name cannot exceed 50 characters.");

                if (!string.IsNullOrWhiteSpace(updateBranchDto.BranchNameEn) && updateBranchDto.BranchNameEn.Length > 50)
                    throw new InvalidOperationException("English branch name cannot exceed 50 characters.");

                _mapper.Map(updateBranchDto, existingBranch);
                var updatedBranch = await _repository.UpdateAsync(existingBranch);
                return _mapper.Map<BranchDto>(updatedBranch);
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
                throw new InvalidOperationException($"Failed to update branch with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("Branch ID must be greater than zero.", nameof(id));

                var existingBranch = await _repository.GetByIdAsync(id);
                if (existingBranch == null)
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
                throw new InvalidOperationException($"Failed to delete branch with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 