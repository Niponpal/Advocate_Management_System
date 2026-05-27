using AMS.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IInvoiceRepository
{
    // CRUD operations for Invoice entity
    Task<IEnumerable<Invoice>> GetAllInvoicesAsync(CancellationToken cancellationToken);
    Task<Invoice?> GetInvoiceByIdAsync(long id, CancellationToken cancellationToken);
    Task<Invoice> AddInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);
    Task<Invoice?> UpdateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken);
    Task<Invoice> DeleteInvoiceAsync(long id, CancellationToken cancellationToken);
}

public class InvoiceRepository : IInvoiceRepository
{
    private readonly ApplicationDbContext _context;
    public InvoiceRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Invoice> AddInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        await _context.invoices.AddAsync(invoice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return invoice;
    }
    public async Task<Invoice> DeleteInvoiceAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.invoices.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.invoices.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Invoice not found");
        }
    }
    public async Task<Invoice?> GetInvoiceByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.invoices.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Invoice not found");
        }
    }
    public async Task<IEnumerable<Invoice>> GetAllInvoicesAsync(CancellationToken cancellationToken)
    {
        return await _context.invoices.ToListAsync(cancellationToken);
    }
    public async Task<Invoice?> UpdateInvoiceAsync(Invoice invoice, CancellationToken cancellationToken)
    {
        var data = await _context.invoices.FindAsync(invoice.Id, cancellationToken);
        if (data != null)
        {
            data.InvoiceNumber = invoice.InvoiceNumber;
            data.TotalAmount = invoice.TotalAmount;
            data.InvoiceDate = invoice.InvoiceDate;
            data.ClientId = invoice.ClientId;
            data.Id = invoice.Id;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Invoice not found");
        }
    }
}