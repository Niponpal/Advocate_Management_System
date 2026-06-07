using AMS.Data;
using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface ICaseRepository
{
    Task<IEnumerable<Case>> GetAllCasesAsync(CancellationToken cancellationToken);
    Task<Case?> GetCaseByIdAsync(long id, CancellationToken cancellationToken);
    Task<Case> AddCaseAsync(Case cases, CancellationToken cancellationToken);
    Task<Case?> UpdateCaseAsync(Case cases, CancellationToken cancellationToken);
    Task<Case> DeleteCaseAsync (long id, CancellationToken cancellationToken);
}

public class CaseRepository : ICaseRepository
{
    private readonly ApplicationDbContext _context;
    public CaseRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Case> AddCaseAsync(Case cases, CancellationToken cancellationToken)
    {
        await _context.cases.AddAsync(cases, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cases;
    }
    public async Task<Case> DeleteCaseAsync(long id, CancellationToken cancellationToken)
    {
       var data =await _context.cases.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.cases.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Case not found");
        }
    }
    public async Task<Case?> GetCaseByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.cases.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Case not found");
        }
    }
    public async Task<IEnumerable<Case>> GetAllCasesAsync(CancellationToken cancellationToken)
    {
        var data = await _context.cases.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("No cases found");
        }
    }
    public async Task<Case?> UpdateCaseAsync(Case cases, CancellationToken cancellationToken)
    {
       var data = await _context.cases.FindAsync(cases.Id, cancellationToken);
       if (data != null)
       {
           data.CaseNumber = cases.CaseNumber;
           data.CaseTitle = cases.CaseTitle;
           data.CaseType = cases.CaseType;
           data.Description = cases.Description;
           data.FilingDate = cases.FilingDate;
           data.Status = cases.Status;
           data.AdvocateId = cases.AdvocateId;
           data.ClientId = cases.ClientId;
           await _context.SaveChangesAsync(cancellationToken);
           return data;
       }
       else
       {
           throw new Exception("Case not found");
       }
    }
}
