namespace brandportal_dotnet.Contracts.Client.FAQ;

public record FAQDto
{
    public string Id { get; set; }

    public string? FaqGroup { get; set; }

    public string? FaqType { get; set; }

    public string? FaqTitle { get; set; }

    public string? FaqContent { get; set; }

    public uint? FaqPosition { get; set; }

    public bool? IsActived { get; set; }
}

public record FAQGroupDto
{
    public string _Id { get; set; }

    public string? FaqGroupName { get; set; }

    public uint? FaqGroupPosition { get; set; }

    public bool? IsActived { get; set; }
    public FAQDto[] FAQ { get; set; }
}

public record FAQGroupResponseDto
{
    public FAQGroupDto[] FaqGroup { get; set; }
}

