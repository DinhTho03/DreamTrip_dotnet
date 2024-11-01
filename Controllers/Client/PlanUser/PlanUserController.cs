using brandportal_dotnet.Contracts.Client.Plan;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.Controllers.Client.PlanUser;

[Route("[controller]")]
[ApiController]
public class PlanUserController : ControllerBase
{
    private readonly IRepository<GroupTripPlan> _groupTripPlanService;
    private readonly IRepository<DetailTripPlan> _detailTripPlanService;

    public PlanUserController(IRepository<GroupTripPlan> groupTripPlanService,
        IRepository<DetailTripPlan> detailTripPlanService)
    {
        _groupTripPlanService = groupTripPlanService;
        _detailTripPlanService = detailTripPlanService;
    }

    [HttpPost("~/api/plan-user/manager/paged")]
    public async Task<IActionResult> GetListPagedAsync(PlanRequestDto data)
    {
        if (data.NamePlan == null)
        {
            return BadRequest(new
            {
                message = "Trường địa chỉ đến là bắt buộc"
            });
        }

        if (data.Data == null)
        {
            return BadRequest(new
            {
                message = "Chưa có lịch trình để khởi tạo"
            });
        }

        var groupTripPlan = new GroupTripPlan
        {
            Name = data.NamePlan,
            StartDate = data.StartDate,
            EndDate = data.EndDate,
            UserId = "1",
            IsExpired = false,
            CreatedAt = DateTime.Now,
            IsPublic = false
        };

        var groupTripPlanId = await _groupTripPlanService.Insert(groupTripPlan, true);

        foreach (var items in data.Data.Select((items, index) => new { items, numberDay = index + 1 }))
        {
            DateTime timeActive = new DateTime(1, 1, 1, 5, 0, 0); // Khởi đầu từ 5 AM

            foreach (var place in items.items)
            {
                string timeActiveString = timeActive.ToString("htt").ToLower(); // Định dạng "h tt"

                var detailTripPlan = new DetailTripPlan
                {
                    Name = place.Name,
                    PlaceId = place.PlaceId,
                    GroupTripPlanId = groupTripPlanId._Id,
                    Lat = place.Geometry.Location.Lat,
                    Lng = place.Geometry.Location.Lng,
                    Distance = place.Distance,
                    Photos = place.Photos,
                    Rating = place.Rating,
                    HasExperienced = false,
                    UserRatingsTotal = place.User_ratings_total,
                    NumberDay = items.numberDay,
                    TimeActive = timeActiveString 
                };

                await _detailTripPlanService.Insert(detailTripPlan);

                timeActive = timeActive.AddHours(2);
            }
        }


        return Ok(new PlanResponse
        {
            Id = groupTripPlan._Id
        });
    }

    [HttpGet("~/api/plan-user/timeline/{id}")]
    public async Task<IActionResult> GetDetailAsync(string id)
    {
        var groupTripPlan = await _groupTripPlanService.GetById(id);
        if (groupTripPlan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy lịch trình"
            });
        }

        string[] dateInPlan = new string[]
        {
            groupTripPlan.StartDate?.ToString("dd/MM/yyyy") ?? "",
            groupTripPlan.EndDate?.ToString("dd/MM/yyyy") ?? ""
        };

        int maxDays =
            (groupTripPlan.EndDate - groupTripPlan.StartDate)?.Days + 1 ??
            0; 
        PlaceTimeLineDto[][] placeList = new PlaceTimeLineDto[maxDays][];

        var detailTripPlan = await _detailTripPlanService.FindListByProperties(new Dictionary<string, object>
        {
            { "GroupTripPlanId", id }
        });

        if (detailTripPlan == null || !detailTripPlan.Any())
        {
            return NotFound(new
            {
                message = "Không tìm thấy lịch trình"
            });
        }

        foreach (var item in detailTripPlan)
        {
            int dayIndex = item.NumberDay - 1;

            if (dayIndex >= 0 && dayIndex < maxDays)
            {
                placeList[dayIndex] ??= new PlaceTimeLineDto[] { }; 
                placeList[dayIndex] = placeList[dayIndex].Append(new PlaceTimeLineDto
                {
                    Name = item.Name,
                    Id = item._Id,
                    Photos = item.Photos,
                    HasExperienced = item.HasExperienced ?? false,
                    TimeActive = item.TimeActive,
                }).ToArray();
            }
        }

        return Ok(new PlaceTimeLineResponse
        {
            PlaceList = placeList,
            DateInPlan = dateInPlan
        });
    }
    
    [HttpGet("~/api/plan-user/group")]
    public async Task<List<LookupDto>> GetGroupTripPlanAsync()
    {
        var query = (from trip in await _groupTripPlanService.GetAll()
            where trip.UserId == "1"
            select new LookupDto
            {
                Id = trip._Id,
                Name = trip.Name,
                Code = trip.Name
            });

        return query.ToList();
       
    }
    
    [HttpPut("~/api/plan-user/group/rename/{id}")]
    public async Task<IActionResult> RenameGroupTripPlanAsync(GroupPlanDto data)
    {
        var groupTripPlan = await _groupTripPlanService.GetById(data.Id);
        if (groupTripPlan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy nhóm kế hoạch"
            });
        }

        groupTripPlan.Name = data.Name;

        await _groupTripPlanService.Update(data.Id, groupTripPlan);

        return Ok(new PlanResponse
        {
            Id = groupTripPlan._Id
        });
    }
    
    [HttpPut("~/api/plan-user/timeline/status/{id}")]
    public async Task<IActionResult> GroupTripPlanAsync(string id, [FromBody] bool hasExperienced)
    {
        var detail = await _detailTripPlanService.GetById(id);
        if (detail == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đại điểm này"
            });
        }

        detail.HasExperienced = hasExperienced;

        await _detailTripPlanService.Update(detail._Id, detail);
        var groupTripPlan = await _groupTripPlanService.GetById(detail.GroupTripPlanId);
        return Ok(new PlanResponse
        {
            Id = groupTripPlan._Id
        });
    }
}