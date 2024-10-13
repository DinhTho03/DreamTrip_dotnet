namespace brandportal_dotnet.Contracts.Games;

public record GameRewardDto
{
    public string Id { get; set; }
    public bool? WinType { get; set; }
    public short? WinQuotaLimit { get; set; }
    public sbyte? NumbetWheel { get; set; }
    public IEnumerable<GameWinRateDto> WinRateData { get; set; } = [];
    public IEnumerable<RewardDto> ListRewardData { get; set; } = [];
}