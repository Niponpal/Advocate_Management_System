using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class CourtController : Controller
{
    private readonly ICourtRepository _courtRepository;
    public CourtController(ICourtRepository courtRepository)
    {
        _courtRepository = courtRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _courtRepository.GetAllCourtAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult>CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == null)
        {
            return View(new Court());
        }
        var data = await _courtRepository.GetCourtByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Court court, CancellationToken cancellationToken)
    {
            if (court.Id == 0)
            {
                await _courtRepository.AddCourtAsync(court, cancellationToken);
            }
            else
            {
                await _courtRepository.UpdateCourtAsync(court, cancellationToken);
            }
            return RedirectToAction(nameof(Index));

    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _courtRepository.DeleteCourtAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _courtRepository.GetCourtByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
}
