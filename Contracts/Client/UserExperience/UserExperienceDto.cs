namespace brandportal_dotnet.Contracts.UserExperience;

public record UserExperienceDto
{
    public string? PlaceId { get; set; }
    public string? Name { get; set; }
    public string? Photos { get; set; }
    public double? Rating { get; set; }
    public string? Distance { get; set; }
    public int? User_ratings_total { get; set; }
    public string? Url { get; set; }
    public string? International_phone_number { get; set; }
    public bool? HasExperienced { get; set; }
    public string? Duration { get; set; }
    
}

public record UserExperienceDetail
{
    public string? Id { get; set; }
    public string? UserId { get; set; }
    public UserExperienceDto?[][] planDetail { get; set; }
    public string?[] DateInPlan { get; set; }
    public string? Departure { get; set; }
    public string? Destination { get; set; }
    public double? PriceTotal { get; set; }
    public string? avatar { get; set; }
    public string? name { get; set; }
    public DateTime? CreateDate { get; set; }
}

public record UserExperienceResponse
{
    public UserExperienceDetail[] UserExperience { get; set; }
    public int PageSize { get; set; }
    public int TotalPage { get; set; }
    public int CurrentPage { get; set; }
}