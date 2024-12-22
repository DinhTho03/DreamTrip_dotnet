using brandportal_dotnet.Contracts.PlaceTourism.PlaceTourismGroup;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Volo.Abp.Application.Dtos;

namespace brandportal_dotnet.Controllers.Admin.Placetourism;

[Route("[controller]")]
[ApiController]
public class PlaceTourismController : ControllerBase
{
    private readonly IRepository<PlaceTourismGroup> _placeTourismGroupService;
    private readonly IRepository<PlaceTourismCategory> _placeTourismCategoryService;

    public PlaceTourismController(IRepository<PlaceTourismGroup> placeTourismGroupService,
        IRepository<PlaceTourismCategory> placeTourismCategoryService)
    {
        _placeTourismGroupService = placeTourismGroupService;
        _placeTourismCategoryService = placeTourismCategoryService;
    }

    [HttpGet("~/api/place-tourism/paged")]
    public async Task<PagedResultDto<PagePlaceTourismDto>> GetListPlaceTourismAsync(PagingRequestInput input)
    {
        var query = (from placeTourismGroup in await _placeTourismGroupService.GetAll()
            join placeTourismCategory in await _placeTourismCategoryService.GetAll() on placeTourismGroup
                .PlaceTourismCateId equals placeTourismCategory._Id
            where placeTourismGroup.IsDeleted == false
            select new PagePlaceTourismDto
            {
                Id = placeTourismGroup._Id,
                Name = placeTourismGroup.Name,
                Type = placeTourismCategory.Name,
                IsActived = placeTourismGroup.IsActive,
                CreatedAt = placeTourismGroup.CreatedAt
            }).Where(input).OrderByDescending(x => x.CreatedAt);

        var data = query
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<PagePlaceTourismDto>(total, data);
    }

    [HttpPut("~/api/place-tourism/{id}/status")]
    public async Task<IActionResult> GetStatusPlaceTourismAsync(string id, [FromBody] bool IsActive)
    {
        var placeTourismGroup = await _placeTourismGroupService.GetById(id);
        if (placeTourismGroup == null)
        {
            return BadRequest(new
            {
                message = "Nhóm địa điểm này không tồn tại"
            });
        }

        placeTourismGroup.IsActive = IsActive;
        await _placeTourismGroupService.Update(id, placeTourismGroup);
        return Ok();
    }

    [HttpGet("~/api/place-tourism/{id}")]
    public async Task<PlaceTourismDetailDto> GetPlaceTourismGroupAsync(string id)
    {
        var queryPlaceGroupAsync = await _placeTourismGroupService.GetById(id);
        var query = new PlaceTourismDetailDto
        {
            Id = id,
            Name = queryPlaceGroupAsync.Name,
            Description = queryPlaceGroupAsync.Description,
            IsActive = queryPlaceGroupAsync.IsActive,
            PlaceTourismCateId = queryPlaceGroupAsync.PlaceTourismCateId
        };
        return query;
    }
    
    [HttpGet("~/api/place-tourism/paged/filter-place-tourism-title")]
    public async Task<List<LookupDto>> GetListPlaceTourismGroupAsync()
    {
        var query = from a in await _placeTourismCategoryService.GetAll()
            select new LookupDto
            {
                Id = a._Id,
                Name = a.Name,
                Code = a.Name,
            };

        return query.ToList();
    }
    
    [HttpPost("~/api/place-tourism")]
    public async Task<IActionResult> AddPlaceTourismAsync(PagePlaceTourismCreateDto input)
    {
        var now = DateTime.UtcNow;
        var placeTourismGroup = new PlaceTourismGroup
        {
            Name = input.Name,
            Description = input.Description,
            IsActive = input.IsActive,
            PlaceTourismCateId = input.PlaceTourismCateId,
            CreatedAt = now,
            UpdatedAt = now,
            UpdatedBy = 1
        };

        await _placeTourismGroupService.Insert(placeTourismGroup);
        var result = new PagePlaceTourismDto
        {
            Id = placeTourismGroup._Id
        };
        return Ok(result);
    }
    
    [HttpPut("~/api/place-tourism/{id}")]
    public async Task<IActionResult> UpdatePlaceTourismAsync(string id, PagePlaceTourismCreateDto input)
    {
        var now = DateTime.UtcNow;
        var placeTourismGroup = await _placeTourismGroupService.GetById(id);
        if (placeTourismGroup == null)
        {
            return BadRequest(new
            {
                message = "Nhóm địa điểm này không tồn tại"
            });
        }

        placeTourismGroup.Name = input.Name;
        placeTourismGroup.Description = input.Description;
        placeTourismGroup.IsActive = input.IsActive;
        placeTourismGroup.PlaceTourismCateId = input.PlaceTourismCateId;
        placeTourismGroup.UpdatedAt = now;
        await _placeTourismGroupService.Update(id, placeTourismGroup);
        return Ok();
    }
}