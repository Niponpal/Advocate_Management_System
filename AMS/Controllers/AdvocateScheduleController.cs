using AMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
}
