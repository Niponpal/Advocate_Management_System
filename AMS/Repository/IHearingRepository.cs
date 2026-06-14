using AMS.Data;
using AMS.Models.Auth;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IHearingRepository
{
    // CRUD operations for Application entity
    Task<IEnumerable<Hearing>> GetAllHearingAsync(CancellationToken cancellationToken);
    Task<Hearing> GetHearingByIdAsync(long id, CancellationToken cancellationToken);
    Task<Hearing> AddHearingAsync(Hearing hearing, CancellationToken cancellationToken);
    Task<Hearing?> UpdateHearingAsync(Hearing hearing, CancellationToken cancellationToken);
    Task<Hearing> DeleteHearingAsync(long id, CancellationToken cancellationToken);
}

public class HearingRepository : IHearingRepository
{
    private readonly ApplicationDbContext _context;
    public HearingRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Hearing> AddHearingAsync(Hearing hearing, CancellationToken cancellationToken)
    {
        await _context.hearings.AddAsync(hearing, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return hearing;
    }
    public async Task<Hearing> DeleteHearingAsync(long id, CancellationToken cancellationToken)
    {
       var data =await _context.hearings.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.hearings.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Hearing not found");
        }
    }
    public async Task<Hearing> GetHearingByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.hearings.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Hearing not found");
        }
    }
    public async Task<IEnumerable<Hearing>> GetAllHearingAsync(CancellationToken cancellationToken)
    {
        return await _context.hearings.ToListAsync(cancellationToken);
    }
    public async Task<Hearing?> UpdateHearingAsync(Hearing hearing, CancellationToken cancellationToken)
    {
         var data = await _context.hearings.FindAsync(hearing.Id, cancellationToken);
         if (data != null)
         {
             data.HearingDate = hearing.HearingDate;
             data.Remarks = hearing.Remarks;
             data.HearingStatus = hearing.HearingStatus;
             data.CaseId = hearing.CaseId;
             data.CourtId = hearing.CourtId;
             await _context.SaveChangesAsync(cancellationToken);
             return data;
         }
         else
         {
             throw new Exception("Hearing not found");
         }
    }
}