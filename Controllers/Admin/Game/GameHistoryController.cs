using brandportal_dotnet.Contracts.Games.GameHistory;
using brandportal_dotnet.Data.Entities;
using brandportal_dotnet.IService;
using brandportal_dotnet.IService.IRewardProgram;
using brandportal_dotnet.Models;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp.Application.Dtos;
using brandportal_dotnet.shared;
namespace brandportal_dotnet.Controllers.game;

[Route("[controller]")]
[ApiController]
public class GameHistoryController
{
    private readonly IRepository<LoyProgram> _loyProgramRepository;
    private readonly IRepository<Game> _gameRepository;
    private readonly IRepository<Account> _accountRepository;
    private readonly IRepository<LoyRewardProgram> _loyRewardProgramRepository;
    private readonly IRepository<GameLog> _gameLogRepository;
    private readonly IRepository<LoyReward> _loyRewardRepository;
    private readonly IRewardProgramRepository<LoyRewardProduct> _loyRewardProductRepository;

    public GameHistoryController
    (
        IRepository<LoyProgram> loyProgramRepository,
        IRepository<Game> gameRepository, 
        IRepository<Account> accountRepository, 
        IRepository<LoyRewardProgram> loyRewardProgramRepository, 
        IRepository<GameLog> gameLogRepository, 
        IRepository<LoyReward> loyRewardRepository, 
        IRewardProgramRepository<LoyRewardProduct> loyRewardProductRepository 
        )
    {
        _loyProgramRepository = loyProgramRepository;
        _gameRepository = gameRepository;
        _accountRepository = accountRepository;
        _loyRewardProgramRepository = loyRewardProgramRepository;
        _gameLogRepository = gameLogRepository;
        _loyRewardRepository = loyRewardRepository;
        _loyRewardProductRepository = loyRewardProductRepository;
    }
    
     [HttpGet("~/api/game-turn-history/paged")]
    public async Task<PagedResultDto<PageGameHistoryDto>> GetListGameAsync(PagingRequestInput input)
    {
        // Get all queryable data asynchronously
        var gameLogQueryable = await _gameLogRepository.GetAll();
        var loyProgramQueryable = await _loyProgramRepository.GetAll();
        var gameQueryable = await _gameRepository.GetAll();
        var loyRewardProgramQueryable = await _loyRewardProgramRepository.GetAll();
        var loyRewardQueryable = await _loyRewardRepository.GetAll();
        var loyRewardProductQueryable = await _loyRewardProductRepository.GetAll();
        var accounts = await _accountRepository.GetAll();


        // Build the query
        var query = (from gameLog in gameLogQueryable
            join account in accounts on gameLog.UserId equals account._Id
            join loyProgram in loyProgramQueryable on gameLog.ProgramId equals loyProgram._Id
                into loyProgramQuery
            from loyProgram in loyProgramQuery.DefaultIfEmpty()
            join game in gameQueryable on gameLog.GameId equals game._Id
         
            join loyRewardProgram in loyRewardProgramQueryable on gameLog.RewardProgramId equals loyRewardProgram._Id
            join loyReward in loyRewardQueryable on loyRewardProgram.RewardId equals loyReward._Id
                into loyRewardGroup
            from loyReward in loyRewardGroup.DefaultIfEmpty()
            join loyRewardProduct in loyRewardProductQueryable on loyRewardProgram.RewardId equals loyRewardProduct
                    .RewardProgramId
                into loyRewardProductGroup
            from loyRewardProduct in loyRewardProductGroup.DefaultIfEmpty()
            orderby gameLog.CreatedAt descending
            select new PageGameHistoryDto
            {
                Id = gameLog._Id,
                CodeProgram = game.GameCode,
                UserName = account.FullName,
                GameType = game.CateType == "wheel" ? "Vòng xoay may mắn" : "",
                ProgramName = game.GameName,
                Result = gameLog.IsWin == true ? "Trúng giải" : "Không trúng giải",
                NameGift =  gameLog.IsWin == true ? loyRewardProgram.RewardProgramName : "",
                Unit = gameLog.IsWin == true && loyReward.Gift == "product" ? loyRewardProgram.RewardProgramDescription : "",
                DateTurn = gameLog.CreatedAt,
            }).Where(input);

        var items = query
            .OrderBy(input)
            .Page(input).ToList();
        var totalCount =  query.Count();
        return new PagedResultDto<PageGameHistoryDto>(totalCount, items);
    }
}