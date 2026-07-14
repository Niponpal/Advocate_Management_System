using AMS.Data;
using AMS.Models;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IClientRepository
{ // CRUD operations for Application entity
    Task<IEnumerable<Client>> GetAllApplicationsAsync(CancellationToken cancellationToken);
    Task<Client?> GetClientByIdAsync(long id, CancellationToken cancellationToken);
    Task<Client> AddClientAsync(Client client, CancellationToken cancellationToken);
    Task<Client?> UpdateClientAsync(Client client, CancellationToken cancellationToken);
    Task<Client> DeleteClientAsync(long id, CancellationToken cancellationToken);
    IEnumerable<SelectListItem> Dropdown();
}

public class ClientRepository : IClientRepository
{
    private readonly ApplicationDbContext _context;
    public ClientRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Client?> UpdateClientAsync(Client client, CancellationToken cancellationToken)
    {
        var data = await _context.clients.FindAsync(client.Id, cancellationToken);
        if (data != null)
        {
            data.ClientName = client.ClientName;
            data.Email = client.Email;
            data.Phone = client.Phone;
            data.Address = client.Address;
            data.NIDNumber = client.NIDNumber;
            data.Cases = client.Cases;
            data.Payments = client.Payments;
            data.Appointments = client.Appointments;
            data.Invoices = client.Invoices;
            data.LegalNotices = client.LegalNotices;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Client not found");
        }
    }
    async Task<Client> IClientRepository.AddClientAsync(Client client, CancellationToken cancellationToken)
    {
      var data = await _context.clients.AddAsync(client, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return client;
    }
    async Task<Client> IClientRepository.DeleteClientAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.clients.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.clients.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Client not found");
        }
    }
    public async Task<Client?> GetClientByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.clients.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Client not found");
        }
    }
    public async Task<IEnumerable<Client>> GetAllApplicationsAsync(CancellationToken cancellationToken)
    {
       return await _context.clients.ToListAsync(cancellationToken);
    }

    public IEnumerable<SelectListItem> Dropdown()
    {
        var data = _context.clients.Select(x => new SelectListItem
        {
            Text = x.ClientName,
            Value = x.Id.ToString()
        }).ToList();
        return data;
    }
}