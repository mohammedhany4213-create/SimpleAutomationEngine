using Microsoft.AspNetCore.Mvc;
using SimpleAutomationEngine.Application.Interfaces;
using SimpleAutomationEngine.Application.DTOs;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace SimpleAutomationEngine.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ActionTasksController : ControllerBase
{
    private readonly IActionTaskService _service;
    public ActionTasksController(IActionTaskService service)
    {
        _service = service ;
    }

    private int GetCurrentUserId()
{
    var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)!.Value;
    return int.Parse(userIdClaim);
}

    [HttpGet]
public async Task<IActionResult> GetAll()
{
    var userId = GetCurrentUserId();
    var result = await _service.GetAllAsync(userId);
    return Ok(result);
}

    [HttpGet("{id}")]
public async Task<IActionResult> GetById(int id)
{
    var userId = GetCurrentUserId();
    var result = await _service.GetByIdAsync(id, userId);
    return result is null ? NotFound() : Ok(result);
}

[HttpPost]
public async Task<IActionResult> Create(CreateActionTaskDto dto)
{
    var userId = GetCurrentUserId();
    var result = await _service.CreateActionTaskAsync(dto, userId);
    return CreatedAtAction(nameof(GetById), new { id = result.UserId }, result);
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