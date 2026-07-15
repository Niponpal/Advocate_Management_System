using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class AppointmentController : Controller
{
    private readonly IAppointmentRepository _appointmentRepository;
    private readonly IAdvocateRepository _advocateRepository;
    private readonly IClientRepository _clientRepository;

    public AppointmentController(IAppointmentRepository appointmentRepository, IAdvocateRepository advocateRepository,IClientRepository clientRepository)
    {
        _appointmentRepository = appointmentRepository;
        _advocateRepository = advocateRepository;
        _clientRepository = clientRepository;
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
        ViewData["ClientId"] = _clientRepository.Dropdown();
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
        ViewData["ClientId"] = _clientRepository.Dropdown();
      
            if (appointment.Id == 0)
            {
                await _appointmentRepository.AddAppointmentAsync(appointment, cancellationToken);
               return RedirectToAction(nameof(Index));
            }
            else
            {
                await _appointmentRepository.UpdateAppointmentAsync(appointment, cancellationToken);
                return RedirectToAction(nameof(Index));
            }
           
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _appointmentRepository.DeleteAppointmentAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
