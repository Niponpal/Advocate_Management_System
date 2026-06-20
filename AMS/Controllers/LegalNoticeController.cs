using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class LegalNoticeController : Controller
{
    private readonly ILegalNoticeRepository _legalNoticeRepository;

    public LegalNoticeController(ILegalNoticeRepository legalNoticeRepository)
    {
        _legalNoticeRepository = legalNoticeRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _legalNoticeRepository.GetAllLegalNoticeAsync(cancellationToken);
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == 0)
        {
            return View(new LegalNotice());
        }

        var notice = await _legalNoticeRepository.GetLegalNoticeByIdAsync(id, cancellationToken);

        if (notice != null)
        {
            return View(notice);
        }

        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(LegalNotice legalNotice, CancellationToken cancellationToken)
    {
        if (legalNotice.Id == 0)
        {
            await _legalNoticeRepository.AddLegalNoticeAsync(legalNotice,cancellationToken );
        }
        else
        {
            await _legalNoticeRepository.UpdateLegalNoticeAsync(legalNotice, cancellationToken);
        }
        return RedirectToAction("Index");
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _legalNoticeRepository.GetLegalNoticeByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _legalNoticeRepository.DeleteLegalNoticeAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
