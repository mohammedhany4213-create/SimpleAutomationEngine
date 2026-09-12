using Microsoft.AspNetCore.Mvc;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Application.DTOs;

namespace SimpleAutomationEngine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ActionTasksController : ControllerBase
{
    private readonly IActionTaskService _service;
    public ActionTasksController(IActionTaskService service)
    {
        _service = service ;
    }

    [HttpGet]
    public async Task <IActionResult> GetAll()
    {
        var result = await _service.GetActionTasksAsync();
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task <IActionResult> GetById(int id)
    {
        try
        {
            var result = await _service.GetActionTaskByIdAsync(id);
            return Ok(result);
        }

        catch(KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task <IActionResult> Create(CreateActionTaskDto dto)
    {
        var result = await _service.CreateActionTaskAsync(dto);
        return CreatedAtAction(nameof(GetById) , new {id = result.ActionTaskId} , result);
    }

    [HttpPut("{id}")]
    public async Task <IActionResult> Update(int id , UpdateActionTaskDto dto)
    {
        try
        {
            var result = await _service.UpdateActionTaskAsync(id , dto);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            await _service.DeleteActionTaskAsync(id);
            return NoContent();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

}