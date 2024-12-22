namespace brandportal_dotnet.Contracts.Client.GameUser;

public class GameLogDto
{
    public string GameId { get; set; }
    public string UserId { get; set; }
    public bool? IsWin { get; set; }
    public string? RewardProgramId { get; set; }
    public bool? IsPublic { get; set; }
    public DateOnly? Date { get; set; }
    public DateTime? CreatedAt { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string ProductName { get; set; }
    
}

public class GameLogResult
{
    public int QuotaLimited { get; set; }
    public GameLogDto[] GameLogDto { get; set; }
}