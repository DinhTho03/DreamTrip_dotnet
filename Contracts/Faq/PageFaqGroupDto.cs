namespace brandportal_dotnet.Contracts.Faq
{
    public record PageFaqGroupDto
    {
        public string Id { get; set; }
        public string? FaqGroupTitle { get; set; }
        public string? FaqGroupParentTitle { get; set; }
        public uint? Position { get; set; }
        public bool? IsActived { get; set; }
        public DateTime? CreatedAt { get; set; }
    }
}
