using CW10.DTOs;
using CW10.Services;
using Microsoft.AspNetCore.Mvc;
using CW10.Exceptions;

namespace CW10.Controllers;

[ApiController]
[Route("/api/[controller]")]
public class TripsController(ITripsService tripsService) : ControllerBase
{

    [HttpGet]
    public async Task<IActionResult> GetTrips([FromQuery] int page = 1, [FromQuery] int pageSize = 10)
    {
        try
        {
            return Ok(await tripsService.GetTrips(page, pageSize));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (TripsException e)
        {
            return BadRequest(e.Message);
        }
    }

    [HttpPost("{idTrip}/clients")]
    public async Task<IActionResult> AddClientToTrip([FromRoute] int idTrip, [FromBody] TripsPostDto client)
    {
        try
        {
            return Ok(await tripsService.AddClientToTrip(idTrip, client));
        }
        catch (NotFoundException e)
        {
            return NotFound(e.Message);
        }
        catch (TripsException e)
        {
            return BadRequest(e.Message);
        }
    }
} 