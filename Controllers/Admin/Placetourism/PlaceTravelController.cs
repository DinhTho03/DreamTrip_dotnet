
using brandportal_dotnet.Contracts.PlaceTourism.PlaceTravel;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using brandportal_dotnet.shared;

namespace brandportal_dotnet.Controllers.Admin.Placetourism;

public class PlaceTravelController : ControllerBase
{
    private readonly IRepository<PlaceTourismGroup> _placeTourismGroupService;
    private readonly IRepository<PlaceTourismCategory> _placeTourismCategoryService;
    private readonly IRepository<PlaceTourism> _tourismService;

    public PlaceTravelController(
        IRepository<PlaceTourismGroup> placeTourismGroupService,
        IRepository<PlaceTourismCategory> placeTourismCategoryService
        , IRepository<PlaceTourism> tourismService
    )
    {
        _placeTourismGroupService = placeTourismGroupService;
        _placeTourismCategoryService = placeTourismCategoryService;
        _tourismService = tourismService;
    }


    [HttpGet("~/api/place-travel/paged")]
    public async Task<PagedResultDto<PlaceTravelDto>> GetListPlaceTourismAsync(PagingRequestInput input)
    {
        var query = (from placeTourism in await _tourismService.GetAll()
            join placeTourismGroup in await _placeTourismGroupService.GetAll() on placeTourism
                .PlaceTourismGroupId equals placeTourismGroup._Id
            where placeTourism.IsDeleted == false
            select new PlaceTravelDto
            {
                Id = placeTourism._Id,
                Name = placeTourism.Name,
                CreatedAt = placeTourism.CreatedAt,
                Description = placeTourism.Description,
                Latitude = placeTourism.Latitude,
                Longitude = placeTourism.Longitude,
                ClosingTime = placeTourism.ClosingTime,
                OpeningTime = placeTourism.OpeningTime,
                PlaceTourismGroupId = placeTourism.PlaceTourismGroupId,
                PlaceTourismGroupName = placeTourismGroup.Name,
                Location = placeTourism.Location,
                MaxEntryFee = placeTourism.MaxEntryFee,
                MinEntryFee = placeTourism.MinEntryFee,
                Rating = placeTourism.Rating,
                IsActive = placeTourism.IsActive
            }).Where(input).OrderByDescending(x => x.CreatedAt);

        var data = query
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<PlaceTravelDto>(total, data);
    }
    
    [HttpGet("~/api/place-travel/{id}")]
    public async Task<PlaceTravelDto> GetPlaceTourismAsync(string id)
    {
        var queryPlaceGroupAsync = await _tourismService.GetById(id);
        var query = new PlaceTravelDto
        {
            Id = id,
            Name = queryPlaceGroupAsync.Name,
            Description = queryPlaceGroupAsync.Description,
            IsActive = queryPlaceGroupAsync.IsActive,
            CreatedAt = queryPlaceGroupAsync.CreatedAt,
            Latitude = queryPlaceGroupAsync.Latitude,
            Longitude = queryPlaceGroupAsync.Longitude,
            ClosingTime = queryPlaceGroupAsync.ClosingTime,
            OpeningTime = queryPlaceGroupAsync.OpeningTime,
            PlaceTourismGroupId = queryPlaceGroupAsync.PlaceTourismGroupId,
            PlaceTourismGroupName = "",
            Location = queryPlaceGroupAsync.Location,
            MaxEntryFee = queryPlaceGroupAsync.MaxEntryFee,
            MinEntryFee = queryPlaceGroupAsync.MinEntryFee,
            Rating = queryPlaceGroupAsync.Rating
        };
        return query;
    }
    
    [HttpPost("~/api/place-travel")]
    public async Task<IActionResult> AddPlaceTourismAsync([FromBody] PlaceTravelDto input)
    {
        var now = DateTime.UtcNow;
        var placeTourism = new PlaceTourism
        {
           Name = input.Name,
           Description = input.Description,
           IsActive = input.IsActive,
           CreatedAt = DateTime.Now,
           ClosingTime = input.ClosingTime,
           OpeningTime = input.OpeningTime,
           UpdatedAt = DateTime.Now,
           Latitude = input.Latitude,
           Longitude = input.Longitude,
           Location = input.Location,
           IsDeleted = false,
           MaxEntryFee = 0,
           MinEntryFee = 0,
           Rating = 5,
           PlaceTourismGroupId = input.PlaceTourismGroupId
           
        };

        await _tourismService.Insert(placeTourism);
        var result = new PlaceTravelDto
        {
            Id = placeTourism._Id
        };
        return Ok(result);
    }
    
    [HttpPut("~/api/place-travel/{id}")]
    public async Task<IActionResult> UpdatePlaceTourismAsync(string id,[FromBody] PlaceTravelDto input)
    {
        var now = DateTime.UtcNow;
        var placeTourism = await _tourismService.GetById(id);
        if (placeTourism == null)
        {
            return BadRequest(new
            {
                message = "Địa điểm này không tồn tại"
            });
        }

        placeTourism.Name = input.Name;
        placeTourism.Description = input.Description;
        placeTourism.Location = input.Location;
        placeTourism.PlaceTourismGroupId = input.PlaceTourismGroupId;
        placeTourism.Latitude = input.Latitude;
        placeTourism.Longitude = input.Longitude;
        placeTourism.OpeningTime = input.OpeningTime;
        placeTourism.ClosingTime = input.ClosingTime;
        placeTourism.IsActive = input.IsActive;
        placeTourism.UpdatedAt = now;
        
        await _tourismService.Update(id, placeTourism);
        return Ok();
    }
    
    
    [HttpGet("~/api/place-travel/paged/filter-place-group")]
    public async Task<List<LookupDto>> GetListPlaceTourismAsync()
    {
        var query = from a in await _placeTourismGroupService.GetAll()
            select new LookupDto
            {
                Id = a._Id,
                Name = a.Name,
                Code = a.Name,
            };

        return query.ToList();
    }
}