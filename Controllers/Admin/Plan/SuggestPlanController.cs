using brandportal_dotnet.Contracts.PageBanner;
using brandportal_dotnet.Contracts.Plan.SuggestPlan;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IPageBanner;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace brandportal_dotnet.Controllers.Plan
{
    [Route("[controller]")]
    [ApiController]
    public class SuggestPlanController
    {
        private const int _landingPageId = 7;
        private readonly IRepository<SuggestPlan> _suggestPlanService;

        public SuggestPlanController(
            IRepository<SuggestPlan> suggestPlanService
        )
        {
            _suggestPlanService = suggestPlanService;
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
                .Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<SuggestPlanDto>(total, items);
        }
    }
}

