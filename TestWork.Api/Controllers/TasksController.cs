using System.Collections.Immutable;
using System.Net;
using Microsoft.AspNetCore.Mvc;
using TestWork.Api.Models;
using TestWork.Api.Models.Projects;
using TestWork.Api.Models.Tasks;
using TestWork.Api.Models.Users;
using TestWork.Data.Repositories;
using TestWork.Entities;
using TestWork.Repositories;

namespace TestWork.Api.Controllers;

[ApiController]
[Route("api/tasks")]
public class TasksController : ControllerBase
{
    private readonly IProjectTasksRepository _tasksRepository;
    private readonly IProjectsRepository _projectsRepository;

    public TasksController(
        IProjectTasksRepository tasksRepository,
        IProjectsRepository projectsRepository
    )
    {
        _tasksRepository = tasksRepository;
        _projectsRepository = projectsRepository;
    }

    
    /// <summary>
    /// Mark task as deleted.
    /// </summary>
    /// <param name="taskId">The task identifier.</param>
    /// <response code="204">Task deleted.</response>
    /// <response code="400">An unexpected error.</response>
    /// <response code="404">Task not found by provided identifier.</response>
    [HttpDelete("{taskId:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid taskId)
    {
        var task = await _tasksRepository.GetByIdAsync(taskId);

        if (task == null)
            return NotFound();

        if (task.Delete())
            await _tasksRepository.UpdateAsync(task);

        return NoContent();
    }
}