using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class HearingController : Controller
{
    private readonly IHearingRepository hearingRepository;
    private readonly ICaseRepository caseRepository;

    public HearingController(IHearingRepository _hearingRepository, ICaseRepository _caseRepository)
    {
        hearingRepository = _hearingRepository;
        caseRepository = _caseRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await hearingRepository.GetAllHearingAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }   
        return View();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        ViewData["CaseId"] = caseRepository.Dropdown();
        if (id == 0)
        {
            return View(new Hearing());
        }
        var data = await hearingRepository.GetHearingByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Hearing hearing, CancellationToken cancellationToken)
    {
        ViewData["CaseId"] = caseRepository.Dropdown();
        if (hearing.Id == 0)
            {
                await hearingRepository.AddHearingAsync(hearing, cancellationToken);
            }
            else
            {
                await hearingRepository.UpdateHearingAsync(hearing, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await hearingRepository.DeleteHearingAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await hearingRepository.GetHearingByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
}
