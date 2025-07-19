using Microsoft.AspNetCore.Mvc;
using Task_ERP_Bar.Models;
using Task_ERP_Bar.Services;

namespace Task_ERP_Bar.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AccountController : ControllerBase
    {
        private readonly IAccountService _accountService;

        public AccountController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        // GET: api/account
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Account>>> GetAccounts()
        {
            try
            {
                var accounts = await _accountService.GetAccountsAsync();
                return Ok(accounts);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // GET: api/account/{id}
        [HttpGet("{id}")]
        public async Task<ActionResult<Account>> GetAccount(int id)
        {
            try
            {
                var account = await _accountService.GetAccountByIdAsync(id);
                if (account == null)
                {
                    return NotFound($"Account with ID {id} not found");
                }
                return Ok(account);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // POST: api/account
        [HttpPost]
        public async Task<ActionResult<Account>> CreateAccount([FromBody] Account account)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var createdAccount = await _accountService.CreateAccountAsync(account);
                return CreatedAtAction(nameof(GetAccount), new { id = createdAccount.AccountID }, createdAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // PUT: api/account/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAccount(int id, [FromBody] Account account)
        {
            try
            {
                if (id != account.AccountID)
                {
                    return BadRequest("ID mismatch");
                }

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var updatedAccount = await _accountService.UpdateAccountAsync(id, account);
                if (updatedAccount == null)
                {
                    return NotFound($"Account with ID {id} not found");
                }

                return Ok(updatedAccount);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        // DELETE: api/account/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAccount(int id)
        {
            try
            {
                var result = await _accountService.DeleteAccountAsync(id);
                if (!result)
                {
                    return NotFound($"Account with ID {id} not found");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }
    }
} 