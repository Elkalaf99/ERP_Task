using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.SubAccountsClient;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SubAccountsClientController : ControllerBase
    {
        private readonly ISubAccountsClientService _service;
        public SubAccountsClientController(ISubAccountsClientService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<SubAccountsClientDto>>> GetAll()
        {
            try
            {
                var subAccountClients = await _service.GetAllAsync();
                return Ok(subAccountClients);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account clients: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<SubAccountsClientDto>> GetById(int id)
        {
            try
            {
                var subAccountClient = await _service.GetByIdAsync(id);
                if (subAccountClient == null) return NotFound();
                return Ok(subAccountClient);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving sub account client: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<SubAccountsClientDto>> Create(CreateSubAccountsClientDto createSubAccountsClientDto)
        {
            try
            {
                var created = await _service.CreateAsync(createSubAccountsClientDto);
                return CreatedAtAction(nameof(GetById), new { id = created.ClientID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the sub account client: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<SubAccountsClientDto>> Update(int id, UpdateSubAccountsClientDto updateSubAccountsClientDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateSubAccountsClientDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the sub account client: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the sub account client: {ex.Message}" });
            }
        }
    }
} 