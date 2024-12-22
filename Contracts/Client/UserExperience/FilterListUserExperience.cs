namespace brandportal_dotnet.Contracts.UserExperience;

public class FilterListUserExperience
{
    public string? Destination { get; set; }
    public double? PriceTotal { get; set; }
    public int? DayTotal { get; set; }
    public double? Rating { get; set; }
    public int CurrentPage { get; set; } = 1;
}