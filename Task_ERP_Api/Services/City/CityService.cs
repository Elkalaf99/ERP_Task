using AutoMapper;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Models;
using Task_ERP_Api.Repositories;

namespace Task_ERP_Api.Services.City
{
    public class CityService : ICityService
    {
        private readonly IGenericRepository<Models.City> _repository;
        private readonly IGenericRepository<Models.Branch> _branchRepository;
        private readonly IMapper _mapper;

        public CityService(
            IGenericRepository<Models.City> repository,
            IGenericRepository<Models.Branch> branchRepository,
            IMapper mapper)
        {
            _repository = repository;
            _branchRepository = branchRepository;
            _mapper = mapper;
        }

        public async Task<IEnumerable<CityDto>> GetAllAsync()
        {
            try
            {
                var cities = await _repository.GetAllAsync();
                return _mapper.Map<IEnumerable<CityDto>>(cities);
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve cities: {ex.Message}", ex);
            }
        }

        public async Task<CityDto?> GetByIdAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("City ID must be greater than zero.", nameof(id));

                var city = await _repository.GetByIdAsync(id);
                return _mapper.Map<CityDto>(city);
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException($"Failed to retrieve city with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<CityDto> CreateAsync(CreateCityDto createCityDto)
        {
            try
            {
                // Validate input
                if (createCityDto == null)
                    throw new ArgumentNullException(nameof(createCityDto), "City data cannot be null.");

                // Validate city code format if provided
                if (!string.IsNullOrWhiteSpace(createCityDto.CityCode))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(createCityDto.CityCode, @"^[A-Za-z0-9\-_]+$"))
                        throw new InvalidOperationException("City code can only contain letters, numbers, hyphens, and underscores.");

                    if (createCityDto.CityCode.Length > 50)
                        throw new InvalidOperationException("City code cannot exceed 50 characters.");
                }

                // Validate city names
                if (!string.IsNullOrWhiteSpace(createCityDto.CityNameAr) && createCityDto.CityNameAr.Length > 100)
                    throw new InvalidOperationException("Arabic city name cannot exceed 100 characters.");

                if (!string.IsNullOrWhiteSpace(createCityDto.CityNameEn) && createCityDto.CityNameEn.Length > 100)
                    throw new InvalidOperationException("English city name cannot exceed 100 characters.");

                // At least one name should be provided
                if (string.IsNullOrWhiteSpace(createCityDto.CityNameAr) && string.IsNullOrWhiteSpace(createCityDto.CityNameEn))
                    throw new InvalidOperationException("At least one city name (Arabic or English) must be provided.");

                // Validate branch exists if provided
                if (createCityDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(createCityDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {createCityDto.BranchID} does not exist.");
                }

                // Check for duplicate city names within the same branch
                var existingCities = await _repository.GetAllAsync();
                var duplicateCheck = existingCities.Where(c => 
                    c.BranchID == createCityDto.BranchID &&
                    ((!string.IsNullOrWhiteSpace(createCityDto.CityNameAr) && c.CityNameAr == createCityDto.CityNameAr) ||
                     (!string.IsNullOrWhiteSpace(createCityDto.CityNameEn) && c.CityNameEn == createCityDto.CityNameEn)));

                if (duplicateCheck.Any())
                    throw new InvalidOperationException("A city with the same name already exists in this branch.");

                var city = _mapper.Map<Models.City>(createCityDto);
                var createdCity = await _repository.AddAsync(city);
                return _mapper.Map<CityDto>(createdCity);
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
                throw new InvalidOperationException($"Failed to create city: {ex.Message}", ex);
            }
        }

        public async Task<CityDto?> UpdateAsync(int id, UpdateCityDto updateCityDto)
        {
            try
            {
                // Validate input
                if (id <= 0)
                    throw new ArgumentException("City ID must be greater than zero.", nameof(id));

                if (updateCityDto == null)
                    throw new ArgumentNullException(nameof(updateCityDto), "City update data cannot be null.");

                var existingCity = await _repository.GetByIdAsync(id);
                if (existingCity == null)
                    return null;

                // Validate city code format if provided
                if (!string.IsNullOrWhiteSpace(updateCityDto.CityCode))
                {
                    if (!System.Text.RegularExpressions.Regex.IsMatch(updateCityDto.CityCode, @"^[A-Za-z0-9\-_]+$"))
                        throw new InvalidOperationException("City code can only contain letters, numbers, hyphens, and underscores.");

                    if (updateCityDto.CityCode.Length > 50)
                        throw new InvalidOperationException("City code cannot exceed 50 characters.");
                }

                // Validate city names
                if (!string.IsNullOrWhiteSpace(updateCityDto.CityNameAr) && updateCityDto.CityNameAr.Length > 100)
                    throw new InvalidOperationException("Arabic city name cannot exceed 100 characters.");

                if (!string.IsNullOrWhiteSpace(updateCityDto.CityNameEn) && updateCityDto.CityNameEn.Length > 100)
                    throw new InvalidOperationException("English city name cannot exceed 100 characters.");

                // At least one name should be provided
                if (string.IsNullOrWhiteSpace(updateCityDto.CityNameAr) && string.IsNullOrWhiteSpace(updateCityDto.CityNameEn))
                    throw new InvalidOperationException("At least one city name (Arabic or English) must be provided.");

                // Validate branch exists if provided
                if (updateCityDto.BranchID.HasValue)
                {
                    var branch = await _branchRepository.GetByIdAsync(updateCityDto.BranchID.Value);
                    if (branch == null)
                        throw new InvalidOperationException($"Branch with ID {updateCityDto.BranchID} does not exist.");
                }

                // Check for duplicate city names within the same branch (excluding current city)
                var existingCities = await _repository.GetAllAsync();
                var duplicateCheck = existingCities.Where(c => 
                    c.CityID != id &&
                    c.BranchID == updateCityDto.BranchID &&
                    ((!string.IsNullOrWhiteSpace(updateCityDto.CityNameAr) && c.CityNameAr == updateCityDto.CityNameAr) ||
                     (!string.IsNullOrWhiteSpace(updateCityDto.CityNameEn) && c.CityNameEn == updateCityDto.CityNameEn)));

                if (duplicateCheck.Any())
                    throw new InvalidOperationException("A city with the same name already exists in this branch.");

                _mapper.Map(updateCityDto, existingCity);
                var updatedCity = await _repository.UpdateAsync(existingCity);
                return _mapper.Map<CityDto>(updatedCity);
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
                throw new InvalidOperationException($"Failed to update city with ID {id}: {ex.Message}", ex);
            }
        }

        public async Task<bool> DeleteAsync(int id)
        {
            try
            {
                if (id <= 0)
                    throw new ArgumentException("City ID must be greater than zero.", nameof(id));

                var existingCity = await _repository.GetByIdAsync(id);
                if (existingCity == null)
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
                throw new InvalidOperationException($"Failed to delete city with ID {id}: {ex.Message}", ex);
            }
        }
    }
} 