using Keepr.Models;
using Keepr.Services;

namespace Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class VaultsController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly VaultsService _vaultsService;
    private readonly VaultKeepsService _vaultKeepsService;

    public VaultsController(Auth0Provider auth0Provider, VaultsService vaultsService, VaultKeepsService vaultKeepsService)
    {
        _auth0Provider = auth0Provider;
        _vaultsService = vaultsService;
        _vaultKeepsService = vaultKeepsService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Vault>> CreateVault([FromBody] Vault newVault)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            newVault.CreatorId = userInfo.Id;

            Vault createdVault = _vaultsService.CreateVault(newVault);
            return Ok(createdVault);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{vaultId}")]
    public async Task<ActionResult<Vault>> GetVaultByVaultId(int vaultId)
    {
        try
        {
            //Get user info if any for vault keeps
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            Vault vault = _vaultsService.GetVaultByVaultId(vaultId, userInfo?.Id); //Passes down user id if any
            return Ok(vault);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }


    [Authorize]
    [HttpPut("{vaultId}")]
    public async Task<ActionResult<Vault>> EditVault(int vaultId, [FromBody] Vault updatedVault)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            Vault vault = _vaultsService.UpdateVaultById(vaultId, updatedVault, userInfo.Id);
            return Ok(vault);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpDelete("{vaultId}")]
    public async Task<ActionResult<string>> DeleteVault(int vaultId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            string message = _vaultsService.DeleteVault(vaultId, userInfo.Id);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{vaultId}/keeps")]
    public async Task<ActionResult<List<KeepVaultView>>> GetKeepsByVaultId(int vaultId)
    {
        try
        {
            //Get user info if any for vault keeps
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            List<KeepVaultView> vaultKeeps = _vaultKeepsService.GetKeepsByVaultId(vaultId, userInfo?.Id); //Passes down user id if any
            return Ok(vaultKeeps);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    //STUB - get vault keep by id
    [HttpGet("{vaultId}/keeps/{vaultKeepId}")]
    public async Task<ActionResult<KeepVaultView>> GetVaultKeepById(int vaultId, int vaultKeepId)
    {
        try
        {
            //Get user info if any for vault keeps
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            List<KeepVaultView> vaultKeeps = _vaultKeepsService.GetKeepsByVaultId(vaultId, userInfo?.Id); //Passes down user id if any
            KeepVaultView vaultKeep = vaultKeeps.Find(vk => vk.VaultKeepId == vaultKeepId); //TODO - optimize in repo?
            if (vaultKeep == null)
            {
                throw new Exception("VaultKeep not found in this Vault");
            }
            return Ok(vaultKeep);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}
