using CW10.Data;
using CW10.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace CW10.Services;

public interface IClientsService
{
    public Task<int> DeleteClient(int idClient);
}

public class ClientsService(MasterContext context) : IClientsService
{
    public async Task<int> DeleteClient(int idClient)
    {
        var client = context.Clients.Include(client => client.ClientTrips).FirstOrDefault(c => c.IdClient == idClient);

        if (client == null)
        {
            throw new NotFoundException("Client not found");
        }

        if (client.ClientTrips.Count > 0)
        {
            throw new ClientException("Client has trips and cannot be deleted");
        }

        var affectedRows = await context.Clients.Where(c => c.IdClient == idClient).ExecuteDeleteAsync();

        if (affectedRows == 0)
        {
            throw new NotFoundException("Client deletion failed");
        }

        return client.IdClient;
    }
}