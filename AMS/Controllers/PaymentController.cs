using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class PaymentController : Controller
{
    private readonly IPaymentRepository _paymentRepository;

    public PaymentController(IPaymentRepository paymentRepository)
    {
        _paymentRepository = paymentRepository;
    }
    public IActionResult Index(CancellationToken cancellationToken)
    {
        var payments = _paymentRepository.GetAllPaymentsAsync(cancellationToken);
        if (payments != null)
        {
            return View(payments);
          
        }
        return NotFound();
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
    [HttpGet]

    public  async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
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
        if (ModelState.IsValid)
        {
            if (payment.Id == 0)
            {
                await _paymentRepository.AddPaymentAsync(payment, cancellationToken);
            }
            else
            {
                await _paymentRepository.UpdatePaymentAsync(payment, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(payment);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _paymentRepository.DeletePaymentAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
}
