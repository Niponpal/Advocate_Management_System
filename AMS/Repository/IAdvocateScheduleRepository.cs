using AMS.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IAdvocateScheduleRepository
{ 
    // CRUD operations for Application entity
    Task<IEnumerable<AdvocateSchedule>> GetAllApplicationsAsync(CancellationToken cancellationToken);
    Task<AdvocateSchedule?> GetAdvocateByIdAsync(long id, CancellationToken cancellationToken);
    Task<AdvocateSchedule> AddAdvocateAsync(AdvocateSchedule advocateSchedule, CancellationToken cancellationToken);
    Task<AdvocateSchedule?> UpdateAdvocateAsync(AdvocateSchedule advocateSchedule, CancellationToken cancellationToken);
    Task<AdvocateSchedule> DeleteAdvocateAsync(long id, CancellationToken cancellationToken);

}

public class AdvocateScheduleRepository : IAdvocateScheduleRepository
{
    private readonly ApplicationDbContext _context;
    public AdvocateScheduleRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<AdvocateSchedule?> UpdateAdvocateAsync(AdvocateSchedule advocateSchedule, CancellationToken cancellationToken)
    {
        var data = await _context.advocateSchedules.FindAsync(advocateSchedule.Id, cancellationToken);
        if (data != null)
        {
            data.ScheduleDate = advocateSchedule.ScheduleDate;
            data.AvailableTime = advocateSchedule.AvailableTime;
            data.AdvocateId = advocateSchedule.AdvocateId;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Advocate Schedule not found");
        }
    }

    async Task<AdvocateSchedule> IAdvocateScheduleRepository.AddAdvocateAsync(AdvocateSchedule advocateSchedule, CancellationToken cancellationToken)
    {
      var data = await _context.advocateSchedules.AddAsync(advocateSchedule, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return advocateSchedule;
    }

    async Task<AdvocateSchedule> IAdvocateScheduleRepository.DeleteAdvocateAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.advocateSchedules.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.advocateSchedules.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Advocate Schedule not found");
        }
    }

    async Task<AdvocateSchedule?> IAdvocateScheduleRepository.GetAdvocateByIdAsync(long id, CancellationToken cancellationToken)
    {
       var data = await _context.advocateSchedules.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Advocate Schedule not found");
        }
    }

    async Task<IEnumerable<AdvocateSchedule>> IAdvocateScheduleRepository.GetAllApplicationsAsync(CancellationToken cancellationToken)
    {
      var data = await _context.advocateSchedules.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("No advocate schedules found");
        }
    }
}
