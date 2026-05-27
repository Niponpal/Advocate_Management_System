using AMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.Controllers;

public class HearingController : Controller
{
    private readonly IHearingRepository hearingRepository;

    public HearingController(IHearingRepository _hearingRepository)
    {
        hearingRepository = _hearingRepository;
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
