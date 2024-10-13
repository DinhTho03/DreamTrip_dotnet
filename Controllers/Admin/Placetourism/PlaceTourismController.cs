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
public class PlaceTourismController: ControllerBase
{
      private readonly IRepository<PlaceTourismGroup> _placeTourismGroupService;
      private readonly IRepository<PlaceTourismCategory> _placeTourismCategoryService;

      public PlaceTourismController(IRepository<PlaceTourismGroup> placeTourismGroupService, IRepository<PlaceTourismCategory> placeTourismCategoryService)
      {
         _placeTourismGroupService = placeTourismGroupService;
         _placeTourismCategoryService = placeTourismCategoryService;
      }
   
      [HttpGet("~/api/placetourism/paged")]
      public async Task<PagedResultDto<PagePlaceTourismDto>> GetListPlaceTourismAsync(PagingRequestInput input)
      {
         var query = (from placeTourismGroup in await _placeTourismGroupService.GetAll()
                        join placeTourismCategory in await _placeTourismCategoryService.GetAll() on placeTourismGroup.PlaceTourismCateId equals placeTourismCategory._Id 
                        where placeTourismGroup.IsDeleted == false
                        select new PagePlaceTourismDto
                        {
                           Id = placeTourismGroup._Id,
                           Title = placeTourismGroup.Name,
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
   
      [HttpPut("~/api/placetourism/{id}/status")]
        public async Task<IActionResult> GetStatusPlaceTourismAsync(string id,[FromBody]  bool IsActive)
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
}