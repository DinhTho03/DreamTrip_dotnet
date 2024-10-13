namespace brandportal_dotnet.Contracts.Games;

public class GameRewardCreateDto
{
    public bool? WinType { get; set; }
    public short? WinQuotaLimit { get; set; }
    public sbyte? NumbetWheel { get; set; }
    public IEnumerable<GameWinRateCreateDto> WinRateData { get; set; } = [];
    public IEnumerable<GameRewardUpdateDto> ListRewardData { get; set; } = [];

}