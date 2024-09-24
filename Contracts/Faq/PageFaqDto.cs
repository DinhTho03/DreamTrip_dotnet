namespace brandportal_dotnet.Contracts.Faq
{
    public record PageFaqDto
    {
        public string Id { get; set; }
        public string? Title { get; set; }
        public string GroupId { get; set; }
        public string? GroupTitle { get; set; }
        public uint? Position { get; set; }
        public bool? IsActived { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
