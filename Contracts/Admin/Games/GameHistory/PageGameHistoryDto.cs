namespace brandportal_dotnet.Contracts.Games.GameHistory;

public class PageGameHistoryDto
{
    public string Id { get; set ; }
    public string? UserName { get; set; }
    public string? CodeProgram { get; set; }
    public string? GameType { get; set; }
    public string? ProgramName { get; set; }
    public string? Result { get; set; }
    public string? NameGift { get; set; }
    public string? Unit { get; set; }
    public DateTime? DateTurn { get; set; } 
    public  string? StoreSearch { get; set; }
}