using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class CaseController : Controller
{
    private readonly ICaseRepository _caseRepository;
    private readonly IClientRepository _clientRepository;   


    public CaseController(ICaseRepository caseRepository, IClientRepository clientRepository)
    {
        _caseRepository = caseRepository;
        _clientRepository = clientRepository;
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
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        ViewData["ClientId"] = _clientRepository.Dropdown();
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
            ViewData["ClientId"] = _clientRepository.Dropdown();
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
}