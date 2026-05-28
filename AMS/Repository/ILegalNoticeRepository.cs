using AMS.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface ILegalNoticeRepository
{ 
    // CRUD operations for Legal Notice entity
    Task<IEnumerable<LegalNotice>> GetAllLegalNoticeAsync(CancellationToken cancellationToken);
    Task<LegalNotice?> GetLegalNoticeByIdAsync(long id, CancellationToken cancellationToken);
    Task<LegalNotice> AddLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken);
    Task<LegalNotice?> UpdateLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken);
    Task<LegalNotice> DeleteLegalNoticeAsync(long id, CancellationToken cancellationToken);
}

public class LegalNoticeRepository : ILegalNoticeRepository
{
    private readonly ApplicationDbContext _context;
    public LegalNoticeRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<LegalNotice?> UpdateLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken)
    {
        var data = await _context.legalNotices.FindAsync(legalNotice.Id, cancellationToken);
        if (data != null)
        {
           data.NoticeTitle = legalNotice.NoticeTitle;
              data.Description = legalNotice.Description;
                data.NoticeDate = legalNotice.NoticeDate;
                data.ClientId = legalNotice.ClientId;
            data.Client = legalNotice.Client;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Legal Notice not found");
        }
    }
    async Task<LegalNotice> ILegalNoticeRepository.AddLegalNoticeAsync(LegalNotice legalNotice, CancellationToken cancellationToken)
    {
      var data = await _context.legalNotices.AddAsync(legalNotice, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return legalNotice;
    }
    async Task<LegalNotice> ILegalNoticeRepository.DeleteLegalNoticeAsync(long id, CancellationToken cancellationToken)
    {
        var data = await _context.legalNotices.FindAsync(id, cancellationToken);
        if (data != null)
        {
            _context.legalNotices.Remove(data);
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        else
        {
            throw new Exception("Legal Notice not found");
        }
    }
    async Task<LegalNotice?> ILegalNoticeRepository.GetLegalNoticeByIdAsync(long id, CancellationToken cancellationToken)
    {
         var data = await _context.legalNotices.FindAsync(id, cancellationToken);
         if (data != null)
         {
             return data;
         }
         else
         {
             throw new Exception("Legal Notice not found");
         }
    }
    async Task<IEnumerable<LegalNotice>> ILegalNoticeRepository.GetAllLegalNoticeAsync(CancellationToken cancellationToken)
    {
         return await _context.legalNotices.ToListAsync(cancellationToken);
    }
}