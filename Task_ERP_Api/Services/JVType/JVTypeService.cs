using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.JVType
{
    public class JVTypeService : IJVTypeService
    {
        private readonly IGenericRepository<Models.JVType> _repository;
        private readonly IGenericRepository<Models.Branch> _branchRepository;
        private readonly IMapper _mapper;

        public JVTypeService(
            IGenericRepository<Models.JVType> repository,
            IGenericRepository<Models.Branch> branchRepository,
            IMapper mapper)
        {
            _repository = repository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<JVTypeDto>> GetAllAsync()
        {
            try
            {
                var jvTypes = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<JVTypeDto>>(jvTypes);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve JV types: {ex.Message}", ex);
            }
        }

        public async Task<JVTypeDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("JV type ID must be greater than zero.", nameof(id));

                var jvType = await _repository.GetByIdAsync(id);
                return _mapper.Map<JVTypeDto>(jvType);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve JV type with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<JVTypeDto> CreateAsync(CreateJVTypeDto createJVTypeDto)
        {
            try
            {
                // Validate input
                if (createJVTypeDto == null)
                    throw new ArgumentNullException(nameof(createJVTypeDto), "JV type data cannot be null.");

                // Validate required fields
                if (string.IsNullOrWhiteSpace(createJVTypeDto.JVTypeNameAr))
                    throw new InvalidOperationException("Arabic JV type name is required.");

                // Validate string lengths
                if (createJVTypeDto.JVTypeNameAr.Length > 100)
                    throw new InvalidOperationException("Arabic JV type name cannot exceed 100 characters.");

                if (!string.IsNullOrWhiteSpace(createJVTypeDto.JVTypeNameEn) && createJVTypeDto.JVTypeNameEn.Length > 100)
                    throw new InvalidOperationException("English JV type name cannot exceed 100 characters.");

                // Check for duplicate JV type names
                var existingJVTypes = await _repository.GetAllAsync();
                if (existingJVTypes.Any(jt => jt.JVTypeNameAr == createJVTypeDto.JVTypeNameAr))
                    throw new InvalidOperationException($"JV type with Arabic name '{createJVTypeDto.JVTypeNameAr}' already exists.");

                if (!string.IsNullOrWhiteSpace(createJVTypeDto.JVTypeNameEn) && 
                    existingJVTypes.Any(jt => jt.JVTypeNameEn == createJVTypeDto.JVTypeNameEn))
                    throw new InvalidOperationException($"JV type with English name '{createJVTypeDto.JVTypeNameEn}' already exists.");

                // Validate branch exists if provided
                if (createJVTypeDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(createJVTypeDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {createJVTypeDto.BranchID} does not exist.");
                }

                var jvType = _mapper.Map<Models.JVType>(createJVTypeDto);
                var createdJVType = await _repository.AddAsync(jvType);
                return _mapper.Map<JVTypeDto>(createdJVType);
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
                throw new InvalidOperationException($"Failed to create JV type: {ex.Message}", ex);
            }
        }

        public async Task<JVTypeDto?> UpdateAsync(int id, UpdateJVTypeDto updateJVTypeDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("JV type ID must be greater than zero.", nameof(id));

                if (updateJVTypeDto == null)
                    throw new ArgumentNullException(nameof(updateJVTypeDto), "JV type update data cannot be null.");

                var existingJVType = await _repository.GetByIdAsync(id);
                if (existingJVType == null)
                    return null;

                // Validate required fields
                if (string.IsNullOrWhiteSpace(updateJVTypeDto.JVTypeNameAr))
                    throw new InvalidOperationException("Arabic JV type name is required.");

                // Validate string lengths
                if (updateJVTypeDto.JVTypeNameAr.Length > 100)
                    throw new InvalidOperationException("Arabic JV type name cannot exceed 100 characters.");

                if (!string.IsNullOrWhiteSpace(updateJVTypeDto.JVTypeNameEn) && updateJVTypeDto.JVTypeNameEn.Length > 100)
                    throw new InvalidOperationException("English JV type name cannot exceed 100 characters.");

                // Check for duplicate JV type names (excluding current JV type)
                var existingJVTypes = await _repository.GetAllAsync();
                if (existingJVTypes.Any(jt => jt.JVTypeNameAr == updateJVTypeDto.JVTypeNameAr && jt.JVTypeID != id))
                    throw new InvalidOperationException($"JV type with Arabic name '{updateJVTypeDto.JVTypeNameAr}' already exists.");

                if (!string.IsNullOrWhiteSpace(updateJVTypeDto.JVTypeNameEn) && 
                    existingJVTypes.Any(jt => jt.JVTypeNameEn == updateJVTypeDto.JVTypeNameEn && jt.JVTypeID != id))
                    throw new InvalidOperationException($"JV type with English name '{updateJVTypeDto.JVTypeNameEn}' already exists.");

                // Validate branch exists if provided
                if (updateJVTypeDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(updateJVTypeDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {updateJVTypeDto.BranchID} does not exist.");
                }

                _mapper.Map(updateJVTypeDto, existingJVType);
                var updatedJVType = await _repository.UpdateAsync(existingJVType);
                return _mapper.Map<JVTypeDto>(updatedJVType);
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
                throw new InvalidOperationException($"Failed to update JV type with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("JV type ID must be greater than zero.", nameof(id));

                var existingJVType = await _repository.GetByIdAsync(id);
                if (existingJVType == null)
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
                throw new InvalidOperationException($"Failed to delete JV type with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 