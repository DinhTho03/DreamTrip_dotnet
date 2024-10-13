using brandportal_dotnet.Contracts.PageBanner;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService.IPageBanner;
using brandportal_dotnet.Models;
using brandportal_dotnet.Service.PageBanner;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using brandportal_dotnet.shared;

namespace brandportal_dotnet.Controllers.PageBanners
{
    [Route("[controller]")]
    [ApiController]
    public class PageBannerGroupController : ControllerBase
    {
        private const int _landingPageId = 4;
        private readonly IPageBannerRepository<Page> _pageService;
        private readonly IPageBannerRepository<PageBanner> _pageBannerService;
        private readonly IPageBannerRepository<PageCard> _pageCardService;

        public PageBannerGroupController(
            IPageBannerRepository<Page> pageService,
            IPageBannerRepository<PageBanner> pageBannerService,
            IPageBannerRepository<PageCard> pageCardService
        )
        {
            _pageService = pageService;
            _pageBannerService = pageBannerService;
            _pageCardService = pageCardService;
        }

        /// <summary>
        /// Danh sách trang
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("~/api/pages/paged")]
        public async Task<PagedResultDto<PageDto>> GetListPagedAsync(PagingRequestInput input)
        {
            var query = (await _pageService.GetAll()).Select(
                page => new PageDto
                {
                    Id = page._Id,
                    Type = page.Type,
                    PageName = page.PageName,
                    PageOrder = page.PageOrder,
                    UpdatedAt = page.UpdatedAt,
                }).Where(input);

            var items = query
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();
            var pageIds = items.Select(x => x.Id);

            var banners =
                (from pageCard in await _pageCardService.GetAll()
                    where pageCard.IsDeleted == false
                    select new
                    {
                        PageImg = pageCard.PageCardImage,
                        PageId = _landingPageId.ToString(), // Convert to string
                    })
                .Union(
                    from pageBanner in await _pageBannerService.GetAll()
                    where pageBanner.IsDeleted == false && pageIds.Contains(pageBanner.PageId ?? "")
                    select new
                    {
                        PageImg = pageBanner.PageImg,
                        PageId = pageBanner.PageId ?? "",
                    }
                );


            foreach (var item in items)
            {
                item.Banner = banners.Where(x => x.PageId == item.Id).Select(x => x.PageImg);
            }

            return new PagedResultDto<PageDto>(total, items);
        }
    }
}