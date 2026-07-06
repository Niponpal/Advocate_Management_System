using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAdvocateRepository _advocateRepository;

    public AppointmentController(IAppointmentRepository appointmentRepository, IAdvocateRepository advocateRepository)
    {
        _appointmentRepository = appointmentRepository;
        _advocateRepository = advocateRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _appointmentRepository.GetAllAppointmentAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _appointmentRepository.GetAppointmentByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        ViewData["AdvocateId"] = _advocateRepository.Dropdown();
        if (id == 0)
        {
            return View(new Appointment());
        }
        else
        {
            var data = await _appointmentRepository.GetAppointmentByIdAsync(id, cancellationToken);
            if (data != null)
            {
                return View(data);
            }
            return NotFound();
        }
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Appointment appointment, CancellationToken cancellationToken)
    {
        ViewData["AdvocateId"] = _advocateRepository.Dropdown();
        if (ModelState.IsValid)
        {
            if (appointment.Id == 0)
            {
                await _appointmentRepository.AddAppointmentAsync(appointment, cancellationToken);
            }
            else
            {
                await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(appointment);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _appointmentRepository.DeleteAppointmentAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
