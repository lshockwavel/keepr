using Keepr.Models;
using Keepr.Services;

namespace Keepr.Controllers;

[ApiController]
[Route("api/[controller]")]
public class KeepsController : ControllerBase
{
    private readonly Auth0Provider _auth0Provider;
    private readonly KeepService _keepService;

    public KeepsController(Auth0Provider auth0Provider, KeepService keepService)
    {
        _auth0Provider = auth0Provider;
        _keepService = keepService;
    }

    [Authorize]
    [HttpPost]
    public async Task<ActionResult<Keep>> CreateKeep([FromBody] Keep newKeep)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            newKeep.CreatorId = userInfo.Id;
            Keep createdKeep = _keepService.CreateKeep(newKeep);
            return Ok(createdKeep);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }

    }

    [HttpGet]
    public async Task<ActionResult<List<Keep>>> GetAllKeeps()
    {
        try
        {
            //Get user info if any for vault keeps
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);

            List<Keep> keeps = _keepService.GetAllKeeps(userInfo?.Id); //Passes down user id if any
            return Ok(keeps);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [HttpGet("{keepId}")]
    public async Task<ActionResult<Keep>> GetKeepById(int keepId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext); //REVIEW Maybe this will be needed later?

            Keep keep = _keepService.IncrementKeepViews(keepId);
            return Ok(keep);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpPut("{keepId}")]
    public async Task<ActionResult<Keep>> UpdateKeep(int keepId, [FromBody] Keep updatedKeep)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            updatedKeep.Id = keepId;
            Keep keep = _keepService.UpdateKeep(updatedKeep, userInfo.Id);
            return Ok(keep);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }

    [Authorize]
    [HttpDelete("{keepId}")]
    public async Task<ActionResult<string>> DeleteKeep(int keepId)
    {
        try
        {
            Account userInfo = await _auth0Provider.GetUserInfoAsync<Account>(HttpContext);
            string message = _keepService.DeleteKeep(keepId, userInfo.Id);
            return Ok(message);
        }
        catch (Exception exception)
        {
            return BadRequest(exception.Message);
        }
    }
}