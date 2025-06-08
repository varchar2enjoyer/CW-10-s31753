using CW10.Data;
using CW10.DTOs;
using CW10.Exceptions;
using CW10.Models;
using Microsoft.EntityFrameworkCore;

namespace CW10.Services;

public interface ITripsService
{
    public Task<TripsGetDto> GetTrips(int page, int pageSize);
    public Task<int> AddClientToTrip(int idTrip, TripsPostDto client);
}

public class TripsService(MasterContext context) : ITripsService
{
    public async Task<TripsGetDto> GetTrips(int page, int pageSize)
    {
        if (pageSize < 1)
        {
            throw new TripsException("Invalid page size");
        }

        var tripsQuery = await context.Trips.Select(trip => new Trip
        {
            IdTrip = trip.IdTrip,
            Name = trip.Name,
            Description = trip.Description,
            DateFrom = trip.DateFrom,
            DateTo = trip.DateTo,
            MaxPeople = trip.MaxPeople,
            ClientTrips = trip.ClientTrips,
            IdCountries = trip.IdCountries
        }).ToListAsync();

        var totalTrips = tripsQuery.Count;
        if (totalTrips == 0)
        {
            throw new TripsException("No trips found");
        }

        var pageCount = (int)Math.Ceiling((double)totalTrips / pageSize);
        var tripsPage = tripsQuery
            .Skip((page - 1) * pageSize)
            .Take(pageSize);

        var trips = tripsPage.Select(t => new SingleTripGetDto
        {
            Name = t.Name,
            Description = t.Description,
            DateFrom = t.DateFrom,
            DateTo = t.DateTo,
            MaxPeople = t.MaxPeople,
            Countries = t.IdCountries.Select(c => new CountryGetDto { Name = c.Name }).ToList(),
            Clients = t.ClientTrips.Select(ct => context.Clients
                    .Where(c => c.IdClient == ct.IdClient)
                    .Select(c => new ClientGetDto
                    {
                        FirstName = c.FirstName,
                        LastName = c.LastName
                    }).FirstOrDefault() ?? throw new ClientException("Client not found")
            ).ToList()
        }).ToList();

        return new TripsGetDto
        {
            PageNum = page,
            PageSize = pageSize,
            AllPages = pageCount,
            Trips = trips
        };
    }

    public async Task<int> AddClientToTrip(int idTrip, TripsPostDto dto)
    {
        
        var client = await context.Clients
            .Include(client => client.ClientTrips)
            .FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);

        if (client != null)
        {
            if (client.ClientTrips.Any(t => dto.IdTrip == t.IdTrip))
            {
                throw new TripsException("Client is already added to this trip");
            }

            throw new TripsException("Client already exists");
        }

        var trip = await context.Trips.FindAsync(idTrip);

        if (trip == null)
        {
            throw new TripsException("Trip not found");
        }

        if (trip.DateFrom <= DateTime.Now)
        {
            throw new TripsException("Cannot add client to trip that is in the past");
        }

        await context.Clients.AddAsync(new Client
        {
            FirstName = dto.FirstName,
            LastName = dto.LastName,
            Telephone = dto.Telephone,
            Email = dto.Email,
            Pesel = dto.Pesel
        });
        await context.SaveChangesAsync();
        
        var addedClient = await context.Clients.Include(cl => cl.ClientTrips).FirstOrDefaultAsync(c => c.Pesel == dto.Pesel);

        if (addedClient == null)
        {
            throw new ClientException("Client not found");
        }
        
        await context.ClientTrips.AddAsync(new ClientTrip
        {
            IdClient = addedClient.IdClient,
            IdTrip = dto.IdTrip,
            RegisteredAt = DateTime.Now,
            PaymentDate = dto.PaymentDate
            
        });
        await context.SaveChangesAsync();
        
        return addedClient.IdClient;
    }
}