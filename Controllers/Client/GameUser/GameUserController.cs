using brandportal_dotnet.Contracts.Client.GameUser;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IPageBanner;
using Microsoft.AspNetCore.Mvc;
using MongoDB.Driver.Linq;

namespace brandportal_dotnet.Controllers.Client.GameUser;

[Route("[controller]")]
[ApiController]
public class GameUserController : ControllerBase
{
    private readonly IRepository<LoyRewardProgramGame> _loyRewardProgramGameGroupService;
    private readonly IRepository<Account> _accountService;
    private readonly IRepository<GameRate> _gameRateGroupService;
    private readonly IRepository<Game> _gameService;
    private readonly IRepository<GameLog> _gameLogGroupService;
    private readonly IRepository<LoyRewardProgramGame> _loyRewardProgramGameService;
    private readonly IRepository<LoyRewardProgram> _loyRewardProgramService;
    private readonly IRepository<LoyNotification> _loyNotificationService;
    private readonly IRepository<LoyRewardAccumulation> _loyRewardAccumulationService;
    private readonly IRepository<LoyAccumulationProgram> _loyAccumulationProgramService;
    private readonly IPageBannerRepository<PageBanner> _pageBannerService;
    private readonly IPageBannerRepository<Page> _pageService;
    

    public GameUserController(
        IRepository<LoyRewardProgramGame> loyRewardProgramGameGroupService,
        IRepository<Account> accountService,
        IRepository<GameRate> gameRateGroupService,
        IRepository<Game> gameService,
        IRepository<GameLog> gameLogGroupService,
        IRepository<LoyRewardProgramGame> loyRewardProgramGameService,
        IRepository<LoyRewardProgram> loyRewardProgramService,
        IRepository<LoyNotification> loyNotificationService,
        IRepository<LoyAccumulationProgram> loyAccumulationProgramService,
        IRepository<LoyRewardAccumulation> loyRewardAccumulationService,
        IPageBannerRepository<PageBanner> pageBannerService,
        IPageBannerRepository<Page> pageService
    )
    {
        _loyRewardProgramGameGroupService = loyRewardProgramGameGroupService;
        _gameRateGroupService = gameRateGroupService;
        _accountService = accountService;
        _gameService = gameService;
        _gameLogGroupService = gameLogGroupService;
        _loyRewardProgramGameService = loyRewardProgramGameService;
        _loyRewardProgramService = loyRewardProgramService;
        _loyNotificationService = loyNotificationService;
        _loyAccumulationProgramService = loyAccumulationProgramService;
        _loyRewardAccumulationService = loyRewardAccumulationService;
        _pageBannerService = pageBannerService;
        _pageService = pageService;
    }

    [HttpGet("~/api/page-user/game/content/{id}")]
    public async Task<IActionResult> GetGameContentAsync(string id)
    {
        var loyRewardProgramGames =
            await _loyRewardProgramGameGroupService.FindListByProperties(new Dictionary<string, object>
                { { "GameId", id } });
        var data = from loyRewardProgramGame in loyRewardProgramGames
            select new GameUserDto
            {
                Id = loyRewardProgramGame._Id,
                RewardProgramId = loyRewardProgramGame.RewardProgramId,
                GameId = loyRewardProgramGame.GameId,
                Image = loyRewardProgramGame.Image,
                Background = loyRewardProgramGame.Background,
                Position = loyRewardProgramGame.Position,
                IsWin = loyRewardProgramGame.IsWin,
            };
        return Ok(data.OrderBy(x => x.Position));
    }

    [HttpGet("~/api/page-user/game/{gameId}")]
    public async Task<IActionResult> PlayGameContentAsync(string gameId, string userId)
    {
        var game = await _gameService.GetById(gameId);
        if (game == null)
        {
            return NotFound(new
            {
                message = "Không tìm thấy game này"
            });
        }

        if (game.IsActive  ==  false)
        {
            return BadRequest(new
            {
                message = "Game này đang không hoạt động"
            });
        }

        var gameLogs = await _gameLogGroupService.FindListByProperties(new Dictionary<string, object>
        {
            { "UserId", userId },
            { "GameId", gameId }
        });
        var rewardProgram = await _loyRewardProgramService.GetById(game.RewardProgramId);
        if (gameLogs.Count >= Int32.Parse(rewardProgram.QuotaLimitedExchangeOutlet))
        {
            return BadRequest(new { message  = "Số lượng chơi game đã đạt giới hạn"
            });
        }

        var user = await _accountService.GetById(userId);
        if (user == null)
        {
            return BadRequest(new { message  = "Người dùng không tồn tại"
            });
        }

        if (user.Point < rewardProgram.Point)
        {
            return BadRequest(new { message  = "Số đậu của bạn không đủ"
            });
        }

        user.Point -= rewardProgram.Point;
        var result = new PlayGameDto();
        // Lấy danh sách game logs của người dùng
        var gameLog = await _gameLogGroupService.FindListByProperties(new Dictionary<string, object>
        {
            { "UserId", userId }
        });

        // Số lần người dùng đã chơi
        var numberPlayGame = gameLog.Count() + 1;

        // Lấy danh sách game rates
        var gameRates = await _gameRateGroupService.FindListByProperties(new Dictionary<string, object>
        {
            { "GameId", gameId }
        });

        // Tìm tỷ lệ thắng phù hợp với số lần chơi
        var currentRate = gameRates.FirstOrDefault(gr => gr.Times == numberPlayGame);

        // Nếu không có tỷ lệ cụ thể, sử dụng tỷ lệ mặc định (Times = -1)
        if (currentRate == null)
        {
            currentRate = gameRates.FirstOrDefault(gr => gr.Times == -1);
        }

        // Xác định tỷ lệ phần trăm thắng
        var percentWin = currentRate?.PercentWin ?? 0;

        var random = new Random();
        var randomValue = random.Next(0, 101); // Tạo số ngẫu nhiên từ 0–100
        var isWin = randomValue <= percentWin;
        if (isWin)
        {
            var loyRewardProgramGameService = await _loyRewardProgramGameService.FindListByProperties(
                new Dictionary<string, object>
                {
                    { "GameId", gameId },
                    { "IsWin", true }
                });
            var rewardProgramIds = loyRewardProgramGameService
                .Where(x => x.RewardProgramId != null)
                .Select(x => x.RewardProgramId)
                .Distinct()
                .ToList();

            var loyRewardProgram = await _loyRewardProgramService.FindListByListId(rewardProgramIds);


            var filterLoyRewardProgram = loyRewardProgram.Where(x => x.QuotaLimitedExchangeAll > 0).ToList();
            if (filterLoyRewardProgram.Count == 0)
            {
                return BadRequest(new
                {
                    message = "Số lượng quà tặng đã hết"
                });
            }


            var randomRewardProgram = filterLoyRewardProgram[random.Next(filterLoyRewardProgram.Count)];

            filterLoyRewardProgram.FirstOrDefault(x => x._Id == randomRewardProgram._Id).QuotaLimitedExchangeAll -= 1;

            var loyAccumulationProgram =
                await _loyAccumulationProgramService.GetById(
                    "66f926f11f4d8fa281acd3d2"); // chỉ dùng cho lựa chọn điểm thưởng

            user.Point = user.Point + (rewardProgram.Point * loyAccumulationProgram.AccumulationPoint);
            await _loyRewardProgramService.Update(randomRewardProgram._Id,
                filterLoyRewardProgram.FirstOrDefault(x => x._Id == randomRewardProgram._Id));
            if (randomRewardProgram != null)
            {
                var gameLogData = new GameLog
                {
                    GameId = gameId,
                    UserId = userId,
                    ProgramId = randomRewardProgram.ProgramId,
                    IsWin = true,
                    RewardProgramId = randomRewardProgram._Id,
                    IsPublic = true,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var noti = await _loyNotificationService.FindByProperties(new Dictionary<string, object>
                    { { "ObjectSubId", randomRewardProgram._Id } });
                if (noti != null)
                {
                    result = new PlayGameDto
                    {
                        Id = loyRewardProgramGameService
                            .FirstOrDefault(x => x.RewardProgramId == randomRewardProgram._Id)._Id,
                        Name = noti.TitleShow,
                        Description = noti.Description.Replace("[:point]", randomRewardProgram.RewardProgramName)
                            .Replace("[:outlet_point]", user.Point.ToString()),
                        Image = noti.Background,
                        GameId = noti.ObjectId,
                    };
                }

                await _accountService.Update(user._Id, user);
                await _gameLogGroupService.Insert(gameLogData);
            }
        }
        else
        {
            var loyRewardProgramGameService = await _loyRewardProgramGameService.FindListByProperties(
                new Dictionary<string, object>
                {
                    { "GameId", gameId },
                    { "IsWin", false }
                });
            var randomRewardProgramGame = loyRewardProgramGameService[random.Next(loyRewardProgramGameService.Count)];
            var randomRewardProgram = await _loyRewardProgramService.GetById(randomRewardProgramGame.RewardProgramId);
            if (randomRewardProgram != null)
            {
                var gameLogData = new GameLog
                {
                    GameId = gameId,
                    UserId = userId,
                    ProgramId = randomRewardProgram.ProgramId,
                    IsWin = false,
                    RewardProgramId = randomRewardProgramGame.RewardProgramId,
                    IsPublic = true,
                    Date = DateOnly.FromDateTime(DateTime.Now),
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now
                };

                var noti = await _loyNotificationService.FindByProperties(new Dictionary<string, object>
                    { { "ObjectSubId", randomRewardProgramGame.RewardProgramId } });

                if (noti != null)
                {
                    result = new PlayGameDto
                    {
                        Id = randomRewardProgramGame._Id,
                        Name = noti.TitleShow,
                        Description = noti.Description,
                        Image = noti.Background,
                        GameId = noti.ObjectId,
                    };
                }

                await _accountService.Update(user._Id, user);
                await _gameLogGroupService.Insert(gameLogData);
            }
        }

        return Ok(result);
    }

    [HttpGet("~/api/page-user/game/log/{userId}")]
    public async Task<IActionResult> GetGameLogAsync(string userId, string gameId)
    {
        var game = await _gameService.GetById(gameId);
        if (game == null)
        {
            return NotFound();
        }

        var rewardProgram = await _loyRewardProgramService.GetById(game.RewardProgramId);


        var gameLogs = await _gameLogGroupService.FindListByProperties(new Dictionary<string, object>
        {
            { "UserId", userId },
            { "GameId", gameId }
        });
        var loyRewardPrograms = await _loyRewardProgramService.GetAll();
        // if (gameLogs.Count >= Int32.Parse(rewardProgram.QuotaLimitedExchangeOutlet))
        // {
        //     return BadRequest(new { message  = "Số lượng chơi game đã đạt giới hạn"
        //     });
        // }
        var data = from gameLog in gameLogs
            join loyRewardProgram in loyRewardPrograms on gameLog.RewardProgramId equals loyRewardProgram._Id
            select new GameLogDto
            {
                GameId = gameLog.GameId,
                UserId = gameLog.UserId,
                IsWin = gameLog.IsWin,
                RewardProgramId = gameLog.RewardProgramId,
                IsPublic = gameLog.IsPublic,
                Date = gameLog.Date,
                CreatedAt = gameLog.CreatedAt,
                UpdatedAt = gameLog.UpdatedAt,
                ProductName = loyRewardProgram.RewardProgramName
            };

        var result = new GameLogResult
        {
            GameLogDto = data.ToArray(),
            QuotaLimited = Int32.Parse(rewardProgram.QuotaLimitedExchangeOutlet) - gameLogs.Count
        };
        return Ok(result);
    }

    [HttpGet("~/api/page-user/game/point/{gameId}")]
    public async Task<IActionResult> GetPointToPlayGameAsync(string gameId)
    {
        var gameService = await _gameService.GetById(gameId);
        if (gameService == null)
        {
            return BadRequest(new { message  = "Game không tồn tại"
            });
        }

        var rewardProgram = await _loyRewardProgramService.GetById(gameService.RewardProgramId!);
        if (rewardProgram == null)
        {
            return BadRequest(new { message  = "Chương trình không tồn tại"
            });
        }

        var result = new PointDto
        {
            Id = rewardProgram._Id,
            Point = rewardProgram.Point
        };
        return Ok(result);
    }
    
    [HttpGet("~/api/gameWheel")]
    public async Task<List<ListGameBanner>> GetListGameAsync(string? searchTitle)
    {
        
        // Banner nhiệm vụ nổi bật
        var page = await _pageService.FindByProperties(new Dictionary<string, object> { { "Type", "slide_CTA" } });
        var banners = await _pageBannerService.GetAll();
        var data = from banner in banners
            join game in await _gameService.GetAll() on banner.EndpointId equals game._Id
            join rewardProgram in await _loyRewardProgramService.GetAll() on game.RewardProgramId equals rewardProgram._Id
            join rewardProgramGame in await _loyRewardProgramGameService.GetAll() on game._Id equals rewardProgramGame.GameId
            where banner.IsActive == true && banner.PageId == page._Id
            group rewardProgramGame by new { banner, game, rewardProgram } into grouped
            select new ListGameBanner
            {
                Id = grouped.Key.banner._Id,
                Name = grouped.Key.banner.PageTitle,
                Photos = grouped.Key.banner.PageImg,
                Description = grouped.Key.rewardProgram.RewardProgramDescription,
                GameId = grouped.Key.banner.EndpointId,
                PageOrder = grouped.Key.banner.PageOrder,
                Point = grouped.Key.rewardProgram.Point,
                StartDate = grouped.Key.game.StartDate,
                EndDate = grouped.Key.game.EndDate,
                CellNumber = grouped.Count() // Đếm số lượng phần tử trong group
            };

        if (!string.IsNullOrEmpty(searchTitle))
        {
            data = data.Where(x => x.Name != null && x.Name.Contains(searchTitle, StringComparison.OrdinalIgnoreCase));
        }

        return data.OrderBy(x => x.PageOrder).ToList();
    }
    
}