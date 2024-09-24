using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text.Json;
using System.Text.Json.Serialization;
using brandportal_dotnet.Common;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Localization;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Localization.Resources.AbpDdd;


namespace brandportal_dotnet.Models;

[PublicAPI]
public record PagingRequestInput : IFilterInput, ISortInput, IPagingInput, IValidatableObject
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
    public int PageSize { get; set; } = PageDefaultConstants.DefaultPageSize;

    /// <summary>
    /// Filter
    /// </summary>
    [JsonIgnore]
    public ColumnFilter[] Filters =>
        !string.IsNullOrWhiteSpace(Filter)
            ? JsonSerializer.Deserialize<ColumnFilter[]>(Filter) ?? Array.Empty<ColumnFilter>()
            : Array.Empty<ColumnFilter>();
    
    public virtual IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
    {
        if (PageSize > LimitedResultRequestDto.MaxMaxResultCount)
        {
            var localizer =
                validationContext.GetRequiredService<IStringLocalizer<AbpDddApplicationContractsResource>>();

            yield return new ValidationResult(
                localizer[
                    "MaxResultCountExceededExceptionMessage",
                    nameof(PageSize),
                    LimitedResultRequestDto.MaxMaxResultCount,
                    typeof(LimitedResultRequestDto).FullName!,
                    nameof(LimitedResultRequestDto.MaxMaxResultCount)
                ],
                new[] { nameof(PageSize) });
        }
    }
}