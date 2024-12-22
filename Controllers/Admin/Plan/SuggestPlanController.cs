using brandportal_dotnet.Contracts.PageBanner;
using brandportal_dotnet.Contracts.Plan.SuggestPlan;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IPageBanner;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;
using Volo.Abp.Application.Dtos;

namespace brandportal_dotnet.Controllers.Plan
{
    [Route("[controller]")]
    [ApiController]
    public class SuggestPlanController : ControllerBase
    {
        private const int _landingPageId = 7;
        private readonly IRepository<SuggestPlan> _suggestPlanService;
        private readonly IRepository<ActivityPlan> _activityPlanService;

        public SuggestPlanController(
            IRepository<SuggestPlan> suggestPlanService,
            IRepository<ActivityPlan> activityPlanService
        )
        {
            _suggestPlanService = suggestPlanService;
            _activityPlanService = activityPlanService;
        }

        /// <summary>
        /// Danh sách trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("~/api/plan/suggest/paged")]
        public async Task<PagedResultDto<SuggestPlanDto>> GetListPagedAsync(PagingRequestInput input)
        {
            var query = (await _suggestPlanService.GetAll()).Select(
                page => new SuggestPlanDto
                {
                    Id = page._Id,
                    Type = page.Type,
                    Name = page.Name,
                    Order = page.Order,
                    UpdatedAt = page.UpdatedAt,
                }).Where(input);

            var items = query
                .OrderBy(x => x.Order)
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<SuggestPlanDto>(total, items);
        }

        [HttpGet("~/api/plan/suggest/{pageId}/pages")]
        public async Task<PagedResultDto<ActivityPlanDto>> ListActivityPlan(string pageId, PagingRequestInput input)
        {
            var query = (await _activityPlanService.GetAll()).Where(x => x.SuggestPlanId == pageId).Select(
                page => new ActivityPlanDto
                {
                    Name = page.Name,
                    IsActive = page.IsActive,
                    CreateAt = page.CreateAt,
                    Id = page._Id,
                    SuggestPlanId = page.SuggestPlanId
                }).Where(input);

            var items = query
                .OrderByDescending(x => x.CreateAt)
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<ActivityPlanDto>(total, items);
        }

        [HttpGet("~/api/plan/suggest/activity-plan/pages/{id}")]
        public async Task<IActionResult> GetActivityPlan(string id)
        {
            var query = await _activityPlanService.GetById(id);
            if (query == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy hoạt động"
                });
            }

            var data = new DetailActivityPlanDto
            {
                Id = query._Id,
                Name = query.Name,
                SuggestPlanId = query.SuggestPlanId,
                IsActive = query.IsActive
            };
            return Ok(data);
        }

        [HttpPost("~/api/plan/suggest/activity-plan/pages")]
        public async Task<IActionResult> CreateActivityPlan(DetailActivityPlanDto detailActivityPlanDto)
        {
            var query = await _activityPlanService.GetAll();
            var filter = query.Where(x => string.Equals(x.Name, detailActivityPlanDto.Name, StringComparison.OrdinalIgnoreCase) && x.SuggestPlanId == detailActivityPlanDto.SuggestPlanId)!.FirstOrDefault();
            if (filter != null)
            {
                if (filter.Name == detailActivityPlanDto.Name)
                {
                    return BadRequest(new
                    {
                        message = "Tên đã tồn tại"
                    });
                }
            }

            var activityPlan = new ActivityPlan
            {
                Name = detailActivityPlanDto.Name,
                SuggestPlanId = detailActivityPlanDto.SuggestPlanId,
                IsActive = detailActivityPlanDto.IsActive,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                CreateBy = "1",
                UpdateBy = "1"
            };

            var data = await _activityPlanService.Insert(activityPlan, true);
            return Ok(new
            {
                Id = data._Id,
                SuggestPlanId = data.SuggestPlanId
            });
        }

        [HttpPut("~/api/plan/suggest/activity-plan/pages/{id}")]
        public async Task<IActionResult> UpdateActivityPlan(string id, DetailActivityPlanDto detailActivityPlanDto)
        {
            var detail = await _activityPlanService.GetById(id);
            var query = await _activityPlanService.GetAll();
            var filter = query.Where(x => string.Equals(x.Name, detailActivityPlanDto.Name, StringComparison.OrdinalIgnoreCase) && x._Id != id && x.SuggestPlanId == detailActivityPlanDto.SuggestPlanId)!.FirstOrDefault();
            if (filter != null)
            {
                if (filter.Name == detailActivityPlanDto.Name)
                {
                    return BadRequest(new
                    {
                        message = "Tên đã tồn tại"
                    });
                }
            }

            detail.Name = detailActivityPlanDto.Name;
            detail.IsActive = detailActivityPlanDto.IsActive;
            detail.UpdateAt = DateTime.Now;
            detail.UpdateBy = "1";
            await _activityPlanService.Update(id, detail);
            return Ok(new
            {
                Id = detail._Id,
                SuggestPlanId = detail.SuggestPlanId
            });
        }

        [HttpDelete("~/api/plan/suggest/activity-plan/pages/{id}")]
        public async Task<IActionResult> DeleteActivityPlan(string id)
        {
            var query = await _activityPlanService.GetById(id);
            if (query == null)
            {
                return NotFound(new
                {
                    message = "Không tìm thấy hoạt động"
                });
            }

            await _activityPlanService.Delete(id);
            return Ok();
        }
        
        [HttpPut("~/api/plan/suggest/activity-plan/pages/{id}/status")]
        public async Task<IActionResult> UpdateStatusFaqAsync(string id, [FromBody] bool isActive)
        {
            var detail = await _activityPlanService.GetById(id);
            if (detail == null)
            {
                return BadRequest(new { message = "Hoạt động này không tồn tại" });
            }

            detail.IsActive = isActive;
            await _activityPlanService.Update(id, detail);
            return Ok();
        }
    }
}