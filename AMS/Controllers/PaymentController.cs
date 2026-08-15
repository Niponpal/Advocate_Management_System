using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentRepository _paymentRepository;
    private readonly IClientRepository _clientRepository;
   private readonly ICaseRepository _caseRepository;

    public PaymentController(IPaymentRepository paymentRepository, IClientRepository clientRepository, ICaseRepository caseRepository)
    {
        _paymentRepository = paymentRepository;
        _clientRepository = clientRepository;
        _caseRepository = caseRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var payments = await _paymentRepository.GetAllPaymentsAsync(cancellationToken);
        if (payments != null)
        {
            return View(payments);
          
        }
        return NotFound();
    }
  
    [HttpGet]
    public  async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        ViewData["ClientId"] = _clientRepository.Dropdown();
        ViewData["CaseId"] = _caseRepository.Dropdown();

        if (id == 0)
        {
            return View(new Payment());
        }
        else
        {
            var payment = await _paymentRepository.GetPaymentByIdAsync(id, cancellationToken);
            if (payment != null)
            {
                return View(payment);
            }
            return NotFound();
        }
    }
     [HttpPost]
     public async Task<IActionResult> CreateOrEdit(Payment payment, CancellationToken cancellationToken)
    {
        ViewData["ClientId"] = _clientRepository.Dropdown();
        ViewData["CaseId"] = _caseRepository.Dropdown();
        if (payment.Id == 0)
            {
                await _paymentRepository.AddPaymentAsync(payment, cancellationToken);
                return RedirectToAction(nameof(Index));
        }
            else
            {
                await _paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
               return RedirectToAction(nameof(Index));
            }   
        
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _paymentRepository.DeletePaymentAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]

    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var payment = await _paymentRepository.GetPaymentByIdAsync(id, cancellationToken);
        if (payment != null)
        {
            return View(payment);
        }
        return NotFound();
    }
}
