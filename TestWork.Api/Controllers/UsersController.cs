using System.Net;
using Microsoft.AspNetCore.Mvc;
using TestWork.Api.Models;
using TestWork.Api.Models.Users;
using TestWork.Data.Repositories;
using TestWork.Repositories;

namespace TestWork.Api.Controllers;

[ApiController]
[Route("api/users")]
public class UsersController(IUsersRepository usersRepository) : ControllerBase
{
    /// <summary>
    /// Returns a collection of users.
    /// </summary>
    /// <returns>A collection of users.</returns>
    /// <response code="200">Collection of users.</response>
    /// <response code="400">An unexpected error.</response>
    [HttpGet]
    [ProducesResponseType(typeof(UserListItemModel[]), (int)HttpStatusCode.OK)]
    [ProducesResponseType(typeof(ErrorResponse), (int)HttpStatusCode.BadRequest)]
    public async Task<IActionResult> GetAsync(
        string? searchValue,
        string? orderBy,
        SortDirection sortDirection,
        int pageIndex,
        int pageSize)
    {
        try
        {
            var result = await usersRepository.GetAsync(
                searchValue,
                orderBy,
                sortDirection switch
                {
                    SortDirection.Asc => true,
                    SortDirection.Desc => false,
                    _ => true
                },
                pageIndex,
                pageSize);

            var model = new PagedResponse<UserListItemModel>
            {
                Total = result.Total,
                Items = result.Items
                    .Select(user => new UserListItemModel
                    {
                        Id = user.Id,
                        FirstName = user.FirstName,
                        LastName = user.LastName,
                        MiddleName = user.MiddleName!,
                        Email = user.Email,
                        CreatedAt = user.CreatedAt.UtcDateTime,
                        UpdatedAt = user.UpdatedAt.UtcDateTime
                    }).ToList()
            };

            return Ok(model);
        }
        catch (Exception exception)
        {
            // _logger.LogError(exception, "An error occurred while getting users");
            return BadRequest(ErrorResponse.UnexpectedError());
        }
    }
}