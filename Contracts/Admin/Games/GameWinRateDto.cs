namespace brandportal_dotnet.Contracts.Games;

public record GameWinRateDto
{
    public string Id { get; set; }
    public int? Times { get; set; }
    public sbyte? PercentWin { get; set; }
}