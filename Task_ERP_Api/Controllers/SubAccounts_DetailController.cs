using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.SubAccounts_Detail;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAccounts_DetailController : ControllerBase
    {
        private readonly ISubAccounts_DetailService _service;
        public SubAccounts_DetailController(ISubAccounts_DetailService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubAccounts_DetailDto>>> GetAll()
        {
            try
            {
                var subAccountDetails = await _service.GetAllAsync();
                return Ok(subAccountDetails);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account details: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubAccounts_DetailDto>> GetById(int id)
        {
            try
            {
                var subAccountDetail = await _service.GetByIdAsync(id);
                if (subAccountDetail == null) return NotFound();
                return Ok(subAccountDetail);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account detail: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubAccounts_DetailDto>> Create(CreateSubAccounts_DetailDto createSubAccountsDetailDto)
        {
            try
            {
                var created = await _service.CreateAsync(createSubAccountsDetailDto);
                return CreatedAtAction(nameof(GetById), new { id = created.SubAccountDetailID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the sub account detail: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubAccounts_DetailDto>> Update(int id, UpdateSubAccounts_DetailDto updateSubAccountsDetailDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateSubAccountsDetailDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the sub account detail: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the sub account detail: {ex.Message}" });
            }
        }
    }
} 