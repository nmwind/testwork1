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
[Route("api/projects")]
public class ProjectsController : ControllerBase
{
    private readonly IProjectsRepository _projectsRepository;
    private readonly IProjectTasksRepository _projectTasksRepository;

    public ProjectsController(
        IProjectsRepository projectsRepository,
        IProjectTasksRepository projectTasksRepository
    )
    {
        _projectsRepository = projectsRepository;
        _projectTasksRepository = projectTasksRepository;
    }

    /// <summary>
    /// Returns a collection of projects.
    /// </summary>
    /// <returns>A collection of projects.</returns>
    /// <response code="200">Collection of projects.</response>
    /// <response code="400">An unexpected error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(ProjectListItemModel[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync(
        string? name,
        ProjectStatus? status,
        Guid? supervisorId,
        string? projectBy,
        SortDirection sortDirection,
        int pageIndex,
        int pageSize)
    {
        var result = await _projectsRepository.GetAsync(name, status, supervisorId, projectBy,
            sortDirection switch
            {
                SortDirection.Asc => true,
                SortDirection.Desc => false,
                _ => true
            },
            pageIndex,
            pageSize);

        var model = new PagedResponse<ProjectListItemModel>
        {
            Total = result.Total,
            Items = result.Items
                .Select(project => new ProjectListItemModel
                {
                    Id = project.Id,
                    Name = project.Name,
                    StartDate = project.StartDate,
                    EndDate = project.EndDate,
                    SupervisorId = project.SupervisorId,
                    ExecutorId = project.ExecutorId,
                    Status = project.Status,
                    Stages = project.StagesCount,
                    Tasks = project.TasksCount,
                    IsDeleted = project.IsDeleted,
                    CreatedAt = project.CreatedAt,
                    UpdatedAt = project.UpdatedAt,
                }).ToList()
        };
        return Ok(model);
    }

    /// <summary>
    /// Retrieves a project by <paramref name="projectId"/>.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A project.</returns>
    /// <response code="200">A project.</response>
    /// <response code="400">An unexpected error.</response>
    /// <response code="404">A project not found by provided identifier.</response>
    [HttpGet("{projectId:guid}")]
    [ProducesResponseType(typeof(ProjectModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> GetByIdAsync([FromRoute] Guid projectId)
    {
        var project = await _projectsRepository.GetByIdAsync(projectId);

        if (project == null)
            return NotFound();

        var tasks = await _projectTasksRepository.GetAsync(projectId);
        var model = new ProjectModel
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            Risks = project.Risks,
            StartDate = project.StartDate,
            EndDate = project.EndDate,
            SupervisorId = project.SupervisorId,
            ExecutorId = project.ExecutorId,
            Status = project.Status,
            Stages = project.Stages,
            Tasks = tasks.Select(task => new ProjectTaskModel
            {
                Id = task.Id,
                ProjectId = project.Id,
                Stage = task.Stage,
                Order = task.Order,
                Title = task.Title,
                Start = task.Start,
                End = task.End,
                IsDeleted = task.IsDeleted,
                CreatedAt = task.CreatedAt,
                UpdatedAt = task.UpdatedAt,
            }).ToList(),
            IsDeleted = project.IsDeleted,
            CreatedAt = project.CreatedAt,
            UpdatedAt = project.UpdatedAt,
        };

        return Ok(model);
    }

    /// <summary>
    /// Creates project.
    /// </summary>
    /// <param name="model">The project creation information.</param>
    /// <returns>Created project result.</returns>
    /// <response code="200">Created project result.</response>
    /// <response code="400">An unexpected error.</response>
    [HttpPost]
    [ProducesResponseType(typeof(ProjectCreateResultModel), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> CreateAsync([FromBody] ProjectCreateModel model)
    {
        try
        {
            var project = Project.Create(
                model.Name,
                model.Description,
                model.Risks,
                model.StartDate,
                model.EndDate,
                model.SupervisorId,
                model.ExecutorId
            );

            await _projectsRepository.InsertAsync(project);

            return Ok(new ProjectCreateResultModel { Id = project.Id });
        }
        catch (Exception exception)
        {
            throw exception;
            //log error
            //return BadRequest(ErrorResponse.UnexpectedError());
        }
    }

    /// <summary>
    /// Updates project.
    /// </summary>
    /// <param name="model">The project update information.</param>
    /// <returns></returns>
    /// <response code="204">Order successfully updated.</response>
    /// <response code="400">An unexpected error.</response>
    /// <response code="404">Order not found.</response>
    [HttpPut]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> UpdateAsync([FromBody] ProjectUpdateModel model)
    {
        var project = await _projectsRepository.GetByIdAsync(model.Id);

        if (project == null)
            return NotFound();

        project.Update(
            model.SupervisorId,
            model.ExecutorId
        );

        project.UpdateStages(model.Stages);

        await _projectsRepository.UpdateAsync(project);

        return NoContent();
    }

    /// <summary>
    /// Deletes project.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <response code="204">Project deleted.</response>
    /// <response code="400">An unexpected error.</response>
    /// <response code="404">Project not found by provided identifier.</response>
    [HttpDelete("{projectId:guid}")]
    [ProducesResponseType((int)HttpStatusCode.NoContent)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    [ProducesResponseType((int)HttpStatusCode.NotFound)]
    public async Task<IActionResult> DeleteAsync([FromRoute] Guid projectId)
    {
        var project = await _projectsRepository.GetByIdAsync(projectId);

        if (project == null)
            return NotFound();

        if (project.Delete())
            await _projectsRepository.UpdateAsync(project);

        return NoContent();
    }

    /// <summary>
    /// Retrieves a collection of project tasks.
    /// </summary>
    /// <param name="projectId">The project identifier.</param>
    /// <returns>A collection of project tasks.</returns>
    /// <response code="200">A collection of project tasks.</response>
    /// <response code="400">An unexpected error.</response>
    [HttpGet("{projectId:guid}/tasks")]
    [ProducesResponseType(typeof(ProjectTaskListItemModel[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType((int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetTasksAsync(
        [FromRoute] Guid projectId)
    {
        throw new NotImplementedException();
        return Ok();
    }
}