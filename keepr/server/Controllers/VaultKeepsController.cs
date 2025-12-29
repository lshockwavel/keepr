using Keepr.Models;
using Keepr.Services;

namespace Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultKeepsController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly VaultKeepsService _vaultKeepsService;

    public VaultKeepsController(Auth0Provider auth0Provider, VaultKeepsService vaultKeepsService)
    {
        _auth0Provider = auth0Provider;
        _vaultKeepsService = vaultKeepsService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<VaultKeep>> CreateVaultKeep([FromBody] VaultKeep vaultKeep)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            VaultKeep createdVaultKeep = _vaultKeepsService.CreateVaultKeep(vaultKeep, userInfo.Id);
            return Ok(createdVaultKeep);
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }

    [Authorize]
    [HttpDelete("{vaultKeepId}")]
    public async Task<ActionResult<string>> DeleteVaultKeep(int vaultKeepId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _vaultKeepsService.DeleteVaultKeep(vaultKeepId, userInfo.Id);
            return Ok("VaultKeep deleted successfully");
        }
        catch (Exception e)
        {
            return BadRequest(e.Message);
        }
    }
}