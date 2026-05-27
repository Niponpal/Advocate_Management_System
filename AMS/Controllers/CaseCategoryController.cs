using AMS.Repository;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace AMS.Controllers;

public class CaseCategoryController : Controller
{
    private readonly ICaseCategoryRepository _categoryRepository;

    public CaseCategoryController(ICaseCategoryRepository categoryRepository)
    {
        _categoryRepository = categoryRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var data = await _categoryRepository.GetAllCaseCategoryAsync(cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == 0)
        {
            return View();
        }
        var data = await _categoryRepository.GetCaseCategoryByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }   
    [HttpPost]
    public async Task<IActionResult> CreateOrEdit(CaseCategory category, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            if (category.Id == 0)
            {
                await _categoryRepository.AddCaseCategoryAsync(category, cancellationToken);
            }
            else
            {
                await _categoryRepository.UpdateCaseCategoryAsync(category, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(category);
    }
    [HttpPost]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _categoryRepository.DeleteCaseCategoryAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }
    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var data = await _categoryRepository.GetCaseCategoryByIdAsync(id, cancellationToken);
        if (data != null)
        {
            return View(data);
        }
        return NotFound();
    }
}
