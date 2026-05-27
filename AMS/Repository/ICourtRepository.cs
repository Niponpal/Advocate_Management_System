using AMS.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface ICourtRepository
{
    Task<IEnumerable<Court>> GetAllCourtAsync(CancellationToken cancellationToken);
    Task<Court?> GetCourtByIdAsync(long id, CancellationToken cancellationToken);
    Task<Court> AddCourtAsync(Court court, CancellationToken cancellationToken);
    Task<Court?> UpdateCourtAsync(Court court, CancellationToken cancellationToken);
    Task<Court> DeleteCourtAsync(long id, CancellationToken cancellationToken);
}

public class CourtRepository : ICourtRepository
{
    private readonly ApplicationDbContext _context;
    public CourtRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Court>> GetAllCourtAsync(CancellationToken cancellationToken)
    {
       var data = await _context.courts.ToListAsync(cancellationToken);
        return data;
    }

    public async Task<Court?> GetCourtByIdAsync(long id, CancellationToken cancellationToken)
    {
       var data = await _context.courts.FindAsync(id, cancellationToken);
        return data;
    }

    public async Task<Court?> UpdateCourtAsync(Court court, CancellationToken cancellationToken)
    {
        var data = await _context.courts.FindAsync(court.Id, cancellationToken);
        if (data != null)
        {
            data.CourtName = court.CourtName;
            data.Location = court.Location;
            data.CourtType = court.CourtType;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Court not found");
        }
    }
    async Task<Court> ICourtRepository.AddCourtAsync(Court court, CancellationToken cancellationToken)
    {
      var data = await _context.courts.AddAsync(court, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return court;
    }
    async Task<Court> ICourtRepository.DeleteCourtAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.courts.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.courts.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Court not found");
        }
    }
}