using AMS.Models;
using AMS.Repository;
using Microsoft.AspNetCore.Mvc;

namespace AMS.Controllers;

public class TaskManagementController : Controller
{
    private readonly ITaskManagementRepository _taskManagementRepository;

    public TaskManagementController(ITaskManagementRepository taskManagementRepository)
    {
        _taskManagementRepository = taskManagementRepository;
    }

    public async Task<IActionResult> Index(CancellationToken cancellationToken)
    {
        var tasks = await _taskManagementRepository.GetAllTasksAsync(cancellationToken);
        if (tasks != null)
        {
            return View(tasks);
        }
        return NotFound();
    }
    
    [HttpGet]
    public async Task<IActionResult> CreateOrEdit(long id, CancellationToken cancellationToken)
    {
        if (id == 0)
        {
            return View(new TaskManagement());
        }
        else
        {
            var task = await _taskManagementRepository.GetTaskByIdAsync(id, cancellationToken);
            if (task != null)
            {
                return View(task);
            }
            return NotFound();
        }
    }
     [HttpPost]
     public async Task<IActionResult> CreateOrEdit(TaskManagement taskManagement, CancellationToken cancellationToken)
    {
        if (ModelState.IsValid)
        {
            if (taskManagement.Id == 0)
            {
                await _taskManagementRepository.AddTaskAsync(taskManagement, cancellationToken);
            }
            else
            {
                await _taskManagementRepository.UpdateTaskAsync(taskManagement, cancellationToken);
            }
            return RedirectToAction(nameof(Index));
        }
        return View(taskManagement);
    }
     [HttpPost]
     public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _taskManagementRepository.DeleteTaskAsync(id, cancellationToken);
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Details(long id, CancellationToken cancellationToken)
    {
        var task = await _taskManagementRepository.GetTaskByIdAsync(id, cancellationToken);
        if (task != null)
        {
            return View(task);
        }
        return NotFound();
    }
}
