using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class AdvocateController1 : Controller
{
    private readonly IAdvocateRepository _advocateRepository;

    public AdvocateController1(IAdvocateRepository advocateRepository)
    {
        _advocateRepository = advocateRepository;
    }
    public IActionResult Index()
    {
        return View();
    }
}
