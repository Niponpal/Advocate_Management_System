using AMS.Data;
using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IAppointmentRepository
{  // CRUD operations for Appointment entity
    Task<IEnumerable<Appointment>> GetAllAppointmentAsync(CancellationToken cancellationToken);
    Task<Appointment> GetAppointmentByIdAsync(long id, CancellationToken cancellationToken);
    Task<Appointment> AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
    Task<Appointment> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken);
    Task<Appointment> DeleteAppointmentAsync(long id, CancellationToken cancellationToken);
}

public class AppointmentRepository : IAppointmentRepository
{
    private readonly ApplicationDbContext _context;
    public AppointmentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Appointment> AddAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
    {
        await _context.appointments.AddAsync(appointment, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return appointment;
    }
    public async Task<Appointment> DeleteAppointmentAsync(long id, CancellationToken cancellationToken)
    {
       var data =await _context.appointments.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.appointments.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Appointment not found");
        }
    }
    public async Task<Appointment?> GetAppointmentByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.appointments.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Appointment not found");
        }
    }
    public async Task<IEnumerable<Appointment>> GetAllAppointmentAsync(CancellationToken cancellationToken)
    {
        var data = await _context.appointments.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("No appointments found");
        } 
    }
    public async Task<Appointment?> UpdateAppointmentAsync(Appointment appointment, CancellationToken cancellationToken)
    {
       var data =await _context.appointments.FindAsync(appointment.Id, cancellationToken);
       if (data != null)
       {
           data.AppointmentDate = appointment.AppointmentDate;
           data.Purpose = appointment.Purpose;
           data.Status = appointment.Status;
           data.ClientId = appointment.ClientId;
           data.AdvocateId = appointment.AdvocateId;
           await _context.SaveChangesAsync(cancellationToken);
           return data;
       }
       else
       {
           throw new Exception("Appointment not found");
       }
    }
}