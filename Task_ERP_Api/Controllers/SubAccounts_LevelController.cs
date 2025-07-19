using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.SubAccounts_Level;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAccounts_LevelController : ControllerBase
    {
        private readonly ISubAccounts_LevelService _service;
        public SubAccounts_LevelController(ISubAccounts_LevelService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubAccounts_LevelDto>>> GetAll()
        {
            try
            {
                var subAccountLevels = await _service.GetAllAsync();
                return Ok(subAccountLevels);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account levels: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubAccounts_LevelDto>> GetById(int id)
        {
            try
            {
                var subAccountLevel = await _service.GetByIdAsync(id);
                if (subAccountLevel == null) return NotFound();
                return Ok(subAccountLevel);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account level: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubAccounts_LevelDto>> Create(CreateSubAccounts_LevelDto createSubAccountsLevelDto)
        {
            try
            {
                var created = await _service.CreateAsync(createSubAccountsLevelDto);
                return CreatedAtAction(nameof(GetById), new { id = created.LevelID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the sub account level: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubAccounts_LevelDto>> Update(int id, UpdateSubAccounts_LevelDto updateSubAccountsLevelDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateSubAccountsLevelDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the sub account level: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the sub account level: {ex.Message}" });
            }
        }
    }
} 