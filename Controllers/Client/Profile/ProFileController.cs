using brandportal_dotnet.Constant;
using brandportal_dotnet.Contracts.Client.Profile;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using Microsoft.AspNetCore.Mvc;

namespace brandportal_dotnet.Controllers.Client.Profile;

[Route("[controller]")]
[ApiController]
public class ProFileController : ControllerBase
{
    private readonly IRepository<Account> _accountService;
    private readonly IRepository<GameLog> _gameLogService;
    private readonly IRepository<Payment> _paymentService;
    private readonly IRepository<Game> _gameService;
    private readonly IRepository<GroupTripPlan> _groupTripPlanService;
    private readonly IRepository<DetailTripPlan> _detailTripPlanService;
    private readonly IRepository<LoyRewardProgram> _loyRewardProgramRepository;

    public ProFileController
    (
        IRepository<Account> accountService,
        IRepository<GameLog> gameLogService,
        IRepository<Payment> paymentService,
        IRepository<Game> gameService,
        IRepository<GroupTripPlan> groupTripPlanService,
        IRepository<DetailTripPlan> detailTripPlanService,
        IRepository<LoyRewardProgram> loyRewardProgramRepository
    )
    {
        _accountService = accountService;
        _gameLogService = gameLogService;
        _paymentService = paymentService;
        _gameService = gameService;
        _groupTripPlanService = groupTripPlanService;
        _detailTripPlanService = detailTripPlanService;
        _loyRewardProgramRepository = loyRewardProgramRepository;
    }

    [HttpGet("~/api/page-user/profile")]
    public async Task<IActionResult> GetProfileDataAsync([FromQuery] string email)
    {
        var account = await _accountService.FindByProperties(new Dictionary<string, object> { { "Email", email } });
        if (account == null)
        {
            return NotFound();
        }

        var data = new ProFileDto
        {
            Id = account._Id,
            Name = account.FullName,
            Email = account.Email!,
            Phone = account.Phone,
            Avatar = account.Avatar,
            Point = account.Point
        };
        return Ok(data);
    }

    [HttpPut("~/api/page-user/profile/{id}")]
    public async Task<IActionResult> GetProfileDataAsync(string id, UpdateAccountDto data)
    {
        var account = await _accountService.GetById(id);
        if (!string.IsNullOrEmpty(data.NewPassword) || !string.IsNullOrEmpty(data.ConfirmPassword) ||
            !string.IsNullOrEmpty(data.OldPassword))
        {
            var checkPassword = BCrypt.Net.BCrypt.Verify(data.OldPassword, account.Password);
            if (!checkPassword)
            {
                return BadRequest(new
                {
                    message = "Mật khẩu cũ không đúng"
                });
            }

            if (data.NewPassword == data.ConfirmPassword)
            {
                var password = BCrypt.Net.BCrypt.HashPassword(data.NewPassword, Constants.PasswordSalt);
                account.Password = password;
                await _accountService.Update(account._Id, account);
            }
            else
            {
                return BadRequest(new
                {
                    message = "Mật khẩu không khớp"
                });
            }
        }
        else
        {
            account.FullName = data.Name;
            account.Avatar = data.Avatar;
            account.Phone = data.Phone;
            await _accountService.Update(account._Id, account);
        }


        return Ok(data);
    }


    [HttpGet("~/api/page-user/history-game")]
    public async Task<IActionResult> GetHistoryGame([FromQuery] string email)
    {
        var account = await _accountService.FindByProperties(new Dictionary<string, object> { { "Email", email } });
        if (account == null)
        {
            return NotFound();
        }

        var data = from gameLog in await _gameLogService.GetAll()
            join game in await _gameService.GetAll() on gameLog.GameId equals game._Id
            join loyRewardProgram in await _loyRewardProgramRepository.GetAll() on game.RewardProgramId equals
                loyRewardProgram._Id
            join loyReward in await _loyRewardProgramRepository.GetAll() on gameLog.RewardProgramId equals loyReward._Id
            where gameLog.UserId == account._Id
            select new HistoryGameDto
            {
                Id = gameLog._Id,
                GameName = game.GameName,
                IsWin = gameLog.IsWin,
                ProductName = loyReward.RewardProgramName,
                CreatedAt = gameLog.CreatedAt,
                point = loyRewardProgram.Point
            };
        return Ok(data.OrderByDescending(x => x.CreatedAt).ToList());
    }

    [HttpGet("~/api/page-user/transaction")]
    public async Task<IActionResult> GetTransaction([FromQuery] string email)
    {
        var TransactionDto = new List<TransactionDto>();
        var account = await _accountService.FindByProperties(new Dictionary<string, object> { { "Email", email } });
        if (account == null)
        {
            return NotFound();
        }

        var historyGame = from gameLog in await _gameLogService.GetAll()
            join game in await _gameService.GetAll() on gameLog.GameId equals game._Id
            join loyRewardProgram in await _loyRewardProgramRepository.GetAll() on game.RewardProgramId equals
                loyRewardProgram._Id
            join loyReward in await _loyRewardProgramRepository.GetAll() on gameLog.RewardProgramId equals loyReward._Id
            where gameLog.UserId == account._Id
            select new TransactionDto
            {
                Id = gameLog._Id,
                TransactionName = game.GameName,
                Points = gameLog.IsWin.HasValue && gameLog.IsWin.Value
                    ? loyRewardProgram.Point
                    : -loyRewardProgram.Point,
                TransactionTime = gameLog.CreatedAt,
                TransactionType = "Chơi game"
            };
        TransactionDto.AddRange(historyGame);
        var payment = await _paymentService.FindListByProperties(new Dictionary<string, object>
            { { "UserId", account._Id } });
        var historyPayment = from item in payment
            select new TransactionDto
            {
                Id = item._Id,
                TransactionName = item.OrderInfo,
                Points = item.Point,
                TransactionTime = item.PaymentDate,
                TransactionType = "Nạp ví",
            };
        TransactionDto.AddRange(historyPayment);
        var userPlan = await _groupTripPlanService.FindListByProperties(new Dictionary<string, object>
            { { "UserExperienceId", account._Id } });
        var historyUserPlan = from item in userPlan
            select new TransactionDto
            {
                Id = item._Id,
                TransactionName = "Tạo lịch trình",
                Points = -5,
                TransactionTime = item.CreatedAt,
                TransactionType = "Lịch trình công khai",
            };
        TransactionDto.AddRange(historyUserPlan);
        var createPlan = await _groupTripPlanService.FindListByProperties(new Dictionary<string, object>
            { { "UserId", account._Id } });
        var historyCreatePlan = from item in createPlan
            select new TransactionDto
            {
                Id = item._Id,
                TransactionName = "Tạo lịch trình",
                Points = -5,
                TransactionTime = item.CreatedAt,
                TransactionType = "Lịch trình từ Google",
            };
        TransactionDto.AddRange(historyCreatePlan);
        var detailPlans = from groupPlan in await _groupTripPlanService.GetAll()
            join detailplan in await _detailTripPlanService.GetAll() on groupPlan._Id equals detailplan.GroupTripPlanId
            where groupPlan.UserId == account._Id
            select detailplan;
        var statisticsTransactionDto = new StatisticsTransactionDto
        {
            TotalPoints = historyPayment?.Sum(x => x.Points.GetValueOrDefault()) ?? 0,
            TotalCreatePlans = createPlan?.Count() ?? 0,
            TotalPlace = detailPlans?.Count() ?? 0,
            TotalExpenses = detailPlans?.Where(x => x.Price != null).Sum(x => decimal.Parse(x.Price)) ?? 0
        };
        return Ok(new StatisticsTransactionResponse
        {
            Transactions = TransactionDto.OrderByDescending(x => x.TransactionTime).ToList(),
            StatisticsTransaction = statisticsTransactionDto
        });
    }
}