using AMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.Controllers;

public class LegalNoticeController : Controller
{
    private readonly ILegalNoticeRepository _legalNoticeRepository;

    public LegalNoticeController(ILegalNoticeRepository legalNoticeRepository)
    {
        _legalNoticeRepository = legalNoticeRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _legalNoticeRepository.GetAllLegalNoticeAsync(cancellationToken);
        return View();
    }
}
