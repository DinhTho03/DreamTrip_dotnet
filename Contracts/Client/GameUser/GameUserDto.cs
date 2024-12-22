namespace brandportal_dotnet.Contracts.Client.GameUser;

public class GameUserDto
{
    public string Id { get; set; }
    public string? RewardProgramId { get; set; }

    public string? GameId { get; set; }

    public string? Image { get; set; }

    public string? Background { get; set; }

    public short? Position { get; set; }

    public bool? IsWin { get; set; }

}