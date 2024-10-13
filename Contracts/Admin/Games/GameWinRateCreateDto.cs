namespace brandportal_dotnet.Contracts.Games;

public class GameWinRateCreateDto
{
    public int GameId { get; set; }
    public int? Times { get; set; }
    public sbyte? PercentWin { get; set; }
}