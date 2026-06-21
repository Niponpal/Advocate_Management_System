using AMS.Data;
using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IPaymentRepository
{
    // CRUD operations for Payment entity
    Task<IEnumerable<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken);
    Task<Payment?> GetPaymentByIdAsync(long id, CancellationToken cancellationToken);
    Task<Payment> AddPaymentAsync(Payment payment, CancellationToken cancellationToken);
    Task<Payment?> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken);
    Task<Payment> DeletePaymentAsync(long id, CancellationToken cancellationToken);
}

public class PaymentRepository : IPaymentRepository
{
    private readonly ApplicationDbContext _context;
    public PaymentRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Payment>> GetAllPaymentsAsync(CancellationToken cancellationToken)
    {
       var data = await _context.payments.ToListAsync(cancellationToken);
        return data;
    }

    public async Task<Payment?> GetPaymentByIdAsync(long id, CancellationToken cancellationToken)
    {
       var data = await _context.payments.FindAsync(id, cancellationToken);

        return data;
    }

    public async Task<Payment?> UpdatePaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        var data = await _context.payments.FindAsync(payment.Id, cancellationToken);
        if (data != null)
        {
            data.Amount = payment.Amount;
            data.PaymentDate = payment.PaymentDate;
            data.PaymentMethod = payment.PaymentMethod;
            data.ClientId = payment.ClientId;
            data.Client = payment.Client;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Payment not found");
        }
    }
    async Task<Payment> IPaymentRepository.AddPaymentAsync(Payment payment, CancellationToken cancellationToken)
    {
        var data = await _context.payments.AddAsync(payment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return payment;
    }
    async Task<Payment> IPaymentRepository.DeletePaymentAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.payments.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.payments.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Payment not found");
        }
    }
}