using AMS.Data;
using AMS.Models;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface ILegalNoticeRepository
{
    Task<IEnumerable<LegalNotice>> GetAllLegalNoticeAsync(CancellationToken cancellationToken);
    Task<LegalNotice?> GetLegalNoticeByIdAsync(long id, CancellationToken cancellationToken);
    Task<LegalNotice> AddLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken);
    Task<LegalNotice?> UpdateLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken);
    Task<bool> DeleteLegalNoticeAsync(long id, CancellationToken cancellationToken);
}


public class LegalNoticeRepository : ILegalNoticeRepository
{
    private readonly ApplicationDbContext _context;

    public LegalNoticeRepository(ApplicationDbContext context)
    {
        _context = context;
    }

    // GET ALL
    public async Task<IEnumerable<LegalNotice>> GetAllLegalNoticeAsync(CancellationToken cancellationToken)
    {
        return await _context.legalNotices
            .AsNoTracking()
            .ToListAsync(cancellationToken);
    }

    // GET BY ID
    public async Task<LegalNotice?> GetLegalNoticeByIdAsync(long id, CancellationToken cancellationToken)
    {
        return await _context.legalNotices
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);
    }

    // ADD
    public async Task<LegalNotice> AddLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken)
    {
        await _context.legalNotices.AddAsync(legalNotice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return legalNotice;
    }

    // UPDATE
    public async Task<LegalNotice?> UpdateLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken)
    {
        var data = await _context.legalNotices
            .FirstOrDefaultAsync(x => x.Id == legalNotice.Id, cancellationToken);

        if (data == null)
            return null;

        data.NoticeTitle = legalNotice.NoticeTitle;
        data.Description = legalNotice.Description;
        data.NoticeDate = legalNotice.NoticeDate;
        data.ClientId = legalNotice.ClientId;

        // ❌ Avoid navigation update (prevents EF tracking issues)
        // data.Client = legalNotice.Client;

        await _context.SaveChangesAsync(cancellationToken);

        return data;
    }

    // DELETE
    public async Task<bool> DeleteLegalNoticeAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.legalNotices
            .FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

        if (data == null)
            return false;

        _context.legalNotices.Remove(data);
        await _context.SaveChangesAsync(cancellationToken);

        return true;
    }
}