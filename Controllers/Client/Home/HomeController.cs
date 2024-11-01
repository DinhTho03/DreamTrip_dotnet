using brandportal_dotnet.Contracts.User.Home;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService.IPageBanner;
using Google.Api.Gax;
using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.Controllers.Client.Home;

[Route("[controller]")]
[ApiController]
public class HomeController
{
    private readonly IPageBannerRepository<PageBanner> _pageBannerService;
    private readonly IPageBannerRepository<Page> _pageService;

    public HomeController(
        IPageBannerRepository<PageBanner> pageBannerService,
        IPageBannerRepository<Page> pageService
    )
    {
        _pageBannerService = pageBannerService;
        _pageService = pageService;
    }
    
    [HttpGet("~/api/home/banner")]
    public async Task<List<ListBannerDto>> GetListBannerAsync()
    {
        
        // Banner nhiệm vụ nổi bật
        var page = await _pageService.FindByProperties(new Dictionary<string, object> { { "Type", "slide_CTA" } });
        var banners = await _pageBannerService.GetAll();
        var data = from banner in banners
            where banner.IsActive == true && banner.PageId == page._Id
                select new ListBannerDto
                {
                    Id = banner._Id,
                    PageTitle = banner.PageTitle,
                    Image = banner.PageImg,
                    ActionName = banner.ActionName,
                    EndpointId = banner.EndpointId,
                    PageOrder = banner.PageOrder,
                };
        return data.OrderBy(x => x.PageOrder).Take(5).ToList();
    }
}