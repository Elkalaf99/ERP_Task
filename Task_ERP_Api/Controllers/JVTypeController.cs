using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.Services.JVType;
using Task_ERP_Api.DTOs;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class JVTypeController : ControllerBase
    {
        private readonly IJVTypeService _service;
        public JVTypeController(IJVTypeService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<JVTypeDto>>> GetAll()
        {
            try
            {
                var jvTypes = await _service.GetAllAsync();
                return Ok(jvTypes);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving JV types: {ex.Message}" });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<JVTypeDto>> GetById(int id)
        {
            try
            {
                var jvType = await _service.GetByIdAsync(id);
                if (jvType == null) return NotFound();
                return Ok(jvType);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while retrieving JV type: {ex.Message}" });
            }
        }

        [HttpPost]
        public async Task<ActionResult<JVTypeDto>> Create(CreateJVTypeDto createJVTypeDto)
        {
            try
            {
                var created = await _service.CreateAsync(createJVTypeDto);
                return CreatedAtAction(nameof(GetById), new { id = created.JVTypeID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the JV type: {ex.Message}" });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<JVTypeDto>> Update(int id, UpdateJVTypeDto updateJVTypeDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateJVTypeDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while updating the JV type: {ex.Message}" });
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
                return StatusCode(500, new { error = $"An error occurred while deleting the JV type: {ex.Message}" });
            }
        }
    }
} 