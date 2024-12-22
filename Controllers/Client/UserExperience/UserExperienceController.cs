using brandportal_dotnet.Contracts.Client.Plan;
using brandportal_dotnet.Contracts.UserExperience;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.Controllers.Client.UserExperience;

[Route("[controller]")]
[ApiController]
public class UserExperienceController : ControllerBase
{
    private readonly IRepository<GroupTripPlan> _groupTripPlanService;
    private readonly IRepository<DetailTripPlan> _detailTripPlanService;
    private readonly IRepository<Account> _accountService;

    public UserExperienceController(IRepository<GroupTripPlan> groupTripPlanService,
        IRepository<DetailTripPlan> detailTripPlanService,
        IRepository<Account> accountService)
    {
        _groupTripPlanService = groupTripPlanService;
        _detailTripPlanService = detailTripPlanService;
        _accountService = accountService;
    }

    [HttpGet("~/api/user-experience/paged")]
    public async Task<IActionResult> GetUserExperience([FromQuery] FilterListUserExperience filter)
    {
        var groupTripPlans = await _groupTripPlanService.GetAll();
        IEnumerable<GroupTripPlan>? dataGroup = groupTripPlans.OrderByDescending(x => x.CreatedAt);
        if (filter.Destination != null && filter.Destination != "")
        {
            dataGroup = dataGroup.Where(x =>
                x.Destination != null &&
                x.Destination.Contains(filter.Destination, StringComparison.OrdinalIgnoreCase) &&
                x.IsPublic == true);
        }

        if (filter.DayTotal != null)
        {
            if (filter.DayTotal <= 5)
            {
                dataGroup = dataGroup.Where(x =>
                    (x.EndDate - x.StartDate)?.Days + 1 == filter.DayTotal &&
                    x.IsPublic == true);
            }
            else
            {
                dataGroup = dataGroup.Where(x =>
                    (x.EndDate - x.StartDate)?.Days + 1 >= filter.DayTotal - 1 &&
                    x.IsPublic == true);
            }
        }

        if (filter.PriceTotal != null)
        {
            if (filter.PriceTotal == 1)
            {
                dataGroup = dataGroup.Where(x =>
                    (x.PriceTotal <= 1000000) || (x.PriceTotal == null) && x.IsPublic == true);
            }
            else if (filter.PriceTotal == 2)
            {
                dataGroup = dataGroup.Where(
                    x => x.PriceTotal > 1000000 && x.PriceTotal <= 2000000 && x.IsPublic == true);
            }
            else if (filter.PriceTotal == 3)
            {
                dataGroup = dataGroup.Where(
                    x => x.PriceTotal > 2000000 && x.PriceTotal <= 3000000 && x.IsPublic == true);
            }
            else if (filter.PriceTotal == 4)
            {
                dataGroup = dataGroup.Where(
                    x => x.PriceTotal > 3000000 && x.PriceTotal <= 4000000 && x.IsPublic == true);
            }
            else if (filter.PriceTotal == 5)
            {
                dataGroup = dataGroup.Where(
                    x => x.PriceTotal > 4000000 && x.PriceTotal <= 5000000 && x.IsPublic == true);
            }
            else
            {
                dataGroup = dataGroup.Where(x => x.PriceTotal > 5000000 && x.IsPublic == true);
            }
        }

        var totalPage = dataGroup.Where(x => x.IsPublic == true).Count();
        dataGroup = dataGroup.Where(x => x.IsPublic == true).Skip(5 * (filter.CurrentPage - 1)).Take(5);

        var userExperienceDetails = new List<UserExperienceDetail>();

        foreach (var groupTripPlan in dataGroup)
        {
            var dateInPlanList = new List<string>();

            // Populate date range
            if (groupTripPlan.StartDate.HasValue && groupTripPlan.EndDate.HasValue)
            {
                DateTime currentDate = groupTripPlan.StartDate.Value;
                DateTime endDate = groupTripPlan.EndDate.Value;

                while (currentDate <= endDate)
                {
                    dateInPlanList.Add(currentDate.ToString("dd/MM/yyyy"));
                    currentDate = currentDate.AddDays(1);
                }
            }

            string[] dateInPlan = dateInPlanList.ToArray();
            int maxDays = (groupTripPlan.EndDate - groupTripPlan.StartDate)?.Days + 1 ?? 0;

            var placeList = new List<UserExperienceDto>[maxDays];
            for (int i = 0; i < maxDays; i++)
            {
                placeList[i] = new List<UserExperienceDto>();
            }

            // Get trip details
            var detailTrip = await _detailTripPlanService.FindListByProperties(new Dictionary<string, object>
            {
                { "GroupTripPlanId", groupTripPlan._Id }
            });

            foreach (var item in detailTrip)
            {
                int dayIndex = item.NumberDay - 1; // Convert to 0-based index

                if (dayIndex >= 0 && dayIndex < maxDays)
                {
                    placeList[dayIndex].Add(new UserExperienceDto
                    {
                        Name = item.Name,
                        Photos = item.Photos,
                        HasExperienced = item.HasExperienced ?? false,
                        Url = item.Url,
                        International_phone_number = item.International_phone_number,
                        Rating = item.Rating,
                        Distance = item.Distance,
                        Duration = item.Duration,
                        PlaceId = item.PlaceId,
                        User_ratings_total = item.UserRatingsTotal
                    });
                }
            }

            var user = await _accountService.GetById(groupTripPlan.UserId);

            // Create UserExperienceDetail
            var userExperienceDetail = new UserExperienceDetail
            {
                Id = groupTripPlan._Id,
                planDetail = placeList.Select(l => l.ToArray()).ToArray(),
                DateInPlan = dateInPlan,
                Departure = groupTripPlan.Departure,
                Destination = groupTripPlan.Destination,
                PriceTotal = groupTripPlan.PriceTotal,
                name = user.FullName,
                avatar = user.Avatar,
                UserId = groupTripPlan.UserId,
                CreateDate = groupTripPlan.CreatedAt
            };

            userExperienceDetails.Add(userExperienceDetail);
        }

        // Create UserExperienceResponse
        var userExperienceResponse = new UserExperienceResponse
        {
            UserExperience = userExperienceDetails.OrderBy(x => x.CreateDate).ToArray(),
            PageSize = 5,
            TotalPage = totalPage,
            CurrentPage = filter.CurrentPage
        };

        return Ok(userExperienceResponse);
    }

    [HttpPost("~/api/user-experience/paged")]
    public async Task<IActionResult> UpdateUserExperience([FromBody] CreatePlanExperienceDto request)
    {
        var groupTrip = await _groupTripPlanService.GetById(request.GroupId);
        if (groupTrip == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy lịch trình"
            });
        }

        var UserExperience = await _accountService.GetById(request.UserExperienceId);
        if (UserExperience == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy người tạo lịch"
            });
        }

        var User = await _accountService.GetById(request.UserId);
        if (User == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy người dùng"
            });
        }


        var detailTrip = await _detailTripPlanService.FindListByProperties(new Dictionary<string, object>
        {
            { "GroupTripPlanId", groupTrip._Id }
        });

        var newGroupTripPlan = new GroupTripPlan
        {
            Name = groupTrip.Name,
            IsExpired = false,
            StartDate = request.StartDate,
            EndDate = request.EndDate,
            CreatedAt = DateTime.Now,
            IsPublic = false,
            UserId = request.UserId,
            Departure = groupTrip.Departure,
            Destination = groupTrip.Destination,
            PriceTotal = groupTrip.PriceTotal,
            View = groupTrip.View,
            StartDateShare = null,
            EndDateShare = null,
            GroupTripPlanId = groupTrip._Id,
            UserExperienceId = request.UserExperienceId
        };
        var saveGroupTrip = await _groupTripPlanService.Insert(newGroupTripPlan, true);
        groupTrip.View += 1;
        UserExperience.Point += 3;
        User.Point -= 5;
        await _groupTripPlanService.Update(groupTrip._Id, groupTrip);
        await _accountService.Update(UserExperience._Id, UserExperience);
        await _accountService.Update(User._Id, User);
        foreach (var item in detailTrip)
        {
            var newDetailTripPlan = new DetailTripPlan
            {
                Name = item.Name,
                Photos = item.Photos,
                HasExperienced = false,
                Url = item.Url,
                International_phone_number = item.International_phone_number,
                Rating = item.Rating,
                Distance = item.Distance,
                Duration = item.Duration,
                UserRatingsTotal = item.UserRatingsTotal,
                NumberDay = item.NumberDay,
                GroupTripPlanId = saveGroupTrip._Id,
                PlaceId = item.PlaceId,
                TimeActive = item.TimeActive,
                TimeOpen = item.TimeOpen,
                TimeClose = item.TimeClose,
                Formatted_address = item.Formatted_address,
                TitleNote = null,
                DescribeNote = null,
                Price = item.Price,
                Lat = item.Lat,
                Lng = item.Lng
            };
            await _detailTripPlanService.Insert(newDetailTripPlan);
        }

        return Ok(new LookupDto
        {
            Id = saveGroupTrip._Id,
            Code = newGroupTripPlan.Departure,
            Name = newGroupTripPlan.Destination
        });
    }
}