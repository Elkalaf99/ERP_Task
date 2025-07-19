using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.City;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CityController : ControllerBase
    {
        private readonly ICityService _service;
        public CityController(ICityService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<CityDto>>> GetAll()
        {
            try
            {
                var cities = await _service.GetAllAsync();
                return Ok(cities);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving cities: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CityDto>> GetById(int id)
        {
            try
            {
                var city = await _service.GetByIdAsync(id);
                if (city == null) return NotFound();
                return Ok(city);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving city: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<CityDto>> Create(CreateCityDto createCityDto)
        {
            try
            {
                var created = await _service.CreateAsync(createCityDto);
                return CreatedAtAction(nameof(GetById), new { id = created.CityID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the city: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<CityDto>> Update(int id, UpdateCityDto updateCityDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateCityDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the city: {ex.Message}" });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                var deleted = await _service.DeleteAsync(id);
                if (!deleted) return NotFound();
                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while deleting the city: {ex.Message}" });
            }
        }
    }
} 