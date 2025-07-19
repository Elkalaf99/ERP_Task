using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.SubAccountsType;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAccountsTypeController : ControllerBase
    {
        private readonly ISubAccountsTypeService _service;
        public SubAccountsTypeController(ISubAccountsTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubAccountsTypeDto>>> GetAll()
        {
            try
            {
                var subAccountTypes = await _service.GetAllAsync();
                return Ok(subAccountTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account types: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubAccountsTypeDto>> GetById(int id)
        {
            try
            {
                var subAccountType = await _service.GetByIdAsync(id);
                if (subAccountType == null) return NotFound();
                return Ok(subAccountType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account type: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubAccountsTypeDto>> Create(CreateSubAccountsTypeDto createSubAccountsTypeDto)
        {
            try
            {
                var created = await _service.CreateAsync(createSubAccountsTypeDto);
                return CreatedAtAction(nameof(GetById), new { id = created.SubAccountTypeID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the sub account type: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubAccountsTypeDto>> Update(int id, UpdateSubAccountsTypeDto updateSubAccountsTypeDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateSubAccountsTypeDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the sub account type: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the sub account type: {ex.Message}" });
            }
        }
    }
} 