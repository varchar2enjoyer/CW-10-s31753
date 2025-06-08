using CW10.Exceptions;
using CW10.Services;
using Microsoft.AspNetCore.Mvc;

namespace CW10.Controllers;


[ApiController]
[Route("/api/[controller]")]
public class ClientsController(IClientsService clientsService) : ControllerBase
{
    [HttpDelete("{idClient:int}")]
    public async Task<ActionResult> DeleteClient([FromRoute] int idClient)
    {
        try
        {
            return Ok(await clientsService.DeleteClient(idClient));
        }
        catch (ClientException e)
        {
            return BadRequest(e.Message);
        }
    }
    
}