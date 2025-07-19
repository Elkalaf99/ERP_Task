using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;
using Task_ERP_Api.DTOs;
using Task_ERP_Api.Services.Account;

namespace Task_ERP_Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _service;
        public AccountController(IAccountService service)
        {
            _service = service;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<AccountDto>>> GetAll()
        {
            var accounts = await _service.GetAllAsync();
            return Ok(accounts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<AccountDto>> GetById(int id)
        {
            var account = await _service.GetByIdAsync(id);
            if (account == null) return NotFound();
            return Ok(account);
        }

        [HttpPost]
        public async Task<ActionResult<AccountDto>> Create(CreateAccountDto createAccountDto)
        {
            try
            {
                var created = await _service.CreateAsync(createAccountDto);
                return CreatedAtAction(nameof(GetById), new { id = created.AccountID }, created);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = $"An error occurred while creating the account: {ex.Message}", details = ex.ToString() });
            }
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<AccountDto>> Update(int id, UpdateAccountDto updateAccountDto)
        {
            try
            {
                var updated = await _service.UpdateAsync(id, updateAccountDto);
                if (updated == null) return NotFound();
                return Ok(updated);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception)
            {
                return StatusCode(500, new { error = "An error occurred while updating the account." });
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var deleted = await _service.DeleteAsync(id);
            if (!deleted) return NotFound();
            return NoContent();
        }
    }
} 