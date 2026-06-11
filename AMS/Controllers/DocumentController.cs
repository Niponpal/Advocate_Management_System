using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class DocumentController : Controller
{
    private readonly IDocumentRepository  documentRepository;

    public DocumentController(IDocumentRepository _documentRepository)
    {
        documentRepository = _documentRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await documentRepository.GetAllDocumentsAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == null)
        {
            return View(new Document());
        }
        var data = await documentRepository.GetDocumentByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Document document, CancellationToken cancellationToken)
    {
            if (document.Id == 0)
            {
                await documentRepository.AddDocumentAsync(document, cancellationToken);
            }
            else
            {
                await documentRepository.UpdateDocumentAsync(document, cancellationToken);
            }
            return RedirectToAction(nameof(Index));

    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await documentRepository.DeleteDocumentAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await documentRepository.GetDocumentByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
}
