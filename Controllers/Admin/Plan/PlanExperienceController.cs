using brandportal_dotnet.Contracts.Plan.PlanExperience;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using brandportal_dotnet.shared;
using Volo.Abp.Application.Dtos;

namespace brandportal_dotnet.Controllers.Plan;

[Route("[controller]")]
[ApiController]
public class PlanExperienceController
{
    private readonly IRepository<GroupTripPlan> _groupTripPlanService;
    private readonly IRepository<Account> _accountService;

    public PlanExperienceController
    (
        IRepository<GroupTripPlan> groupTripPlanService,
        IRepository<Account> accountService
    )
    {
        _groupTripPlanService = groupTripPlanService;
        _accountService = accountService;
    }
    
    [HttpGet("~/api/admin/plan-experience/paged")]
    public async Task<PagedResultDto<PagePlanExperienceDto>> GetListPagedAsync(PagingRequestInput input)
    {
        var query = (from groupPlan in await _groupTripPlanService.GetAll()
                join user in await _accountService.GetAll() on groupPlan.UserId equals user._Id
                join userExperience in await _accountService.GetAll() on groupPlan.UserExperienceId equals userExperience._Id
                where groupPlan.UserExperienceId != null
                select new PagePlanExperienceDto
                {
                    UserId = groupPlan.UserExperienceId,
                    UserNameInherit = user.FullName,
                    Departure = groupPlan.Departure,
                    Destination = groupPlan.Destination,
                    CreatedAt = groupPlan.CreatedAt,
                    UserName = userExperience.FullName,
                    Id = groupPlan._Id
                }).Where(input);

        var items = query
            .OrderByDescending(x => x.CreatedAt)
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<PagePlanExperienceDto>(total, items);
    }
}