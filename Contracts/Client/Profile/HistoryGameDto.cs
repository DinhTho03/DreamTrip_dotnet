namespace brandportal_dotnet.Contracts.Client.Profile;

public class HistoryGameDto
{
    public string Id { get; set; }
    public string GameName { get; set; }
    public int? point { get; set; }
    public bool? IsWin { get; set; }
    public string? ProductName { get; set; }
    public DateTime? CreatedAt { get; set; }
}