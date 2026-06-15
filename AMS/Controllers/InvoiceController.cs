using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class InvoiceController : Controller
{
    private readonly IInvoiceRepository _invoiceRepository;

    public InvoiceController(IInvoiceRepository invoiceRepository)
    {
        _invoiceRepository = invoiceRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data =await _invoiceRepository.GetAllInvoicesAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }

        return View();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == 0)
        {
            return View(new Invoice());
        }
        var data = await _invoiceRepository.GetInvoiceByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Invoice invoice, CancellationToken cancellationToken)
    {
            if (invoice.Id == 0)
            {
                await _invoiceRepository.AddInvoiceAsync(invoice, cancellationToken);
            }
            else
            {
                await _invoiceRepository.UpdateInvoiceAsync(invoice, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _invoiceRepository.DeleteInvoiceAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _invoiceRepository.GetInvoiceByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
}
