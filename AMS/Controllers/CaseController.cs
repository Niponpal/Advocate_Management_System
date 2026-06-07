using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.Controllers;

public class CaseController : Controller
{
    private readonly ICaseRepository _caseRepository;

    public CaseController(ICaseRepository caseRepository)
    {
        _caseRepository = caseRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _caseRepository.GetAllCasesAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }

    [HttpGet]
    public async Task<IActionResult> Details(int id, CancellationToken cancellationToken)
    {
        var data = await _caseRepository.GetCaseByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == 0)
        {
            return View();
        }
        var data = await _caseRepository.GetCaseByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Case caseData, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            if (caseData.Id == 0)
            {
                await _caseRepository.AddCaseAsync(caseData, cancellationToken);
            }
            else
            {
                await _caseRepository.UpdateCaseAsync(caseData, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(caseData);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _caseRepository.DeleteCaseAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}