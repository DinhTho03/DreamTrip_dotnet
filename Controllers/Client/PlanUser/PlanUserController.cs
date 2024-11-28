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
    public async Task<IActionResult> AddListPlandAsync(PlanRequestDto data)
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
            StartDate = data.StartDate.AddDays(1),
            EndDate = data.EndDate.AddDays(1),
            UserId = "1",
            IsExpired = false,
            CreatedAt = DateTime.Now,
            IsPublic = false,
            Departure = data.Departure,
            Destination = data.Destination
        };

        var groupTripPlanId = await _groupTripPlanService.Insert(groupTripPlan, true);

        foreach (var items in data.Data.Select((items, index) => new { items, numberDay = index + 1 }))
        {
            DateTime timeActive;

            // Thiết lập thời gian bắt đầu cho từng ngày
            if (items.numberDay == 1)
            {
                timeActive = new DateTime(1, 1, 1, 1, 0, 0); // Ngày đầu tiên: 0 AM
            }
            else
            {
                timeActive = new DateTime(1, 1, 1, 7, 0, 0); // Các ngày còn lại: 7 PM
            }

            // Duyệt qua từng địa điểm
            for (int i = 0; i < items.items.Count(); i++)
            {
                var place = items.items[i];

                // Định dạng thời gian thành chuỗi theo "h tt"
                string timeActiveString = timeActive.ToString("htt").ToLower();

                // Kiểm tra và tách giờ cho TimeOpen
                string hours = "";
                string minutes = "";
                if (!string.IsNullOrEmpty(place.TimeOpen) && place.TimeOpen.Length >= 4)
                {
                    hours = place.TimeOpen.Substring(0, 2); // Lấy 2 ký tự đầu (giờ)
                    minutes = place.TimeOpen.Substring(2, 2); // Lấy 2 ký tự tiếp theo (phút)
                }

                string TimeOpen = $"{hours}:{minutes}"; // Kết hợp lại thành HH:mm

                string hoursClose = "";
                string minutesClose = "";
                if (!string.IsNullOrEmpty(place.TimeClose) && place.TimeClose.Length >= 4)
                {
                    hoursClose = place.TimeClose.Substring(0, 2); // Lấy 2 ký tự đầu (giờ)
                    minutesClose = place.TimeClose.Substring(2, 2); // Lấy 2 ký tự tiếp theo (phút)
                }

                string TimeClose = $"{hoursClose}:{minutesClose}";


                var detailTripPlan = new DetailTripPlan
                {
                    Name = place.Name,
                    PlaceId = place.PlaceId,
                    GroupTripPlanId = groupTripPlanId._Id,
                    Lat = place.Geometry.Location.Lat,
                    Lng = place.Geometry.Location.Lng,
                    Distance = place.Distance,
                    Duration = place.Duration,
                    Photos = place.Photos,
                    Rating = place.Rating,
                    HasExperienced = false,
                    UserRatingsTotal = place.User_ratings_total,
                    NumberDay = items.numberDay,
                    TimeActive = timeActiveString,
                    Url = place.Url,
                    International_phone_number = place.International_phone_number,
                    TimeOpen = TimeOpen,
                    TimeClose = TimeClose,
                    Formatted_address = place.Formatted_address,
                    Price = null,
                    TitleNote = "Tiêu đề ghi chú",
                    DescribeNote = "Nội dung ghi chú",
                };

                await _detailTripPlanService.Insert(detailTripPlan);

                // Điều chỉnh thời gian dựa trên vị trí và ngày
                if (items.numberDay == 1)
                {
                    // Ngày đầu tiên
                    if (i == 0)
                    {
                        timeActive = new DateTime(1, 1, 1, 14, 0, 0); // Điểm thứ hai: 2 PM
                    }
                    else
                    {
                        timeActive = timeActive.AddHours(2); // Các điểm tiếp theo: Tăng 2 giờ
                    }
                }
                else if (items.numberDay >= 3 && i == items.items.Count() - 1)
                {
                    // Ngày thứ 3 trở đi, điểm cuối cùng là 0 AM
                    timeActive = new DateTime(1, 1, 1, 12, 0, 0);
                }
                else
                {
                    // Ngày thứ 2 trở đi (không phải ngày đầu), tăng 2 giờ
                    timeActive = timeActive.AddHours(2);
                }
            }
        }


        return Ok(new PlanResponse
        {
            Id = groupTripPlan._Id
        });
    }

    [HttpPut("~/api/plan-user/detail/{id}")]
    public async Task<IActionResult> EditPlandAsync(string id, UpdatePlanRequestDto data)
    {
        var conditionDetailPlan = new Dictionary<string, object>
        {
            { "GroupTripPlanId", id }
        };
        var detailPlan = await _detailTripPlanService.FindListByProperties(conditionDetailPlan);
        await _detailTripPlanService.DeleteAll(detailPlan.Select(x => x._Id).ToList());
        foreach (var items in data.Data.Select((items, index) => new { items, numberDay = index + 1 }))
        {
            DateTime timeActive;
            for (int i = 0; i < items.items.Count(); i++)
            {
                var place = items.items[i];
                
                var detailTripPlan = new DetailTripPlan
                {
                    Name = place.Name,
                    PlaceId = place.PlaceId,
                    GroupTripPlanId = id,
                    Lat = place.Geometry.Location.Lat,
                    Lng = place.Geometry.Location.Lng,
                    Distance = place.Distance,
                    Duration = place.Duration,
                    Photos = place.Photos,
                    Rating = place.Rating,
                    HasExperienced = place.HasExperienced ?? false,
                    UserRatingsTotal = place.User_ratings_total,
                    NumberDay = items.numberDay,
                    TimeActive = place.TimeActive,
                    Url = place.Url,
                    International_phone_number = place.International_phone_number,
                    TimeOpen = place.TimeOpen,
                    TimeClose = place.TimeClose,
                    Formatted_address = place.Formatted_address,
                    Price = place.Price ?? null,
                    TitleNote = place.TitleNote ?? "Tiêu đề ghi chú",
                    DescribeNote = place.DescribeNote ?? "Tiêu đề ghi chú",
                };
                await _detailTripPlanService.Insert(detailTripPlan);
            }
        }


        return Ok(new PlanResponse
        {
            Id = id
        });
    }

    [HttpGet("~/api/plan-user/timeline/{id}")]
    public async Task<IActionResult> GetTimelineAsync(string id)
    {
        var groupTripPlan = await _groupTripPlanService.GetById(id);
        if (groupTripPlan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy lịch trình"
            });
        }

        var dateInPlanList = new List<string>();

        if (groupTripPlan.StartDate.HasValue && groupTripPlan.EndDate.HasValue)
        {
            DateTime currentDate = groupTripPlan.StartDate.Value;
            DateTime endDate = groupTripPlan.EndDate.Value;

            while (currentDate <= endDate)
            {
                dateInPlanList.Add(currentDate.ToString("dd/MM/yyyy"));
                currentDate = currentDate.AddDays(1); // Tăng ngày lên 1
            }
        }

        string[] dateInPlan = dateInPlanList.ToArray();

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

    [HttpGet("~/api/plan-user/detail/{id}")]
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

        var dateInPlanList = new List<string>();

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

        int maxDays =
            (groupTripPlan.EndDate - groupTripPlan.StartDate)?.Days + 1 ??
            0;
        PlanDetailDto[][] placeList = new PlanDetailDto[maxDays][];

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
                placeList[dayIndex] ??= new PlanDetailDto[] { };
                placeList[dayIndex] = placeList[dayIndex].Append(new PlanDetailDto
                {
                    Name = item.Name,
                    Id = item._Id,
                    Photos = item.Photos,
                    HasExperienced = item.HasExperienced ?? false,
                    TimeActive = item.TimeActive,
                    TimeOpen = item.TimeOpen,
                    TimeClose = item.TimeClose,
                    Url = item.Url,
                    International_phone_number = item.International_phone_number,
                    Formatted_address = item.Formatted_address,
                    TitleNote = item.TitleNote,
                    DescribeNote = item.DescribeNote,
                    Rating = item.Rating,
                    Distance = item.Distance,
                    Duration = item.Duration,
                    Price = item.Price,
                    UserRatingsTotal = item.UserRatingsTotal,
                    PlaceId = item.PlaceId
                }).ToArray();
            }
        }

        return Ok(new PlanDetailDtoResponse
        {
            PlanDetail = placeList,
            DateInPlan = dateInPlan,
            Departure = groupTripPlan.Departure,
            Destination = groupTripPlan.Destination
        });
    }


    [HttpPut("~/api/plan-user/detail/take-note/{id}")]
    public async Task<IActionResult> TakeNoteAsync(string id, [FromBody] TakeNoteDto data)
    {
        var detail = await _detailTripPlanService.GetById(id);
        if (detail == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đại điểm này"
            });
        }

        detail.TitleNote = data.TitleNote;
        detail.DescribeNote = data.DescribeNote;

        await _detailTripPlanService.Update(detail._Id, detail);
        var groupTripPlan = await _groupTripPlanService.GetById(detail.GroupTripPlanId);
        return Ok(new PlanResponse
        {
            Id = groupTripPlan._Id
        });
    }

    [HttpPut("~/api/plan-user/detail/edit-price/{id}")]
    public async Task<IActionResult> EditPriceAsync(string id, [FromBody] EditPricePlanDto data)
    {
        var detail = await _detailTripPlanService.GetById(id);
        if (detail == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy đại điểm này"
            });
        }

        detail.HasExperienced = data.HasExperienced;
        detail.Price = data.Price;

        await _detailTripPlanService.Update(detail._Id, detail);
        var groupTripPlan = await _groupTripPlanService.GetById(detail.GroupTripPlanId);
        return Ok(new PlanResponse
        {
            Id = groupTripPlan._Id
        });
    }

    [HttpGet("~/api/plan-user/detail/change/{id}")]
    public async Task<IActionResult> GetDetailToEditAsync(string id)
    {
        var groupTripPlan = await _groupTripPlanService.GetById(id);
        if (groupTripPlan == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy lịch trình"
            });
        }

        var dateInPlanList = new List<string>();

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

        int maxDays =
            (groupTripPlan.EndDate - groupTripPlan.StartDate)?.Days + 1 ??
            0;
        PlanDetailChangeDto[][] placeListAM = new PlanDetailChangeDto[maxDays][];
        PlanDetailChangeDto[][] placeListPM = new PlanDetailChangeDto[maxDays][];

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
            if (item.TimeActive.Contains("am"))
            {
                int dayIndex = item.NumberDay - 1;

                if (dayIndex >= 0 && dayIndex < maxDays)
                {
                    placeListAM[dayIndex] ??= new PlanDetailChangeDto[] { };
                    placeListAM[dayIndex] = placeListAM[dayIndex].Append(new PlanDetailChangeDto
                    {
                        Name = item.Name,
                        Id = item._Id,
                        Photos = item.Photos,
                        TimeActive = item.TimeActive,
                        TimeOpen = item.TimeOpen,
                        TimeClose = item.TimeClose,
                        Url = item.Url,
                        International_phone_number = item.International_phone_number,
                        Formatted_address = item.Formatted_address,
                        TitleNote = item.TitleNote,
                        DescribeNote = item.DescribeNote,
                        Rating = item.Rating,
                        Distance = item.Distance,
                        Duration = item.Duration,
                        Price = item.Price,
                        UserRatingsTotal = item.UserRatingsTotal,
                        PlaceId = item.PlaceId,
                        GroupTripPlanId = item.GroupTripPlanId,
                        HasExperienced = item.HasExperienced,
                        NumberDay = item.NumberDay,
                        Lat = item.Lat,
                        Lng = item.Lng
                    }).ToArray();
                }
            }
            else if (item.TimeActive.Contains("pm"))
            {
                int dayIndex = item.NumberDay - 1;

                if (dayIndex >= 0 && dayIndex < maxDays)
                {
                    placeListPM[dayIndex] ??= new PlanDetailChangeDto[] { };
                    placeListPM[dayIndex] = placeListPM[dayIndex].Append(new PlanDetailChangeDto
                    {
                        Name = item.Name,
                        Id = item._Id,
                        Photos = item.Photos,
                        TimeActive = item.TimeActive,
                        TimeOpen = item.TimeOpen,
                        TimeClose = item.TimeClose,
                        Url = item.Url,
                        International_phone_number = item.International_phone_number,
                        Formatted_address = item.Formatted_address,
                        TitleNote = item.TitleNote,
                        DescribeNote = item.DescribeNote,
                        Rating = item.Rating,
                        Distance = item.Distance,
                        Duration = item.Duration,
                        Price = item.Price,
                        UserRatingsTotal = item.UserRatingsTotal,
                        PlaceId = item.PlaceId,
                        GroupTripPlanId = item.GroupTripPlanId,
                        HasExperienced = item.HasExperienced,
                        NumberDay = item.NumberDay,
                        Lat = item.Lat,
                        Lng = item.Lng
                    }).ToArray();
                }
            }
        }

        return Ok(new PlanDetailChangeDtoResponse
        {
            PlanDetailAM = placeListAM,
            DateInPlan = dateInPlan,
            PlanDetailPM = placeListPM,
        });
    }
}