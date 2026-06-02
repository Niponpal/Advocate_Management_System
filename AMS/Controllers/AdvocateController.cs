using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;
namespace AMS.Controllers;

public class AdvocateController : Controller
{
    private readonly IAdvocateRepository _advocateRepository;

    public AdvocateController(IAdvocateRepository advocateRepository)
    {
        _advocateRepository = advocateRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data =await _advocateRepository.GetAllApplicationsAsync(cancellationToken);
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
            return View(new Advocate());
        }
        else
        {
            var data = await _advocateRepository.GetAdvocateByIdAsync(id, cancellationToken);
            if (data != null)
            {
                return View(data);
            }
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Advocate advocate, CancellationToken cancellationToken)
    {
      
            if (advocate.Id == 0)
            {
                await _advocateRepository.AddAdvocateAsync(advocate, cancellationToken);
            }
            else
            {
                await _advocateRepository.UpdateAdvocateAsync(advocate, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        
      
    }

    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _advocateRepository.GetAdvocateByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _advocateRepository.DeleteAdvocateAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

}
