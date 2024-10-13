namespace brandportal_dotnet.Contracts.Games;

public class GameRewardProgramDto
{
    public string? RewardProgramId { get; set; }
    public string? ProgramId { get; set; }
    public bool? ApplyLoyalty { get; set; }
    public string? BonusPlaysType { get; set; }
    public uint? PlayTurnNumber { get; set; }
    public string? CateType { get; set; }
}