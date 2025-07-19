using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.Branch;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BranchController : ControllerBase
    {
        private readonly IBranchService _service;
        public BranchController(IBranchService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<BranchDto>>> GetAll()
        {
            try
            {
                var branches = await _service.GetAllAsync();
                return Ok(branches);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving branches: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<BranchDto>> GetById(int id)
        {
            try
            {
                var branch = await _service.GetByIdAsync(id);
                if (branch == null) return NotFound();
                return Ok(branch);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving branch: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<BranchDto>> Create(CreateBranchDto createBranchDto)
        {
            try
            {
                var created = await _service.CreateAsync(createBranchDto);
                return CreatedAtAction(nameof(GetById), new { id = created.BranchID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the branch: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<BranchDto>> Update(int id, UpdateBranchDto updateBranchDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateBranchDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the branch: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the branch: {ex.Message}" });
            }
        }
    }
} 