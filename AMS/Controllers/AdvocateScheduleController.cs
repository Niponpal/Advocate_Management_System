using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class AdvocateScheduleController : Controller
{
    private readonly IAdvocateScheduleRepository _advocateScheduleRepository;
    
    public AdvocateScheduleController(IAdvocateScheduleRepository advocateScheduleRepository)
    {
        _advocateScheduleRepository = advocateScheduleRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data =await _advocateScheduleRepository.GetAllApplicationsAsync(cancellationToken);
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
                return View(new AdvocateSchedule());
            }
            else
            {
                var data = await _advocateScheduleRepository.GetAdvocateByIdAsync(id, cancellationToken);
                if (data != null)
                {
                    return View(data);
                }
                return NotFound();
            }
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(AdvocateSchedule advocateSchedule, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            if (advocateSchedule.Id == 0)
            {
                await _advocateScheduleRepository.AddAdvocateAsync(advocateSchedule, cancellationToken);
            }
            else
            {
                await _advocateScheduleRepository.UpdateAdvocateAsync(advocateSchedule, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(advocateSchedule);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _advocateScheduleRepository.DeleteAdvocateAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]

    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _advocateScheduleRepository.GetAdvocateByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }

}
