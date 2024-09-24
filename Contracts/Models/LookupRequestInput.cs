using System;
using System.Text.Json;
using System.Text.Json.Serialization;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using brandportal_dotnet.Common;

namespace brandportal_dotnet.Models;

[PublicAPI]
public record LookupRequestInput: IFilterInput, ISortInput, IPagingInput
{
    /// <summary>
    /// Filter string
    /// </summary>
    [FromQuery(Name = "filter")]
    public string? Filter { get; set; }
    
    
    /// <summary>
    /// Sort direction: asc, desc
    /// </summary>
    public string? Direction { get; set; }
    
    /// <summary>
    /// Sort active column
    /// </summary>
    public string? Active { get; set; }

    /// <summary>
    /// Page index
    /// </summary>
    [FromQuery(Name = "pageIndex")]
    public int PageIndex { get; set; } = PageDefaultConstants.DefaultPageIndex;

    /// <summary>
    /// Page size
    /// </summary>
    [FromQuery(Name = "pageSize")]
    public int PageSize { get; set; } = int.MaxValue;

    /// <summary>
    /// Filter
    /// </summary>
    [JsonIgnore]
    public ColumnFilter[] Filters =>
        !string.IsNullOrWhiteSpace(Filter)
            ? JsonSerializer.Deserialize<ColumnFilter[]>(Filter) ?? Array.Empty<ColumnFilter>()
            : Array.Empty<ColumnFilter>();
}