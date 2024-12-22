namespace brandportal_dotnet.Contracts.Client.GameUser;

public class ListGameBanner
{
    public string Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    public string Photos { get; set; }
    public string GameId { get; set; }
    public int? PageOrder { get; set; }

    public int? Point { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int CellNumber { get; set; }
}

