using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.JV;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JVController : ControllerBase
    {
        private readonly IJVService _service;
        public JVController(IJVService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JVDto>>> GetAll()
        {
            try
            {
                var jvs = await _service.GetAllAsync();
                return Ok(jvs);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving journal vouchers: {ex.Message}" });
            }
        }

        [HttpGet("{id:int}")]
        public async Task<ActionResult<JVDto>> GetById(int id)
        {
            try
            {
                var jv = await _service.GetByIdAsync(id);
                if (jv == null) return NotFound();
                return Ok(jv);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving journal voucher: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<JVDto>> Create(CreateJVDto createJVDto)
        {
            try
            {
                var created = await _service.CreateAsync(createJVDto);
                return CreatedAtAction(nameof(GetById), new { id = created.JVID }, created);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the journal voucher: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JVDto>> Update(int id, UpdateJVDto updateJVDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateJVDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (ArgumentNullException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the journal voucher: {ex.Message}" });
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
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while deleting the journal voucher: {ex.Message}" });
            }
        }
    }
} 