using brandportal_dotnet.Contracts.Games;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IRewardProgram;
using brandportal_dotnet.Models;
using brandportal_dotnet.Service.RewardProgram;
using brandportal_dotnet.shared;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Domain.Entities;

namespace brandportal_dotnet.Controllers.game
{
    [Route("[controller]")]
    [ApiController]
    public class GameController : ControllerBase
    {
        private readonly IRepository<Game> _gameService;
        private readonly IRepository<GameCategory> _gameCategoryService;
        private readonly IRewardProgramRepository<LoyRewardProgram> _loyRewardProgramService;
        private readonly IRewardProgramRepository<LoyProgram> _loyProgramService;
        private readonly IRepository<GameRate> _gameRateService;
        private readonly IRepository<LoyReward> _loyRewardService;
        private readonly IRepository<LoyRewardProgramGame> _loyRewardProgramGameService;
        private readonly IRepository<LoyAccumulationProgram> _loyAccumulationProgramService;
        private readonly IRepository<LoyRewardAccumulation> _loyRewardAccumulationService;
        private readonly IRepository<LoyNotification> _loyNotificationService;


        public GameController
        (
            IRepository<Game> gameRepository,
            IRepository<GameCategory> gameCategoryRepository,
            IRewardProgramRepository<LoyRewardProgram> loyRewardProgramService,
            IRepository<GameRate> gameRateService,
            IRewardProgramRepository<LoyProgram> loyProgramService,
            IRepository<LoyReward> loyRewardService,
            IRepository<LoyRewardProgramGame> loyRewardProgramGameService,
            IRepository<LoyAccumulationProgram> loyAccumulationProgramService,
            IRepository<LoyRewardAccumulation> loyRewardAccumulationService,
            IRepository<LoyNotification> loyNotificationService
        )
        {
            _gameService = gameRepository;
            _gameCategoryService = gameCategoryRepository;
            _loyRewardProgramService = loyRewardProgramService;
            _gameRateService = gameRateService;
            _loyProgramService = loyProgramService;
            _loyRewardService = loyRewardService;
            _loyRewardProgramGameService = loyRewardProgramGameService;
            _loyAccumulationProgramService = loyAccumulationProgramService;
            _loyRewardAccumulationService = loyRewardAccumulationService;
            _loyNotificationService = loyNotificationService;
        }

        [HttpGet("~/api/game-all/paged")]
        public async Task<PagedResultDto<PageGameDto>> GetListGameAsync(PagingRequestInput input)
        {
            var query = (from game in await _gameService.GetAll()
                where game.IsDeleted == false
                join gameCategory in await _gameCategoryService.GetAll() on game.CateId equals gameCategory._Id
                select new PageGameDto
                {
                    Id = game._Id,
                    Name = game.GameName,
                    Code = game.GameCode,
                    CateId = gameCategory._Id,
                    CateName = gameCategory.CateName,
                    IsActive = game.IsActive,
                    StartDate = game.StartDate,
                    EndDate = game.EndDate,
                    CreatedAt = game.CreatedAt,
                }).Where(input);

            var data = query
                .OrderByDescending(x => x.CreatedAt)
                .OrderBy(input)
                .Page(input).ToList();
            var total = query.Count();

            return new PagedResultDto<PageGameDto>(total, data);
        }

        /// <summary>
        /// Chi tiết chương trình game: Thông tin chung
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("~/api/game/detail-info/{id}")]
        public async Task<GameGeneralInfoDto> GetDetailGameAsync(string id, string type)
        {
            var query = (from game in await _gameService.GetAll()
                where game.IsDeleted == false && game._Id == id
                where game.CateType == type
                join gameCate in await _gameCategoryService.GetAll() on game.CateId equals gameCate._Id
                join rewardProgram in await _loyRewardProgramService.GetAll() on game.RewardProgramId equals
                    rewardProgram._Id
                select new GameGeneralInfoDto
                {
                    Id = game._Id,
                    Name = game.GameName,
                    Code = game.GameCode,
                    PeriodType = game.PeriodType,
                    StartDate = game.StartDate,
                    EndDate = game.EndDate,
                    CateId = gameCate._Id,
                    CateName = gameCate.CateName,
                    CateType = gameCate.CateType,
                    Frequency = game.Frequency,
                    FrequencyValue = game.FrequencyValue,
                    FrequencyMonthlyType = game.FrequencyMonthlyType,
                    DayInMonthly = game.DayInMonthly,
                    DayInWeek = game.DayInWeek,
                    DayInWeekRepeat = game.DayInWeekRepeat,
                    PeriodInDateType = game.PeriodInDateType,
                    PeriodInDateStart = game.PeriodInDateStart,
                    PeriodInDateEnd = game.PeriodInDateEnd,
                    RewardProgramId = game.RewardProgramId,
                    RewardProgramName = rewardProgram.RewardProgramName,
                    ConfigTurn = game.ConfigTurn,
                    IsActive = game.IsActive,
                    CreatedAt = game.CreatedAt
                });
            var item = query.FirstOrDefault()
                       ?? throw new EntityNotFoundException(typeof(Game), id);
            return item;
        }

        /// <summary>
        /// Danh sách Loại game
        /// </summary>
        /// <returns></returns>
        [HttpGet("~/api/game-all/filter-cate-name")]
        public async Task<List<GameCategoryByNameDto>> GetCategoryByCateNameAsync()
        {
            // Chỉ lấy 1 vòng xoay may mắn với Id là 636460b82884711f5decfea1 
            var query = (from a in await _gameCategoryService.GetAll()
                select new GameCategoryByNameDto
                {
                    Id = a._Id,
                    Name = a.CateName
                }).Where(x => x.Id == "636460b82884711f5decfea1");

            var listQuery = query.ToList();

            return listQuery;
        }


        [HttpGet("~/api/game/filter-reward-program")]
        public async Task<List<GameRewardProgramNameDto>> GetRewardProgramNameAsync()
        {
            var query = (from rewardProgram in await _loyRewardProgramService.GetAll()
                where rewardProgram.RewardProgramType == "program" && rewardProgram.SourcePointKey == "game_wheel"
                select new GameRewardProgramNameDto
                {
                    Id = rewardProgram._Id,
                    Name = rewardProgram.RewardProgramName
                }).Distinct();
            var listQuery = query.ToList();

            return listQuery;
        }

        [HttpPost("~/api/game/detail-info")]
        public async Task<IActionResult> AddDetailGameAsync(GameGeneralInfoCreateDto input)
        {
            var now = DateTime.UtcNow;
            var cate = await _gameCategoryService.GetById(input.CateId)
                       ?? throw new EntityNotFoundException(typeof(GameCategory), input.CateId);
            var rewardProgram = await _loyRewardProgramService.GetById(input.RewardProgramId)
                                ?? throw new EntityNotFoundException(typeof(LoyRewardProgram),
                                    input.RewardProgramId);

            var gameCodeFilters = new Dictionary<string, object>
            {
                { "GameCode", input.Code },
            };
            var gameCodeIsExisted = await _gameService.FindByProperties(gameCodeFilters);
            if (gameCodeIsExisted != null)
            {
                return BadRequest(new { message = "Mã chương trình đã tồn tại vui lòng nhập mã khác" });
            }

            var game = new Game
            {
                GameName = input.Name,
                GameCode = input.Code,
                CateId = cate._Id,
                IsActive = false,
                IsActived = false,
                PeriodType = input.PeriodType,
                StartDate = input.StartDate,
                EndDate = input.EndDate,
                Frequency = input.Frequency,
                FrequencyMonthlyType = input.FrequencyMonthlyType,
                FrequencyValue = input.FrequencyValue,
                DayInMonthly = input.DayInMonthly,
                DayInWeek = input.DayInWeek,
                DayInWeekRepeat = input.DayInWeekRepeat,
                PeriodInDateType = input.PeriodInDateType,
                PeriodInDateStart = input.PeriodInDateStart,
                PeriodInDateEnd = input.PeriodInDateEnd,
                RewardProgramId = rewardProgram._Id,
                ObjectApply = "all",
                ConfigTurn = input.ConfigTurn,
                CreatedAt = now,
                IsDeleted = false,
                IsUpdateInfo = false,
                IsUpdateWin = false,
                CateType = cate.CateType,
            };

            await _gameService.Insert(game);

            return Ok(new GameResultDto
            {
                Id = game._Id,
            });
        }

        [HttpPut("~/api/game/detail-info/{id}")]
        public async Task<IActionResult> UpdateDetailGameAsync(string id, GameGeneralInfoCreateDto input)
        {
            var now = DateTime.UtcNow;

            var cate = await _gameCategoryService.GetById(input.CateId)
                       ?? throw new EntityNotFoundException(typeof(GameCategory), input.CateId);
            var game = await _gameService.GetById(id)
                       ?? throw new EntityNotFoundException(typeof(Game), id);
            var rewardProgram = await _loyRewardProgramService.GetById(input.RewardProgramId)
                                ?? throw new EntityNotFoundException(typeof(LoyRewardProgram),
                                    input.RewardProgramId);
            var gameCodeFilters = new Dictionary<string, object>
            {
                { "GameCode", input.Code },
            };
            var existGame = await _gameService.FindByProperties(gameCodeFilters);
            if (existGame._Id != game._Id)
            {
                return BadRequest(new { message = "Mã code này đã tồn tại" });
            }

            game.GameName = input.Name;
            game.GameCode = input.Code;
            game.CateId = cate._Id;
            game.PeriodType = input.PeriodType;
            game.StartDate = input.StartDate;
            game.EndDate = input.EndDate;
            game.Frequency = input.Frequency;
            game.FrequencyMonthlyType = input.FrequencyMonthlyType;
            game.FrequencyValue = input.FrequencyValue;
            game.DayInMonthly = input.DayInMonthly;
            game.DayInWeek = input.DayInWeek;
            game.DayInWeekRepeat = input.DayInWeekRepeat;
            game.PeriodInDateType = input.PeriodInDateType;
            game.PeriodInDateStart = input.PeriodInDateStart;
            game.PeriodInDateEnd = input.PeriodInDateEnd;
            game.RewardProgramId = rewardProgram._Id;
            game.ConfigTurn = input.ConfigTurn;
            game.UpdatedAt = now;
            game.CateType = cate.CateType;

            await _gameService.Update(id, game);
            return Ok(new GameResultDto
            {
                Id = game._Id
            });
        }

        [HttpGet("~/api/reward-game/{id}")]
        public async Task<GameRewardDto> GetRewardGameAsync(string id)
        {
            var game = await _gameService.GetById(id)
                       ?? throw new EntityNotFoundException(typeof(Game), id);

            var winratesCondition = new Dictionary<string, object>
            {
                { "GameId", game._Id }
            };
            var winrates = await _gameRateService.FindListByProperties(winratesCondition);

            var query = (from rewardProgram in await _loyRewardProgramService.GetAll()
                join loyReward in await _loyRewardService.GetAll() on rewardProgram.RewardId equals loyReward._Id
                join rewardProgramGame in await _loyRewardProgramGameService.GetAll() on rewardProgram._Id equals
                    rewardProgramGame.RewardProgramId
                join rewardAccumulation in (await _loyRewardAccumulationService.GetAll()) on rewardProgram._Id equals
                    rewardAccumulation?.RewardProgramId into groupRewardAccumulations
                from rewardAccumulation in groupRewardAccumulations.DefaultIfEmpty()
                join loyAccumulation in (await _loyAccumulationProgramService.GetAll()) on rewardAccumulation
                    ?.AccumulationProgramId equals loyAccumulation?._Id into groupAccumulations
                from loyAccumulation in groupAccumulations.DefaultIfEmpty()
                join loyNotification in await _loyNotificationService.GetAll() on rewardProgramGame.RewardProgramId
                    equals loyNotification?.ObjectSubId into groupLoyNotifications
                from loyNotification in groupLoyNotifications.DefaultIfEmpty()
                where rewardProgramGame.GameId == id.ToString()
                group new
                {
                    rewardProgram,
                    rewardProgramGame,
                    loyReward,
                    loyNotification,
                    loyAccumulation
                } by rewardProgram._Id
                into g
                select new RewardDto
                {
                    Id = g.Max(x => x.rewardProgram._Id),
                    RewardProgramGameId = g.Max(x => x.rewardProgramGame._Id),
                    GiftType = g.Max(x => x.loyReward.Gift),
                    Image = g.Max(x => x.rewardProgramGame.Image),
                    Value = g.Max(x => x.rewardProgramGame.Value),
                    Position = g.Max(x => x.rewardProgramGame.Position),
                    TotalBudgetQuota = g.Max(x => x.rewardProgramGame.TotalBudgetQuota),
                    Background = g.Max(x => x.rewardProgramGame.Background),
                    WinRate = g.Max(x => x.rewardProgramGame.WinRate),
                    IsActive = g.Max(x => x.rewardProgram.IsActive),
                    QuotaChange = g.Max(x => x.rewardProgram.QuotaChange),
                    QuotaLimitedExchangeAll = g.Max(x => x.rewardProgram.QuotaLimitedExchangeAll),
                    AccumulationProgramName = g.Max(x => x.loyAccumulation?.AccumulationProgramName ?? ""),
                    CateType = game.CateType,
                    GameNotificationData = new GameNotificationCreateDto
                    {
                        Id = g.Max(x => x.loyNotification?._Id),
                        Title = g.Max(x => x.loyNotification?.Title),
                        NotificationType = g.Max(x => x.loyNotification?.NotificationType),
                        ObjectSubId = g.Max(x => x.loyNotification?.ObjectSubId),
                        Background = g.Max(x => x.loyNotification?.Background),
                        Description = g.Max(x => x.loyNotification?.Description),
                        DescriptionShow = g.Max(x => x.loyNotification?.DescriptionShow),
                        ObjectId = g.Max(x => x.loyNotification?.ObjectId),
                        ParamsShow = g.Max(x => x.loyNotification?.ParamsShow),
                        TitleShow = g.Max(x => x.loyNotification?.TitleShow),
                        SubId = g.Max(x => x.loyNotification?.SubId),
                    }
                });


            var data = query.OrderBy(x => x.Position).ToList();

            return new GameRewardDto
            {
                Id = game._Id,
                WinType = game.WinType,
                NumbetWheel = game.NumbetWheel,
                WinQuotaLimit = game.WinQuotaLimit,
                ListRewardData = data,
                WinRateData = winrates.Select(x => new GameWinRateDto
                {
                    Id = x._Id,
                    PercentWin = x.PercentWin,
                    Times = x.Times
                })
            };
        }


        private async Task<GameRewardProgramDto> GetDetailGame(string id, string? type)
        {
            var arrGame = (from games in await _gameService.GetAll()
                where games._Id == id
                join rewardProgram in await _loyRewardProgramService.GetAll() on games.RewardProgramId equals
                    rewardProgram._Id into groupRewardPrograms
                from groupRewardProgram in groupRewardPrograms.DefaultIfEmpty()
                select new GameRewardProgramDto
                {
                    ProgramId = groupRewardProgram.ProgramId,
                    ApplyLoyalty = groupRewardProgram.ApplyLoyalty,
                    BonusPlaysType = groupRewardProgram.BonusPlaysType,
                    PlayTurnNumber = groupRewardProgram.PlayTurnNumber,
                    CateType = games.CateType
                });
            if (!type.IsNullOrEmpty())
            {
                arrGame.Where(game => game.CateType == type);
            }

            ;
            return arrGame.FirstOrDefault()!;
        }

        /// <summary>
        /// Cập nhật chương trình game: tỉ lệ thắng giải và giải thưởng
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("~/api/reward-game/{id}")]
        public async Task<IActionResult> UpdateRewardGameAsync(string id, GameRewardCreateDto input)
        {
            if (input.ListRewardData.Count() == 0)
            {
                return BadRequest(new { message = "Vui lòng cài đặt giải thưởng" });
            }

            if (input.ListRewardData.Where(reward => reward.IsActive == true).Count() != input.NumbetWheel)
            {
                return BadRequest(new
                {
                    message =
                        "Số lượng quà tặng trạng thái hoạt động không đúng số lượng đã cấu hình"
                });
            }


            var now = DateTime.Now;
            var game = await _gameService.GetById(id)
                       ?? throw new EntityNotFoundException(typeof(Game), id);
            game.IsActive = false;

            var winRate = input.ListRewardData.Sum(x => x.WinRate);
            if (winRate != 100 && game.CateType == "wheel_distributor")
            {
                return BadRequest(new { message = "Tổng tỷ lệ trúng thưởng phải bằng 100%" });
            }

            var arrGame = await GetDetailGame(id, null);

            var gameRateCondition = new Dictionary<string, object>
            {
                { "GameId", id }
            };
            var gameRate = await _gameRateService.FindListByProperties(gameRateCondition);
            // Xóa tất cả tỉ lệ cũ
            if (gameRate.Count > 0)
            {
                await _gameRateService.DeleteAll(gameRate.Select(x => x._Id).ToList());
            }

            if (input.WinType == false)
            {
                if (input.WinQuotaLimit == -1)
                {
                    var gameWinRate = new GameRate()
                    {
                        GameId = id,
                        Times = -1,
                        PercentWin = input.WinRateData.First().PercentWin
                    };
                    await _gameRateService.Insert(gameWinRate);
                }
                else
                {
                    game.WinQuotaLimit = input.WinQuotaLimit;
                    var key = 1;
                    foreach (var item in input.WinRateData)
                    {
                        var arrGameWin = new GameRate()
                        {
                            GameId = id,
                            Times = key,
                            PercentWin = item.PercentWin
                        };
                        key++;
                        await _gameRateService.Insert(arrGameWin);
                    }

                    var gameWinRate = new GameRate()
                    {
                        GameId = id,
                        Times = -1,
                        PercentWin = 0
                    };
                    await _gameRateService.Insert(gameWinRate);
                }
            }
            else
            {
                var gameWinRate = new GameRate()
                {
                    GameId = id,
                    Times = -1,
                    PercentWin = 100
                };
                await _gameRateService.Insert(gameWinRate);
            }

            game.WinType = input.WinType;
            game.WinQuotaLimit = input.WinQuotaLimit;
            game.NumbetWheel = input.NumbetWheel;
            game.IsUpdateWin = true;
            await _gameService.Update(id, game);
            short position = 1;
            var arrProduct = input.ListRewardData.Where(x => x.GiftType == "product").ToList();

            var arrAccumulatePoint = input.ListRewardData.Where(x => x.GiftType == "point").ToList();
            foreach (var item in arrAccumulatePoint)
            {
                string rewardProgramId;
                var arrRewardProgram = await _loyRewardProgramService.GetById(item.Id) ??
                                       new LoyRewardProgram() { };

                arrRewardProgram.RewardProgramType = "gift_wheel";
                arrRewardProgram.SourcePointKey = "game_wheel";
                arrRewardProgram.ProgramId = arrGame.ProgramId;
                arrRewardProgram.QuotaChange = item.QuotaChange ?? 1;
                arrRewardProgram.IsLimitedExchangeAll = true;
                arrRewardProgram.QuotaLimitedExchangeAll = item.QuotaLimitedExchangeAll ?? 0;
                arrRewardProgram.MembershipType = "all";
                arrRewardProgram.IsActive = item.IsActive ?? false;
                arrRewardProgram.RewardProgramName = item.AccumulationProgramName;
                arrRewardProgram.RewardProgramDescription = item.AccumulationProgramName;
                arrRewardProgram.RewardId = "66f926f11f4d8fa281acd3d3";
                arrRewardProgram.ConfigTurn = "minus_points_directly";
                if (arrRewardProgram._Id == null)
                {
                    arrRewardProgram.CreatedAt = now;
                    arrRewardProgram.CreatedBy = 1;
                    var insertedRewardProgram = await _loyRewardProgramService.Insert(arrRewardProgram, true);
                    rewardProgramId = insertedRewardProgram._Id;
                }
                else
                {
                    arrRewardProgram.UpdatedAt = now;
                    arrRewardProgram.UpdatedBy = 1;
                    await _loyRewardProgramService.Update(item.Id, arrRewardProgram);
                    rewardProgramId = arrRewardProgram._Id;
                }

                var arrRewardProgramGame =
                    await _loyRewardProgramGameService.GetById(item?.RewardProgramGameId) ??
                    new LoyRewardProgramGame { };
                arrRewardProgramGame.GameId = game._Id;
                arrRewardProgramGame.Image = item.Image;
                arrRewardProgramGame.Value = item.Value;
                arrRewardProgramGame.Background = item.Background;
                arrRewardProgramGame.Position = item.Position;
                arrRewardProgramGame.WinRate = item.WinRate;
                arrRewardProgramGame.QuantityUnit = item.QuotaChange ?? 1;
                arrRewardProgramGame.CreatedAt = now;
                // Id của Point là 66f926f11f4d8fa281acd3d2
                var arrLoyReward = await _loyRewardService.GetById("66f926f11f4d8fa281acd3d2");

                if (arrLoyReward != null)
                {
                    arrRewardProgramGame.IsWin = arrLoyReward.IsWin;
                }

                ;

                arrRewardProgramGame.RewardProgramId = rewardProgramId;

                if (arrRewardProgramGame._Id == null)
                {
                    arrRewardProgramGame.CreatedAt = now;
                    await _loyRewardProgramGameService.Insert(arrRewardProgramGame);
                }
                else
                {
                    arrRewardProgramGame.UpdatedAt = now;
                    await _loyRewardProgramGameService.Update(item.RewardProgramGameId, arrRewardProgramGame);
                }

                var loyAccumulation =
                    await _loyRewardAccumulationService.GetById(item.RewardAccumulationId) ??
                    new LoyRewardAccumulation { };
                loyAccumulation.ProgramId = arrGame.ProgramId;
                loyAccumulation.RewardId = "4";
                loyAccumulation.RewardProgramId = rewardProgramId;
                loyAccumulation.Quota = item.QuotaChange;
                loyAccumulation.AccumulationProgramId = item.AccumulationProgramId;
                if (loyAccumulation._Id == null)
                {
                    loyAccumulation.CreatedAt = now;
                    loyAccumulation.CreatedBy = 1;
                    await _loyRewardAccumulationService.Insert(loyAccumulation);
                }
                else
                {
                    loyAccumulation.UpdatedAt = now;
                    loyAccumulation.UpdatedBy = 1;
                    await _loyRewardAccumulationService.Update(item.RewardAccumulationId, loyAccumulation);
                }

                var arrNotiCondition = new Dictionary<string, object>
                {
                    { "ObjectSubId", item.Id }
                };
                var arrNoti = await _loyNotificationService.FindByProperties(arrNotiCondition) ??
                              new LoyNotification() { };
                arrNoti.NotificationType = "game";
                arrNoti.SubId = position;
                arrNoti.ObjectId = game._Id;
                arrNoti.ObjectSubId = rewardProgramId;
                arrNoti.TitleShow = item.GameNotificationData.TitleShow;
                arrNoti.Title = item.GameNotificationData.TitleShow;
                arrNoti.DescriptionShow = item.GameNotificationData.DescriptionShow;
                arrNoti.Description = item.GameNotificationData.Description;
                arrNoti.Background = item.GameNotificationData.Background;
                arrNoti.ParamsShow = item.GameNotificationData.ParamsShow;
                if (arrNoti._Id == null)
                {
                    arrNoti.CreatedAt = now;
                    arrNoti.CreatedBy = 1;
                    await _loyNotificationService.Insert(arrNoti);
                }
                else
                {
                    arrNoti.UpdatedAt = now;
                    arrNoti.UpdatedBy = 1;
                    await _loyNotificationService.Update(arrNoti._Id, arrNoti);
                }
            }


            var arrNotWin = input.ListRewardData.Where(x => x.GiftType == "not_win").ToList();
            foreach (var item in arrNotWin)
            {
                string rewardProgramId;
                var arrRewardProgram = await _loyRewardProgramService.GetById(item.Id) ??
                                       new LoyRewardProgram { };

                arrRewardProgram.RewardProgramType = "gift_wheel";
                arrRewardProgram.SourcePointKey = "game_wheel";
                arrRewardProgram.ProgramId = arrGame.ProgramId;
                arrRewardProgram.QuotaChange = item.QuotaChange ?? 1;
                arrRewardProgram.IsLimitedExchangeAll = true;
                arrRewardProgram.QuotaLimitedExchangeAll = item.QuotaLimitedExchangeAll ?? 0;
                arrRewardProgram.MembershipType = "all";
                arrRewardProgram.IsActive = item.IsActive ?? false;
                arrRewardProgram.RewardProgramName = "Không trúng";
                arrRewardProgram.RewardProgramDescription = "Không trúng";
                arrRewardProgram.RewardId = "66f926f11f4d8fa281acd3d4";
                arrRewardProgram.ConfigTurn = "minus_points_directly";

                if (arrRewardProgram._Id == null)
                {
                    arrRewardProgram.CreatedAt = now;
                    arrRewardProgram.CreatedBy = 1;
                    var insertedRewardProgram = await _loyRewardProgramService.Insert(arrRewardProgram, true);
                    rewardProgramId = insertedRewardProgram._Id;
                }
                else
                {
                    arrRewardProgram.UpdatedAt = now;
                    arrRewardProgram.UpdatedBy = 1;
                    await _loyRewardProgramService.Update(arrRewardProgram._Id, arrRewardProgram);
                    rewardProgramId = arrRewardProgram._Id;
                }

                var arrRewardProgramGame =
                    await _loyRewardProgramGameService.GetById(item.RewardProgramGameId) ??
                    new LoyRewardProgramGame { };
                arrRewardProgramGame.GameId = game._Id.ToString();
                arrRewardProgramGame.Image = item.Image;
                arrRewardProgramGame.Value = item.Value;
                arrRewardProgramGame.Background = item.Background;
                arrRewardProgramGame.Position = item.Position;
                arrRewardProgramGame.WinRate = item.WinRate;
                arrRewardProgramGame.QuantityUnit = item.QuotaChange ?? 1;
                arrRewardProgramGame.CreatedAt = now;
                // Id của not_win là 66f926f11f4d8fa281acd3d4
                var arrLoyReward = await _loyRewardService.GetById("66f926f11f4d8fa281acd3d4");

                if (arrLoyReward != null)
                {
                    arrRewardProgramGame.IsWin = arrLoyReward.IsWin;
                }

                ;

                arrRewardProgramGame.RewardProgramId = rewardProgramId;

                if (arrRewardProgramGame._Id == null)
                {
                    arrRewardProgramGame.CreatedAt = now;
                    await _loyRewardProgramGameService.Insert(arrRewardProgramGame);
                }
                else
                {
                    arrRewardProgramGame.UpdatedAt = now;
                    await _loyRewardProgramGameService.Update(arrRewardProgramGame._Id,
                        arrRewardProgramGame);
                }

                var arrNotiCondition = new Dictionary<string, object>
                {
                    { "ObjectSubId", item.Id }
                };

                var arrNoti = await _loyNotificationService.FindByProperties(arrNotiCondition) ??
                              new LoyNotification { };
                arrNoti.NotificationType = "game";
                arrNoti.SubId = position;
                arrNoti.ObjectId = game._Id;
                arrNoti.ObjectSubId = rewardProgramId;
                arrNoti.TitleShow = item.GameNotificationData.TitleShow;
                arrNoti.Title = item.GameNotificationData.TitleShow;
                arrNoti.DescriptionShow = item.GameNotificationData.DescriptionShow;
                arrNoti.Description = item.GameNotificationData.DescriptionShow;
                arrNoti.Background = item.GameNotificationData.Background;
                arrNoti.ParamsShow = item.GameNotificationData.ParamsShow;
                if (arrNoti._Id == null)
                {
                    arrNoti.CreatedAt = now;
                    arrNoti.CreatedBy = 1;
                    await _loyNotificationService.Insert(arrNoti);
                }
                else
                {
                    arrNoti.UpdatedAt = now;
                    arrNoti.UpdatedBy = 1;
                    await _loyNotificationService.Update(arrNoti._Id, arrNoti);
                }
            }

            return Ok();
        }


        [HttpGet("~/api/game/list-accumulation")]
        public async Task<List<LookupDto>> GetListAccumulation()
        {
            var query = (from accumulation in await _loyAccumulationProgramService.GetAll()
                where accumulation.SourcePointKey == "game_wheel" && accumulation.IsDeleted == false
                select new LookupDto
                {
                    Id = accumulation._Id,
                    Name = accumulation.AccumulationProgramName,
                    Code = accumulation.AccumulationProgramCode
                });

            return query.ToList();
        }

        [HttpGet("~/api/game/gift-type")]
        public async Task<List<LookupDto>> GetGiftType()
        {
            var query = (from reward in await _loyRewardService.GetAll()
                where reward.GameWheel == true && reward.IsWin == true
                select new LookupDto
                {
                    Id = reward._Id,
                    Name = reward.Name!,
                    Code = reward.Gift!
                });

            return query.ToList();
        }


        /// <summary>
        /// Cập nhật trạng thái chương trình game
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPut("~/api/game/{id}/status")]
        public async Task<IActionResult> UpdateStatusGameAsync(string id, [FromBody] bool isActive)
        {
            var game = await _gameService.GetById(id);
            if (game == null)
            {
                return BadRequest(new
                {
                    message = "Chương trình game này không tồn tại"
                });
            }

            if (game.IsUpdateWin != true)
            {
                return BadRequest(new
                {
                    message =
                        "Bạn chỉ có thể hiển thị game khi game đã có thông tin hiển thị, tỷ lệ thắng giải và giải thưởng.\r\nVui lòng cài đặt đủ thông tin trước khi hiển thị"
                });
            }

            game.IsActive = isActive;
            await _gameService.Update(id, game);
            return Ok();
        }
    }
}