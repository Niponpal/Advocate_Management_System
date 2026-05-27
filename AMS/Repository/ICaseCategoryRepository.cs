using AMS.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface ICaseCategoryRepository
{
    Task<IEnumerable<CaseCategory>> GetAllCaseCategoryAsync(CancellationToken cancellationToken);
    Task<CaseCategory?> GetCaseCategoryByIdAsync(long id, CancellationToken cancellationToken);
    Task<CaseCategory> AddCaseCategoryAsync(CaseCategory cases, CancellationToken cancellationToken);
    Task<CaseCategory?> UpdateCaseCategoryAsync(CaseCategory cases, CancellationToken cancellationToken);
    Task<CaseCategory> DeleteCaseCategoryAsync(long id, CancellationToken cancellationToken);
}

public class CaseCategoryRepository: ICaseCategoryRepository
{
    private readonly ApplicationDbContext _context;
    public CaseCategoryRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<CaseCategory> AddCaseCategoryAsync(CaseCategory cases, CancellationToken cancellationToken)
    {
        await _context.caseCategories.AddAsync(cases, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return cases;
    }
    public async Task<CaseCategory> DeleteCaseCategoryAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.caseCategories.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.caseCategories.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Case Category not found");
        }
    }
    public async Task<CaseCategory?> GetCaseCategoryByIdAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.caseCategories.FindAsync(id, cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("Case Category not found");
        }
    }
    public async Task<IEnumerable<CaseCategory>> GetAllCaseCategoryAsync(CancellationToken cancellationToken)
    {
        var data = await _context.caseCategories.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        else
        {
            throw new Exception("No Case Category found");
        }
    }

    public async Task<CaseCategory?> UpdateCaseCategoryAsync(CaseCategory cases, CancellationToken cancellationToken)
    {
       var data = await _context.caseCategories.FindAsync(cases.Id, cancellationToken);
        if (data != null)
        {
            data.CategoryName = cases.CategoryName;
            data.IsActive = cases.IsActive;
            _context.caseCategories.Update(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Case Category not found");
        }
    }
}
