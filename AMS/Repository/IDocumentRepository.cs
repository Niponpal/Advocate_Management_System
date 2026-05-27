using AMS.Data;
using Microsoft.EntityFrameworkCore;

namespace AMS.Repository;

public interface IDocumentRepository
{
    Task<IEnumerable<Document>> GetAllDocumentsAsync(CancellationToken cancellationToken);
    Task<Document?> GetDocumentByIdAsync(long id, CancellationToken cancellationToken);
    Task<Document> AddDocumentAsync(Document document, CancellationToken cancellationToken);
    Task<Document?> UpdateDocumentAsync(Document document, CancellationToken cancellationToken);
    Task<Document> DeleteDocumentAsync(long id, CancellationToken cancellationToken);
}

public class DocumentRepository : IDocumentRepository
{
    private readonly ApplicationDbContext _context;
    public DocumentRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Document> AddDocumentAsync(Document document, CancellationToken cancellationToken)
    {
        await _context.documents.AddAsync(document, cancellationToken);
        await _context.SaveChangesAsync(cancellationToken);
        return document;
    }

    public async Task<Document> DeleteDocumentAsync(long id, CancellationToken cancellationToken)
    {
        var document = await _context.documents.FindAsync( id, cancellationToken);
        if (document != null)
        {
            _context.documents.Remove(document);
            await _context.SaveChangesAsync(cancellationToken);
            return document;
        }
        throw new KeyNotFoundException("Document not found");
    }

    public async Task<IEnumerable<Document>> GetAllDocumentsAsync(CancellationToken cancellationToken)
    {
       var data = await _context.documents.ToListAsync(cancellationToken);
        if (data != null)
        {
            return data;
        }
        throw new KeyNotFoundException("No documents found");
    }

    public async Task<Document?> GetDocumentByIdAsync(long id, CancellationToken cancellationToken)
    {
      var document = await _context.documents.FindAsync(id, cancellationToken);
        if (document != null)
        {
            return document;
        }
        throw new KeyNotFoundException("Document not found");
    }

    public async Task<Document?> UpdateDocumentAsync(Document document, CancellationToken cancellationToken)
    {
       var data = await _context.documents.FindAsync(document.Id, cancellationToken);
        if (data != null)
        {
            data.DocumentTitle = document.DocumentTitle;
            data.FilePath = document.FilePath;
            data.DocumentType = document.DocumentType;
            data.UploadDate = document.UploadDate;
            data.CaseId = document.CaseId;
            await _context.SaveChangesAsync(cancellationToken);
            return data;
        }
        throw new KeyNotFoundException("Document not found");
    }
}
