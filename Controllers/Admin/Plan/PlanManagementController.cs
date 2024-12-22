using brandportal_dotnet.Contracts.Plan.PlanManagement;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using brandportal_dotnet.shared;
namespace brandportal_dotnet.Controllers.Plan;

[Route("[controller]")]
[ApiController]
public class PlanManagementController
{
    private readonly IRepository<GroupTripPlan> _groupTripPlanService;
    private readonly  IRepository<Account> _accountService;

    public PlanManagementController
    (
        IRepository<GroupTripPlan> groupTripPlanService,
            IRepository<Account> accountService
    )
    {
        _groupTripPlanService = groupTripPlanService;
        _accountService = accountService;
    }
    
    [HttpGet("~/api/admin/plan-management/paged")]
    public async Task<PagedResultDto<PageManagementPlanDto>> GetListPagedAsync(PagingRequestInput input)
    {
        var query = (from groupPlan in await _groupTripPlanService.GetAll()
                join user in await _accountService.GetAll() on groupPlan.UserId equals user._Id
                select new PageManagementPlanDto
                {
                    IsPublic = groupPlan.IsPublic,
                    UserId = groupPlan.UserId,
                    Departure = groupPlan.Departure,
                    Destination = groupPlan.Destination,
                    CreatedAt = groupPlan.CreatedAt,
                    UserName = user.FullName,
                    Id = groupPlan._Id
                }).Where(input);

        var items = query
            .OrderByDescending(x => x.CreatedAt)
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<PageManagementPlanDto>(total, items);
    }
    
}