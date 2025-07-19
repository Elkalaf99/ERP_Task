using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.JVDetail;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JVDetailController : ControllerBase
    {
        private readonly IJVDetailService _service;
        public JVDetailController(IJVDetailService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JVDetailDto>>> GetAll()
        {
            try
            {
                var jvDetails = await _service.GetAllAsync();
                return Ok(jvDetails);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving JV details: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JVDetailDto>> GetById(int id)
        {
            try
            {
                var jvDetail = await _service.GetByIdAsync(id);
                if (jvDetail == null) return NotFound();
                return Ok(jvDetail);
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
                return StatusCode(500, new { error = $"An error occurred while retrieving JV detail: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<JVDetailDto>> Create(CreateJVDetailDto createJVDetailDto)
        {
            try
            {
                var created = await _service.CreateAsync(createJVDetailDto);
                return CreatedAtAction(nameof(GetById), new { id = created.JVDetailID }, created);
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
                return StatusCode(500, new { error = $"An error occurred while creating the JV detail: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JVDetailDto>> Update(int id, UpdateJVDetailDto updateJVDetailDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateJVDetailDto);
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
                return StatusCode(500, new { error = $"An error occurred while updating the JV detail: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the JV detail: {ex.Message}" });
            }
        }
    }
} 