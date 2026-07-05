using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class AdvocateScheduleController : Controller
{
    private readonly IAdvocateScheduleRepository _advocateScheduleRepository;
    private readonly IAdvocateRepository _advocateRepository;


    public AdvocateScheduleController(IAdvocateScheduleRepository advocateScheduleRepository, IAdvocateRepository advocateRepository)
    {
        _advocateScheduleRepository = advocateScheduleRepository;
        _advocateRepository = advocateRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken = default)
    {
        var schedules = await _advocateScheduleRepository.GetAllApplicationsAsync(cancellationToken);

        // Better approach: Never return NotFound for list page, return empty list instead
        if (schedules == null)
        {
            schedules = new List<AdvocateSchedule>();
        }

        return View(schedules);
    }
    [HttpGet]
        public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
        {
        ViewData["AdvocateId"] = _advocateRepository.Dropdown();
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
        ViewData["AdvocateId"] = _advocateRepository.Dropdown();
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
