using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.SubAccount;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAccountController : ControllerBase
    {
        private readonly ISubAccountService _service;
        public SubAccountController(ISubAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubAccountDto>>> GetAll()
        {
            try
            {
                var subAccounts = await _service.GetAllAsync();
                return Ok(subAccounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub accounts: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubAccountDto>> GetById(int id)
        {
            try
            {
                var subAccount = await _service.GetByIdAsync(id);
                if (subAccount == null) return NotFound();
                return Ok(subAccount);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubAccountDto>> Create(CreateSubAccountDto createSubAccountDto)
        {
            try
            {
                var created = await _service.CreateAsync(createSubAccountDto);
                return CreatedAtAction(nameof(GetById), new { id = created.SubAccountID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the sub account: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubAccountDto>> Update(int id, UpdateSubAccountDto updateSubAccountDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateSubAccountDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the sub account: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the sub account: {ex.Message}" });
            }
        }
    }
} 