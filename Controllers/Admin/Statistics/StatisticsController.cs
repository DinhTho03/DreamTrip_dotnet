using brandportal_dotnet.Contracts.Statistics;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;

namespace brandportal_dotnet.Controllers.Admin.Statictis;

[Route("[controller]")]
[ApiController]
public class StatisticsController : ControllerBase
{
    private readonly IRepository<GroupTripPlan> _groupTripPlanService;
    private readonly IRepository<DetailTripPlan> _detailTripPlanService;
    private readonly IRepository<Account> _accountService;
    private readonly IRepository<Payment> _paymentService;


    public StatisticsController
    (
        IRepository<GroupTripPlan> groupTripPlanService,
        IRepository<Account> accountService,
        IRepository<DetailTripPlan> detailTripPlanService,
        IRepository<Payment> paymentService
    )
    {
        _groupTripPlanService = groupTripPlanService;
        _detailTripPlanService = detailTripPlanService;
        _accountService = accountService;
        _paymentService = paymentService;
    }

    [HttpGet("~/api/admin/statistics/paged")]
    public async Task<IActionResult> GetListPagedAsync()
    {
        var group = await _groupTripPlanService.GetAll();
        var detail = await _detailTripPlanService.GetAll();
        var data = new StatisticsDto()
        {
            GroupPlanCount = group.Count(),
            PlaceInPlanCount = detail.Count(),
            HasExperiencedCount = detail.Count(x => x.HasExperienced == true),
            HasExperienceCount = detail.Count(x => x.HasExperienced == false)
        };
        return Ok(data);
    }

    [HttpGet("~/api/admin/statistics/status/page")]
    public async Task<IActionResult> GetStatisticsStatusAsync([FromQuery] ForDateStatus input)
    {
        var group = await _groupTripPlanService.GetAll();
        var detail = await _detailTripPlanService.GetAll();
        var account = await _accountService.GetAll();
        IEnumerable<GroupTripPlan> groupPlanCount;
        if (input.StartDate == null || input.EndDate == null)
        {
            groupPlanCount = group;
            var HasExperiencedCount = detail.Count(x => x.HasExperienced == true);
            var HasExperienceCount = detail.Count(x => x.HasExperienced == false);
            var data = new StatisticStatusDto
            {
                GroupPlanCount = groupPlanCount.Count(),
                HasExperiencedCount = HasExperiencedCount,
                HasExperienceCount = HasExperienceCount,
                HasExperiencePercent = (HasExperienceCount * 100) / (HasExperienceCount + HasExperiencedCount),
                HasExperiencedPercent = 100 - ((HasExperienceCount * 100) / (HasExperienceCount + HasExperiencedCount)),
            };

            var statisticList = (from groupPlan in groupPlanCount
                join accountUser in account on groupPlan.UserId equals accountUser._Id
                select new StatisticListDto
                {
                    Id = groupPlan._Id,
                    UserName = accountUser.FullName,
                    Departure = groupPlan.Departure,
                    Destination = groupPlan.Destination,
                    StartDate = groupPlan.StartDate,
                    EndDate = groupPlan.EndDate,
                    CreateAt = groupPlan.CreatedAt
                }).OrderByDescending(x => x.CreateAt);


            data.StatisticList = statisticList.ToArray();

            return Ok(data);
        }
        else
        {
            groupPlanCount = group.Where(x => x.StartDate >= input.StartDate && x.EndDate <= input.EndDate);

            var HasExperiencedCount = (from groupPlan in groupPlanCount
                join detailPlan in detail on groupPlan._Id equals detailPlan.GroupTripPlanId
                where detailPlan.HasExperienced == true
                select detailPlan).Count();

            var HasExperienceCount = (from groupPlan in groupPlanCount
                join detailPlan in detail on groupPlan._Id equals detailPlan.GroupTripPlanId
                where detailPlan.HasExperienced == false
                select detailPlan).Count();

            StatisticStatusDto data;
            if (HasExperiencedCount == 0 && HasExperienceCount == 0)
            {
                data = new StatisticStatusDto
                {
                    GroupPlanCount = 0,
                    HasExperiencedCount = 0,
                    HasExperienceCount = 0,
                    HasExperiencePercent = 0,
                    HasExperiencedPercent = 0,
                };
            }
            else
            {
                data = new StatisticStatusDto
                {
                    GroupPlanCount = groupPlanCount.Count(),
                    HasExperiencedCount = HasExperiencedCount,
                    HasExperienceCount = HasExperienceCount,
                    HasExperiencePercent = (HasExperienceCount * 100) / (HasExperienceCount + HasExperiencedCount),
                    HasExperiencedPercent =
                        100 - ((HasExperienceCount * 100) / (HasExperienceCount + HasExperiencedCount)),
                };
            }


            var statisticList = (from groupPlan in groupPlanCount
                join accountUser in account on groupPlan.UserId equals accountUser._Id
                select new StatisticListDto
                {
                    Id = groupPlan._Id,
                    UserName = accountUser.FullName,
                    Departure = groupPlan.Departure,
                    Destination = groupPlan.Destination,
                    StartDate = groupPlan.StartDate,
                    EndDate = groupPlan.EndDate,
                    CreateAt = groupPlan.CreatedAt
                }).OrderByDescending(x => x.CreateAt);

            data.StatisticList = statisticList.ToArray();

            return Ok(data);
        }
    }


    [HttpGet("~/api/admin/statistics/public-plan/page")]
    public async Task<IActionResult> GetStatisticsPublicPlanAsync([FromQuery] ForDateStatus input)
    {
        var group = await _groupTripPlanService.GetAll();
        var StatisticsPublicPlan = new StatisticsPublicPlanDto()
        {
            TotalPublicPlan = group.Where(x => x.IsPublic == true).Count(),
            TotalCreatedPlan = group.Where(x => x.UserExperienceId != null).Count(),
            TotalBean = group.Where(x => x.UserExperienceId != null).Count() * 5,
            TotalInterestRate = (group.Where(x => x.UserExperienceId != null).Count() * 5) -
                                (group.Where(x => x.UserExperienceId != null).Count() * 3)
        };
        IEnumerable<GroupTripPlan>? groupPlanCount;
        if (input.StartDate == null && input.EndDate == null)
        {
            groupPlanCount = group.Where(x => x.UserExperienceId != null);
        }
        else
        {
            groupPlanCount = group.Where(x =>
                x.CreatedAt >= input.StartDate && x.CreatedAt <= input.EndDate && x.UserExperienceId != null);
        }


        var groupPublicPlan = groupPlanCount
            .GroupBy(groupPublic => groupPublic.CreatedAt.HasValue
                ? groupPublic.CreatedAt.Value.ToString("dd/MM/yyyy")
                : "Unknown")
            .Select(group => new StatisticsPublicPlanStatusDto
            {
                Id = group.First()._Id,
                CreateAt = group.Key != "Unknown"
                    ? DateTime.ParseExact(group.Key, "dd/MM/yyyy", null)
                    : (DateTime?)null,
                TotalPlan = group.Count()
            });

        var data = new StatisticsPublicPlanResultDto
        {
            StatisticsPublicPlan = StatisticsPublicPlan,
            StatisticsPublicPlanStatus = groupPublicPlan.OrderBy(x => x.CreateAt).ToArray()
        };

        return Ok(data);
    }
    
    [HttpGet("~/api/statistics/budget/paged")]
    public async Task<PagedResultDto<StatisticsBudgetDto>> GetListPagedAsync(PagingRequestInput input)
    {
        var query = (from payment in await _paymentService.GetAll()
            join account in await _accountService.GetAll() on payment.UserId equals account._Id
            select new StatisticsBudgetDto
            {
                Id = payment._Id,
                OrderInfo = payment.OrderInfo,
                PaymentDate = payment.PaymentDate,
                Point = payment.Point,
                UserId = payment.UserId,
                UserName = account.FullName,
                Amount = payment.Amount,
                TransactionId = payment.TransactionId,
                BankCode = payment.BankCode,
                CardType = payment.CardType,
                TransactionStatus = payment.TransactionStatus,
                SecureHash = payment.SecureHash
            }).Where(input);

        var items = query
            .OrderByDescending(x => x.PaymentDate)
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<StatisticsBudgetDto>(total, items);
    }
    
    [HttpGet("~/api/statistics/budget/card")]
    public async Task<StatisticsBudgetCard> GetCardAsync()
    {
        var query = await _paymentService.GetAll();
        var totalAmount = query.Sum(x => x.Amount);
        var totalPoint = query.Sum(x => x.Point);
        var totalTransaction = query.Count();

        return new StatisticsBudgetCard
        {
            TotalAmount = totalAmount,
            TotalPoint = totalPoint,
            TotalTransaction = totalTransaction
        };
       
    }
}