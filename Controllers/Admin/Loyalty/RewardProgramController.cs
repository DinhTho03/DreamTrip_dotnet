using brandportal_dotnet.Contracts.Loyalty.RewardProgram;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService.IRewardProgram;
using brandportal_dotnet.Models;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace brandportal_dotnet.Controllers.Loyalty;

[Route("[controller]")]
[ApiController]
public class RewardProgramController : ControllerBase
{
    private readonly IRewardProgramRepository<LoyRewardProgram> _loyRewardProgramService;
    private readonly IRewardProgramRepository<LoyProgram> _loyProgramService;
    private readonly IRewardProgramRepository<LoyRewardRedeem> _loyRewardRedeemService;
    private readonly IRewardProgramRepository<LoyRewardProduct> _loyRewardProductService;

    public RewardProgramController(
        IRewardProgramRepository<LoyRewardProgram> rewardProgramService, 
        IRewardProgramRepository<LoyProgram> loyProgramService, 
        IRewardProgramRepository<LoyRewardRedeem> loyRewardRedeemService, 
        IRewardProgramRepository<LoyRewardProduct> loyRewardProductService
        ) {
        _loyRewardProgramService = rewardProgramService;
        _loyProgramService = loyProgramService;
        _loyRewardRedeemService = loyRewardRedeemService;
        _loyRewardProductService = loyRewardProductService;
    }
    /// <summary>
    /// Danh sách chương trình tiêu điểm
    /// </summary>
    /// <param name="input"></param>
    /// <returns></returns>
    [HttpGet("~/api/loyalty/reward-program/selector-paged")]
    public async Task<PagedResultDto<RewardProgramDto>> GetSelectorListPagedAsync(PagingRequestInput input)
    {
        var query = (from reward in await _loyRewardProgramService.GetAll()
                     where reward.IsDeleted == false && reward.IsActive != false && reward.RewardProgramType == "program"
                     join loyProgram in await _loyProgramService.GetAll() on reward.ProgramId equals loyProgram._Id
                     join rewardRedeem in await _loyRewardRedeemService.GetAll() on reward._Id equals rewardRedeem.RewardProgramId into groupRedeem
                     select new RewardProgramDto
                     {
                         Id = reward._Id,
                         RewardProgramCode = reward.RewardProgramCode,
                         RewardProgramName = reward.RewardProgramName,
                         ProgramName = loyProgram.Name,
                         RewardId = reward.RewardId,
                         DateStart = reward.DateStart,
                         DateEnd = reward.DateEnd,
                         Point = reward.Point,
                         MembershipType = reward.MembershipType,
                         IsActive = reward.IsActive,
                         IsLimitedExchangeAll = reward.IsLimitedExchangeAll,
                         QuotaLimitedExchangeAll = reward.QuotaLimitedExchangeAll,
                         RedeemSuccess = groupRedeem.Count(),
                     }).Where(input);

        var data = query
            .OrderByDescending(e => e.Id)
            .OrderBy(input)
            .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<RewardProgramDto>(total, data);
    }

    /// <summary>
    /// Get reward program paged
    /// </summary>
    [HttpGet("~/api/loyalty/reward-program/paged")]
    public async Task<PagedResultDto<RewardProgramPagedDto>> GetListPagedAsync(PagingRequestInput input)
    {
        var query = (from reward in await _loyRewardProgramService.GetAll()
                     join program in await _loyProgramService.GetAll()
                     on reward.ProgramId equals program._Id into programGroup
                     from program in programGroup.DefaultIfEmpty()
                     join redeem in await _loyRewardRedeemService.GetAll()
                     on reward._Id equals redeem.RewardProgramId into redeemGroup
                     from redeem in redeemGroup.DefaultIfEmpty()
                     where reward.RewardProgramType == "program"
                     group redeem by new
                     {
                         reward._Id,
                         reward.RewardProgramCode,
                         reward.RewardProgramName,
                         reward.RewardId,
                         program.Name,
                         reward.DateStart,
                         reward.DateEnd,
                         reward.Point,
                         reward.MembershipType,
                         reward.IsActive,
                         reward.IsLimitedExchangeAll,
                         reward.QuotaLimitedExchangeAll,
                         reward.LimitedExchangeType,
                         reward.SourcePointKey,
                     } into g
                     select new RewardProgramPagedDto
                     {
                         Id = g.Key._Id,
                         RewardProgramCode = g.Key.RewardProgramCode,
                         RewardProgramName = g.Key.RewardProgramName,
                         ProgramName = g.Key.Name,
                         DateStart = g.Key.DateStart,
                         DateEnd = g.Key.DateEnd,
                         Point = g.Key.Point,
                         MembershipType = g.Key.MembershipType,
                         IsActive = g.Key.IsActive,
                         IsLimitedExchangeAll = g.Key.IsLimitedExchangeAll,
                         QuotaLimitedExchangeAll = g.Key.QuotaLimitedExchangeAll,
                         LimitedExchangeType = g.Key.LimitedExchangeType,
                         RewardId = g.Key.RewardId,
                         SourcePointKey = g.Key.SourcePointKey,
                         //RedeemSuccess = g.Count(x => x.RewardProgramId != null),
                     }
                     ).Where(input);

        var data = query
                       .OrderByDescending(e => e.Id)
                                  .OrderBy(input)
                                             .Page(input).ToList();
        var total = query.Count();

        return new PagedResultDto<RewardProgramPagedDto>(total, data);
    }



    /// <summary>
    /// Get reward program by id
    /// </summary>
    [HttpGet("~/api/loyalty/reward-program/{id}")]
    public async Task<RewardProgramDetailDto> GetRewardProgramByIdAsync(string id)
    {
        var rewardProgram = await _loyRewardProgramService.GetById(id)
            ?? throw new EntityNotFoundException(typeof(LoyRewardProgram), id);

        RewardProgramDetailDto rewardProgramDetail = new RewardProgramDetailDto
        {
            Id = rewardProgram._Id,
            RewardProgramCode = rewardProgram.RewardProgramCode,
            RewardProgramName = rewardProgram.RewardProgramName,
            ProgramId = rewardProgram.ProgramId,
            RewardId = rewardProgram.RewardId,
            Point = rewardProgram.Point,
            MembershipType = rewardProgram.MembershipType,
            IsActive = rewardProgram.IsActive,
            IsLimitedExchangeAll = rewardProgram.IsLimitedExchangeAll,
            QuotaLimitedExchangeAll = rewardProgram.QuotaLimitedExchangeAll,
            IsLimitedExchangeOutlet = rewardProgram.IsLimitedExchangeOutlet,
            QuotaLimitedExchangeOutlet = !string.IsNullOrEmpty(rewardProgram.QuotaLimitedExchangeOutlet) ? Int32.Parse(rewardProgram.QuotaLimitedExchangeOutlet) : null,
            StartDate = rewardProgram.DateStart,
            EndDate = rewardProgram.DateEnd,
            ConfigTurn = rewardProgram.ConfigTurn,
            LimitedExchangeType = rewardProgram.LimitedExchangeType,
            BonusPlaysType = rewardProgram.BonusPlaysType,
            PlayTurnNumber = rewardProgram.PlayTurnNumber,
            QuotaChange = rewardProgram.QuotaChange,
            Banner = rewardProgram.Banner,
            ApplyLoyalty = rewardProgram.ApplyLoyalty,
            RewardProgramDescription = rewardProgram.RewardProgramDescription,
            ProgramType = rewardProgram.SourcePointKey,
            IsDisplay = rewardProgram.IsDisplay
        };

        //if (rewardProgram.ApplyLoyalty == true && rewardProgram.SourcePointKey == "photo_tracking")
        //{
        //    var rewardProduct = await _loyRewardProductRepository.FirstOrDefaultAsync(x => x.RewardProgramId == rewardProgram.Id && x.RewardId == rewardProgram.RewardId.ToString())
        //        // ?? throw new EntityNotFoundException(typeof(DmsproMysLoyRewardProduct), id)
        //        ;
        //    rewardProgramDetail.ProductId = rewardProduct?.ProductId;
        //    rewardProgramDetail.UomId = rewardProduct?.ProductUomId;
        //}

        //if (rewardProgram.ApplyLoyalty == true && rewardProgram.MembershipType == "rank")
        //{
        //    var rankItems = await _loyRewardProgramRankRepository.GetListAsync(x => x.RewardProgramId == id);
        //    rewardProgramDetail.RankItems = rankItems.Select(e => new RankItemDto
        //    {
        //        Id = e.Id,
        //        RankId = e.RankId,
        //        Point = e.Point
        //    }).ToList();
        //}

        return rewardProgramDetail;
    }

    /// <summary>
    /// Get programs
    /// </summary>
    [HttpGet("~/api/loyalty/reward-program/programs")]
    public async Task<List<ProgramDto>> GetProgramsAsync()
    {
        var query = await _loyProgramService.GetAll();
        var result = query.Select(e => new ProgramDto
        {
            Id = e._Id,
            Name = e.Name
        }).ToList();
        return result;
    }

    ///<summary>Reward program create</summary>
    [HttpPost("~/api/loyalty/reward-program")]
    public async Task<resultRewardProgramCreateDto> RewardProgramCreateAsync([FromBody] RewardProgramCreateDto input)
    {
        var createRewardProgram = new LoyRewardProgram
        {
            RewardProgramType = "program",
            ProgramId = input.ProgramId,
            RewardProgramCode = input.RewardProgramCode,
            RewardProgramName = input.RewardProgramName,
            IsActive = input.IsActive,
            IsDisplay = input.IsDisplay,
            SourcePointKey = input.ProgramType,
            MembershipType = input.MembershipType,
            Point = input.Point,
            IsLimitedExchangeAll = input.IsLimitedExchangeAll,
            QuotaLimitedExchangeAll = input.QuotaLimitedExchangeAll,
            IsLimitedExchangeOutlet = input.IsLimitedExchangeOutlet,
            QuotaLimitedExchangeOutlet = input.QuotaLimitedExchangeOutlet.ToString(),
            CreatedAt = DateTime.Now,
            CreatedBy = 1,
            DateStart = input.ApplyLoyalty == true ? input.StartDate ?? DateTime.Now : null,
            DateEnd = input.ApplyLoyalty == true ? input.EndDate ?? DateTime.Now.AddDays(90) : null,
            ConfigTurn = input.ConfigTurn ?? "minus_points_directly",
            RewardId = input.RewardId,
            LimitedExchangeType = input.LimitedExchangeType,
            BonusPlaysType = input.BonusPlaysType,
            PlayTurnNumber = input.PlayTurnNumber,
            QuotaChange = input.QuotaChange,
            Banner = input.Banner,
            ApplyLoyalty = input.ApplyLoyalty,
            RewardProgramDescription = input.RewardProgramDescription ?? string.Empty
        };

        await _loyRewardProgramService.Insert(createRewardProgram);

        if (input.ProgramType != "game_wheel")
        {
            var createRewardProduct = new LoyRewardProduct
            {
                RewardId = input.RewardId.ToString(),
                RewardProgramId = createRewardProgram._Id,
                ProductId = input.ProductId,
                ProductUomId = input.UomId,
                Quota = input.QuotaChange,
                CreatedAt = DateTime.Now
            };

            await _loyRewardProductService.Insert(createRewardProduct);
        }

        //if (input.MembershipType == "rank" && input.RankItems.Any())
        //{
        //    var createRankItems = input.RankItems.Select(e => new DmsproMysLoyRewardProgramRank
        //    {
        //        RewardProgramId = createRewardProgram.Id,
        //        RankId = e.RankId,
        //        Point = e.Point,
        //        CreatedAt = DateTime.Now
        //    }).ToList();

        //    await _loyRewardProgramRankRepository.InsertManyAsync(createRankItems, true);
        //}

        return new resultRewardProgramCreateDto
        {
            result = createRewardProgram._Id
        };
    }


    ///<summary>Reward program update</summary>
    [HttpPut("~/api/loyalty/reward-program/{id}")]
    public async Task<resultRewardProgramCreateDto> RewardProgramUpdateAsync(string id, [FromBody] RewardProgramCreateDto input)
    {
        var rewardProgram = await _loyRewardProgramService.GetById(id)
            ?? throw new EntityNotFoundException(typeof(LoyRewardProgram), id);

        if (rewardProgram.ApplyLoyalty != true)
        {
            rewardProgram.RewardProgramName = input.RewardProgramName;
            rewardProgram.BonusPlaysType = input.BonusPlaysType;
            rewardProgram.PlayTurnNumber = input.PlayTurnNumber;
        }
        else
        {
            rewardProgram.ProgramId = input.ProgramId;
            rewardProgram.RewardProgramName = input.RewardProgramName;
            rewardProgram.SourcePointKey = input.ProgramType;
            rewardProgram.Point = input.Point;
            rewardProgram.MembershipType = input.MembershipType;
            rewardProgram.IsLimitedExchangeOutlet = input.IsLimitedExchangeOutlet;
            rewardProgram.QuotaLimitedExchangeOutlet = input.QuotaLimitedExchangeOutlet.ToString();
            rewardProgram.LimitedExchangeType = input.LimitedExchangeType;
            rewardProgram.Banner = input.Banner;
            rewardProgram.RewardProgramDescription = input.RewardProgramDescription ?? string.Empty;
            if (input.ProgramType == "photo_tracking")
            {
                rewardProgram.IsActive = input.IsActive;
                rewardProgram.DateStart = input.StartDate;
                rewardProgram.DateEnd = input.EndDate;
                rewardProgram.RewardId = input.RewardId;
                rewardProgram.QuotaChange = input.QuotaChange;
                rewardProgram.QuotaLimitedExchangeAll = input.QuotaLimitedExchangeAll;
            }
            else
            {
                rewardProgram.IsDisplay = input.IsDisplay;
                rewardProgram.IsLimitedExchangeAll = input.IsLimitedExchangeAll;
                rewardProgram.QuotaLimitedExchangeAll = input.QuotaLimitedExchangeAll;
                rewardProgram.ConfigTurn = input.ConfigTurn ?? "minus_points_directly";
            }
        }

        await _loyRewardProgramService.Update(id ,rewardProgram);

        //if (input.ProgramType != "game_wheel")
        //{
        //    var rewardProduct = await _loyRewardProductRepository.FirstOrDefaultAsync(x => x.RewardProgramId == id);
        //    if (rewardProduct == null)
        //    {
        //        var createRewardProduct = new DmsproMysLoyRewardProduct
        //        {
        //            RewardId = input.RewardId.ToString(),
        //            RewardProgramId = id,
        //            ProductId = input.ProductId,
        //            ProductUomId = input.UomId,
        //            Quota = input.QuotaChange,
        //            CreatedAt = DateTime.Now
        //        };

        //        await _loyRewardProductRepository.InsertAsync(createRewardProduct, true);
        //    }
        //    else
        //    {
        //        rewardProduct.RewardId = input.RewardId.ToString();
        //        rewardProduct.ProductId = input.ProductId;
        //        rewardProduct.ProductUomId = input.UomId;
        //        rewardProduct.Quota = input.QuotaChange;
        //        rewardProduct.UpdatedAt = DateTime.Now;

        //        await _loyRewardProductRepository.UpdateAsync(rewardProduct, true);
        //    }
        //}

        //var deleteRankItems = await _loyRewardProgramRankRepository.GetListAsync(x => x.RewardProgramId == id);
        //if (deleteRankItems.Any())
        //{
        //    await _loyRewardProgramRankRepository.DeleteManyAsync(deleteRankItems, true);
        //}

        //if (input.MembershipType == "rank" && input.RankItems.Any())
        //{
        //    var createRankItems = input.RankItems.Select(e => new DmsproMysLoyRewardProgramRank
        //    {
        //        RewardProgramId = rewardProgram.Id,
        //        RankId = e.RankId,
        //        Point = e.Point,
        //        CreatedAt = DateTime.Now
        //    }).ToList();

        //    await _loyRewardProgramRankRepository.InsertManyAsync(createRankItems, true);
        //}

        return new resultRewardProgramCreateDto
        {
            result = rewardProgram._Id
        };
    }



}
