using brandportal_dotnet.Contracts.Admin.User;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace brandportal_dotnet.Controllers.Admin.User;

[Route("[controller]")]
[ApiController]
public class UserController : ControllerBase
{
    private readonly IRepository<Account> _accountService;

    public UserController
    (
        IRepository<Account> accountService
    )
    {
        _accountService = accountService;
    }

    [HttpGet("~/api/admin/user")]
    public async Task<PagedResultDto<ListUserDto>> GetListUserAsync(PagingRequestInput input)
    {
        var queryUser = await _accountService.GetAll();
        var query = (from a in queryUser
            select new ListUserDto
            {
                Id = a._Id,
                FullName = a.FullName,
                Phone = a.Phone,
                Email = a.Email,
                IsDeleted = a.IsDeleted
            }).Where(input);
        var data = query
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<ListUserDto>(total, data);
    }

    [HttpPut("~/api/admin/user/{id}/status")]
    public async Task<IActionResult> UpdateStatusFaqAsync(string id, [FromBody] bool IsDeleted)
    {
        var user = await _accountService.GetById(id);
        if (user == null)
        {
            return BadRequest(new { message = "Người dùng này không tồn tại" });
        }

        user.IsDeleted = IsDeleted;
        await _accountService.Update(id, user);
        return Ok();
    }
}