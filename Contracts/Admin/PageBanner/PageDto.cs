using System;
using System.Collections.Generic;

namespace brandportal_dotnet.Contracts.PageBanner;

public record PageDto
{
    public string Id { get; set; }
    public string? Type { get; set; }
    public string? PageName { get; set; }
    public sbyte? PageOrder { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public IEnumerable<string?> Banner { get; set; } = Array.Empty<string?>();
}
