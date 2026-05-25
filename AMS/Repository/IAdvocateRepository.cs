using AMS.Data;
using Microsoft.EntityFrameworkCore;
namespace AMS.Repository;

public interface IAdvocateRepository
{
    // CRUD operations for Application entity
    Task<IEnumerable<Advocate>> GetAllApplicationsAsync(CancellationToken cancellationToken);
    Task<Advocate?> GetAdvocateByIdAsync(long id, CancellationToken cancellationToken);
    Task<Advocate> AddAdvocateAsync(Advocate advocate, CancellationToken cancellationToken);
    Task<Advocate?> UpdateAdvocateAsync(Advocate advocate, CancellationToken cancellationToken);
    Task<Advocate> DeleteAdvocateAsync(long id, CancellationToken cancellationToken);

    //Task<bool> IsAlreadyAppliedAsync(long jobId, long userId, CancellationToken cancellationToken);
    //Task<List<Job>> GetAppliedJobsByUserAsync(long userId, CancellationToken cancellationToken);
}

public class AdvocateRepository : IAdvocateRepository
{
    private readonly ApplicationDbContext _context;
    public AdvocateRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Advocate> AddAdvocateAsync(Advocate advocate, CancellationToken cancellationToken)
    {
        await _context.advocates.AddAsync(advocate, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return advocate;
    }

    public async Task<Advocate> DeleteAdvocateAsync(long id, CancellationToken cancellationToken)
    {
       var data =await _context.advocates.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.advocates.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Advocate not found");
        }
    }

    public async Task<Advocate?> GetAdvocateByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.advocates.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Advocate not found");
        }
    }

    public async Task<IEnumerable<Advocate>> GetAllApplicationsAsync(CancellationToken cancellationToken)
    {
      var data = await _context.advocates.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("No advocates found");
        }
    }

    public async Task<Advocate?> UpdateAdvocateAsync(Advocate advocate, CancellationToken cancellationToken)
    {
       var data = await _context.advocates.FindAsync(advocate.Id, cancellationToken);
        if (data != null)
        {
            data.AdvocateName = advocate.AdvocateName;
            data.LicenseNumber = advocate.LicenseNumber;
            data.Specialization = advocate.Specialization;
            data.Email = advocate.Email;
            data.Phone = advocate.Phone;
            data.Address = advocate.Address;
            data.ExperienceYears = advocate.ExperienceYears;
            _context.advocates.Update(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Advocate not found");
        }
    }
}
