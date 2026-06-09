using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class ClientController : Controller
{
    private readonly IClientRepository _clientRepository;
    public ClientController(IClientRepository clientRepository)
    {
        _clientRepository = clientRepository;
    }
    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _clientRepository.GetAllApplicationsAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == null || id == 0)
        {
            return View(new Client());
        }

        var data = await _clientRepository.GetClientByIdAsync(id, cancellationToken);

        if (data == null)
        {
            return NotFound();
        }

        return View(data);
    }

    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(Client model, CancellationToken cancellationToken)
    {
            if (model.Id == 0)
            {
                await _clientRepository.AddClientAsync(model, cancellationToken);
            }
            else
            {
                await _clientRepository.UpdateClientAsync(model, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _clientRepository.GetClientByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _clientRepository.DeleteClientAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
 }
