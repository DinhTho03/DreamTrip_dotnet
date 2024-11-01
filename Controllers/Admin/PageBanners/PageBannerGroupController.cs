using brandportal_dotnet.Contracts.PageBanner;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IPageBanner;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using brandportal_dotnet.shared;
using Volo.Abp;
using Volo.Abp.Domain.Entities;

namespace brandportal_dotnet.Controllers.PageBanners
{
    [Route("[controller]")]
    [ApiController]
    public class PageBannerGroupController : ControllerBase
    {
        private const string _CTA = "636460b82884711f5decfeb4";
        private const string _landingPageId = "636460b82884711f5decfeb3";
        private readonly IPageBannerRepository<Page> _pageService;
        private readonly IPageBannerRepository<PageBanner> _pageBannerService;
        private readonly IPageBannerRepository<PageCard> _pageCardService;
        private readonly IRepository<NotificationPage> _notificationService;
        private readonly IRepository<Game> _gameService;
        private readonly IRepository<GameCategory> _gameCategoryService;
        private readonly IRepository<EndpointPage> _endpointPageService;

        public PageBannerGroupController(
            IPageBannerRepository<Page> pageService,
            IPageBannerRepository<PageBanner> pageBannerService,
            IPageBannerRepository<PageCard> pageCardService,
            IRepository<NotificationPage> notificationService,
            IRepository<Game> gameService,
            IRepository<GameCategory> gameCategoryService,
            IRepository<EndpointPage> endpointPageService
        )
        {
            _pageService = pageService;
            _pageBannerService = pageBannerService;
            _pageCardService = pageCardService;
            _notificationService = notificationService;
            _gameService = gameService;
            _gameCategoryService = gameCategoryService;
            _gameCategoryService = gameCategoryService;
            _endpointPageService = endpointPageService;
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
                        PageId = _landingPageId,
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

        [HttpGet("~/api/pages/{pageId}/banners")]
        public async Task<PagedResultDto<PageBannerDto>> GetListByPageType(string pageId, PagingRequestInput input)
        {
            IEnumerable<PageBannerDto> query;

            if (pageId != _landingPageId)
            {
                query = (from pageBanner in await _pageBannerService.GetAll()
                    join endpoint in await _endpointPageService.GetAll() on pageBanner.EndpointId equals endpoint
                        .EndPointId
                    join noti in await _notificationService.GetAll() on pageBanner.Action equals noti.Type
                    where pageBanner.IsDeleted != true && pageBanner.PageId == pageId
                    group new
                    {
                        pageBanner,
                        endpoint,
                        noti
                    } by pageBanner._Id into g
                    select new PageBannerDto
                    {
                        Id = g.Max(x => x.pageBanner._Id),
                        Order = g.Max(x => x.pageBanner.PageOrder),
                        Image = g.Max(x => x.pageBanner.PageImg) ,
                        Title =  g.Max(x => x.pageBanner.PageTitle),
                        Action =  g.Max(x => x.noti.Title),
                        ActionParams =  g.Max(x => x.endpoint.Name),
                        UpdatedAt =  g.Max(x => x.pageBanner.UpdatedAt)
                    }).Where(input);
                query = query
                    .OrderBy(x => x.Order)
                    .OrderBy(input)
                    .Page(input).ToList();
                // .Where(x => x.PageId == pageId && x.IsDeleted != true)
                // .OrderBy(x => x.PageOrder)
                // .Select(x => new PageBannerDto
                // {
                //     Id = x._Id,
                //     Order = x.PageOrder,
                //     Image = x.PageImg,
                //     Title = x.PageTitle,
                //     Action = x.Action,
                //     ActionParams = x.ActionParams,
                //     UpdatedAt = x.UpdatedAt
                // }).Where(input);
            }
            else
            {
                query = (await _pageCardService.GetAll())
                    .Where(x => x.IsDeleted != true)
                    .OrderBy(x => x.PageCardPosition)
                    .Select(x => new PageBannerDto
                    {
                        Id = x._Id,
                        Order = x.PageCardPosition,
                        Image = x.PageCardImage,
                        Title = x.PageCardTitle,
                        Action = x.Action,
                        ActionParams = x.ActionParams,
                        UpdatedAt = x.UpdatedAt
                    }).Where(input);
            }

            var banners = query.OrderBy(input).Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<PageBannerDto>(total, banners);
        }


        /// <summary>
        /// Get chi tiết banner/card
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="bannerId"></param>
        /// <returns></returns>
        [HttpGet("~/api/pages/{pageId}/banners/{bannerId}")]
        public async Task<PageBannerDetailDto> GetBannerDetailAsync(string pageId, string bannerId)
        {
            var query = pageId != _landingPageId
                ? (await _pageBannerService.GetAll())
                .Select(x => new PageBannerDetailDto
                {
                    Id = x._Id,
                    Tagline = x.PageTagline,
                    Image = x.PageImg,
                    Title = x.PageTitle,
                    ActionName = x.ActionName,
                    ActionParams = x.ActionParams,
                    EndPoint = x.Action,
                    AllowAllOutlet = x.AllowAllOutlet,
                    UpdatedAt = x.UpdatedAt,
                    EndPointId = x.EndpointId,
                    IsActive = x.IsActive,
                    StartEffectiveDate = x.StartEffectiveDate,
                    EndEffectiveDate = x.EndEffectiveDate,
                    Position = x.PageOrder,
                })
                : (await _pageCardService.GetAll())
                .Select(x => new PageBannerDetailDto
                {
                    Id = x._Id,
                    Tagline = x.PageCardTagline,
                    Image = x.PageCardImage,
                    Title = x.PageCardTitle,
                    ActionParams = x.ActionParams,
                    EndPoint = x.Action,
                    AllowAllOutlet = x.AllowAllOutlet + 1 - 1, // TODO: fix tinyint issue
                    UpdatedAt = x.UpdatedAt,
                    IsActive = x.IsActive,
                    StartEffectiveDate = x.StartEffectiveDate,
                    EndEffectiveDate = x.EndEffectiveDate,
                    Position = x.PageCardPosition,
                });

            var banner = query.Where(x => x.Id == bannerId)?.FirstOrDefault() ??
                         throw new EntityNotFoundException(typeof(PageCard), bannerId);
            var endpointCondition = new Dictionary<string, object>
            {
                { "EndPointId", banner.EndPointId },
                { "Type", banner.EndPoint }
            };

            var endpointData = await _endpointPageService.FindByProperties(endpointCondition);

            banner.EndPointName = endpointData.Name;

            // banner.Position = (pageId == _landingPageId
            //     ? await _pageCardService.CountAsync(x => x.IsDeleted != true && x.PageCardPosition < banner.Position)
            //     : await _bannerRepository.CountAsync(x =>
            //         x.IsDeleted != true && x.PageId == pageId && x.PageOrder < banner.Position)) + 1;

            return banner;
        }

        [HttpGet("~/api/pages/notification-types/{pageId}")]
        public async Task<List<LookupDto>> GetNotificationTypes(string pageId)
        {
            var query = (await _notificationService.GetAll())
                .Where(x => x.PageId == pageId)
                .Select(x => new LookupDto
                {
                    Id = x._Id,
                    Name = x.Title,
                    Code = x.Type
                }).ToList();
            return query;
        }

        /// <summary>
        /// Danh sách trang chi tiết game
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        [HttpGet("~/api/pages/game-all")]
        public async Task<IActionResult> GetListGamePageAsync(PagingRequestInput input)
        {
            var query = (from a in await _gameService.GetAll()
                where a.IsDeleted == false
                join b in await _gameCategoryService.GetAll() on a.CateId equals b._Id
                select new PageGameDto
                {
                    Id = a._Id,
                    Name = a.GameName,
                    Code = a.GameCode,
                    CateId = b._Id,
                    CateName = b.CateName,
                    IsActive = a.IsActive,
                    StartDate = a.StartDate,
                    EndDate = a.EndDate,
                    CreatedAt = a.CreatedAt,
                }).Where(input);

            var data = query
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();


            return Ok(new PagedResultDto<PageGameDto>(total, data));
        }

        /// <summary>
        /// Tạo mới banner/card
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="body"></param>
        /// <returns></returns>
        [HttpPost("~/api/pages/{pageId}/banners")]
        public async Task<IActionResult> CreateBannerAsync(string pageId, [FromBody] PageBannerCreateDto body)
        {
            var now = DateTime.Now;
            var endpointData = new EndpointPage
            {
                EndPointId = body.EndPointId,
                Name = body.EndPointName,
                Type = body.EndPoint,
                CreateAt = now
            };
            await _endpointPageService.Insert(endpointData);


            if (pageId == _landingPageId)
            {
                // var total = await _cardRepository.CountAsync(x => x.IsDeleted != true);
                // if (total >= 1000)
                // {
                //     throw new UserFriendlyException("Số lượng card đã đạt giới hạn tối đa (1000)");
                // }
                //
                // var lowestPosition = await _cardRepository.MaxAsync(x => x.PageCardPosition);
                // var item = new DmsproMysPageCard
                // {
                //     PageCardImage = body.Image,
                //     PageCardTagline = body.Tagline,
                //     PageCardTitle = body.Title,
                //     Action = body.EndPoint,
                //     AllowAllOutlet = body.AllowAllOutlet,
                //     ActionParams = jsonData,
                //     PageCardPosition = lowestPosition + 1,
                //     CreatedAt = now,
                //     CreatedBy = PortalUserId,
                //     UpdatedAt = now,
                //     UpdatedBy = PortalUserId,
                //     IsActive = body.IsActive,
                //     StartEffectiveDate = body.StartEffectiveDate,
                //     EndEffectiveDate = body.EndEffectiveDate
                // };
                //
                // await _cardRepository.InsertAsync(item, true);
                return Ok();
            }
            else
            {
                var total = await _pageBannerService.CountAsync(x => x.IsDeleted != true);
                if (total >= 1000)
                {
                    return BadRequest(new { message = "Số lượng banner đã đạt giới hạn tối đa (1000)" });
                }

                // var lowestPosition = await _pageBannerService.MaxAsync(x => x.PageOrder);
                var page = await _pageService.GetById(pageId);
                var lowestPosition = await _pageBannerService.CountAsync(x => x.IsDeleted != true);
                var item = new PageBanner()
                {
                    PageId = pageId,
                    PageType = page != null ? page.Type : string.Empty,
                    PageImg = body.Image,
                    PageTitle = body.Title,
                    ActionName = body.ActionName,
                    PageTagline = body.Tagline,
                    Action = body.EndPoint,
                    AllowAllOutlet = body.AllowAllOutlet,
                    ActionParams = "",
                    EndpointId = body.EndPointId,
                    PageOrder = lowestPosition + 1,
                    CreatedAt = now,
                    CreatedBy = 1,
                    UpdatedAt = now,
                    UpdatedBy = 1,
                    IsActive = body.IsActive,
                    StartEffectiveDate = body.StartEffectiveDate,
                    EndEffectiveDate = body.EndEffectiveDate
                };

                await _pageBannerService.Insert(item);
                return Ok(new BannerIdDto
                {
                    Id = item._Id
                });
            }
        }
        
        [HttpPut("~/api/pages/{pageId}/banners/{bannerId}/position")]
        public async Task<IActionResult> UpdateBannerPositionAsync(string pageId, string bannerId, [FromBody] int newPosition)
        {
            if (pageId == _landingPageId)
            {
                // await _cardRepository.UpdatePositionAsync(bannerId, newPosition);
            }
            else
            {
                var item = await _pageBannerService.GetById(bannerId);
                if (item == null)
                {
                    return BadRequest(new { message = $"Id '{bannerId}' không tồn tại" });
                }
                else
                {
                    int currentPosition = (int)item.PageOrder!;
                    int m = (newPosition - currentPosition) / Math.Abs(newPosition - currentPosition);
                    item.PageOrder = newPosition;
                   
                    var middleItems = await _pageBannerService.GetListAsync(x =>
                        m * x.PageOrder > m * currentPosition && m * x.PageOrder <= m * newPosition);
                    await _pageBannerService.Update(bannerId, item);
                    foreach (var middleItem in middleItems)
                    {
                        middleItem.PageOrder = middleItem.PageOrder - m;
                        await _pageBannerService.Update(middleItem._Id, middleItem);
                    }
                }
               
            }
            return Ok();
        }

        [HttpPut("~/api/pages/{pageId}/banners/{bannerId}")]
        public async Task<IActionResult> UpdateBannerAsync(string pageId, string bannerId,
            [FromBody] PageBannerCreateDto body)
        {
            var now = DateTime.Now;


            if (pageId == _landingPageId)
            {
                // var item = await AsyncExecuter.FirstOrDefaultAsync(
                //                (await _cardRepository.GetQueryableAsync())
                //                .Select(x => new DmsproMysPageCard
                //                {
                //                    Id = x.Id,
                //                    PageCardImage = x.PageCardImage,
                //                    PageCardTagline = x.PageCardTagline,
                //                    PageCardTitle = x.PageCardTitle,
                //                    Action = x.Action,
                //                    AllowAllOutlet = x.AllowAllOutlet + 1 - 1, // TODO: fix tinyint issue
                //                    ActionParams = x.ActionParams,
                //                    PageCardPosition = x.PageCardPosition,
                //                    CreatedAt = x.CreatedAt,
                //                    CreatedBy = x.CreatedBy,
                //                    UpdatedAt = x.UpdatedAt,
                //                    UpdatedBy = x.UpdatedBy,
                //                    IsDeleted = x.IsDeleted,
                //                    IsActive = x.IsActive,
                //                    StartEffectiveDate = x.StartEffectiveDate,
                //                    EndEffectiveDate = x.EndEffectiveDate
                //                }),
                //                x => x.Id == bannerId) ??
                //            throw new EntityNotFoundException(typeof(DmsproMysPageCard), bannerId);
                //
                // item.PageCardImage = body.Image;
                // item.PageCardTagline = body.Tagline;
                // item.PageCardTitle = body.Title;
                // item.Action = body.EndPoint;
                // item.AllowAllOutlet = body.AllowAllOutlet;
                // item.ActionParams = jsonData;
                // item.UpdatedAt = now;
                // item.UpdatedBy = PortalUserId;
                // item.IsActive = body.IsActive;
                // item.StartEffectiveDate = body.StartEffectiveDate;
                // item.EndEffectiveDate = body.EndEffectiveDate;
                //
                // await _cardRepository.UpdateAsync(item);
            }
            else
            {
                var item = await _pageBannerService.GetById(bannerId);
                if (item == null)
                {
                    return BadRequest(new { message = $"Id '{bannerId}' không tồn tại" });
                }

                var endpointCondition = new Dictionary<string, object>
                {
                    { "EndpointId", item.EndpointId }
                };
                var endpoint = await _endpointPageService.FindByProperties(endpointCondition);

                if (endpoint != null)
                {
                    var endpointData = new EndpointPage
                    {
                        EndPointId = body.EndPointId,
                        Name = body.EndPointName,
                        Type = body.EndPoint,
                        CreateAt = now
                    };
                    await _endpointPageService.Update(endpoint._Id, endpointData);
                }


                item.PageId = pageId;
                item.PageImg = body.Image;
                item.PageTitle = body.Title;
                item.PageTagline = body.Tagline;
                item.Action = body.EndPoint;
                item.ActionName = body.ActionName;
                item.AllowAllOutlet = body.AllowAllOutlet;
                item.ActionParams = "";
                item.UpdatedAt = now;
                item.UpdatedBy = 1;
                item.EndpointId = body.EndPointId;
                item.IsActive = body.IsActive;
                item.StartEffectiveDate = body.StartEffectiveDate;
                item.EndEffectiveDate = body.EndEffectiveDate;

                await _pageBannerService.Update(bannerId, item);
              
            }
            return Ok();
            // // remove all relationship banner and outlet folowing AllowAllOutlet value
            // if (pageId == _landingPageId)
            // {
            //     // switch (body.AllowAllOutlet)
            //     // {
            //     //     case 1:
            //     //         await _cardRepository.RemoveOutletAsync([bannerId]);
            //     //         await _cardRepository.DeleteCardOutletGroupAsync([bannerId]);
            //     //         break;
            //     //     case 0:
            //     //         await _cardRepository.DeleteCardOutletGroupAsync([bannerId]);
            //     //         break;
            //     // }
            // }
            // else if (body.AllowAllOutlet == 1)
            // {
            //     await _pageBannerService.([bannerId]);
            // }
        }
        
        /// <summary>
        /// Xóa banner/card
        /// </summary>
        /// <param name="pageId"></param>
        /// <param name="bannerIds"></param>
        /// <returns></returns>
        [HttpDelete("~/api/pages/{pageId}/banners")]
        public async Task DeleteBannerAsync(string pageId, [FromBody] string[] bannerIds)
        {
            if (pageId == _landingPageId)
            {
                // await _cardRepository.RemoveManyAsync(bannerIds);
            }
            else
            {
                await _pageBannerService.DeleteMany(pageId, bannerIds);
            }
        }
    }
}