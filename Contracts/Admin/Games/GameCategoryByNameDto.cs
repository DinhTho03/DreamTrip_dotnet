namespace brandportal_dotnet.Contracts.Games
{
    public record GameCategoryByNameDto
    {
        public string Id { get; set; }
        public string? Name { get; set; }
    }
}
