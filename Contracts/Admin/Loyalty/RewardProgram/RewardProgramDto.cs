using System;
using System.Collections.Generic;

namespace brandportal_dotnet.Contracts.Loyalty.RewardProgram;

public record RewardProgramDto
{
    public string Id { get; set; }

    public string? RewardProgramCode { get; set; }

    public string? RewardProgramName { get; set; }

    public string? RewardId { get; set; }

    public string? ProgramName { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public int? Point { get; set; }

    public string? MembershipType { get; set; }

    public bool? IsActive { get; set; }

    public int? RedeemSuccess { get; set; }

    public bool? IsLimitedExchangeAll { get; set; }

    public int? QuotaLimitedExchangeAll { get; set; }
}

public record RewardProgramPagedDto
{
    public string Id { get; set; }

    public string? RewardProgramCode { get; set; }

    public string? RewardProgramName { get; set; }

    public string? RewardId { get; set; }

    public string? ProgramName { get; set; }

    public DateTime? DateStart { get; set; }

    public DateTime? DateEnd { get; set; }

    public int? Point { get; set; }

    public string? MembershipType { get; set; }

    public bool? IsActive { get; set; }

    public int? RedeemSuccess { get; set; }

    public bool? IsLimitedExchangeAll { get; set; }

    public int? QuotaLimitedExchangeAll { get; set; }

    public string? LimitedExchangeType { get; set; }

    public string? SourcePointKey { get; set; }
}

public record RewardProgramProductDto
{
    public long? Id { get; set; }

    public string? ProductName { get; set; }
}


public record RewardProgramUomDto
{
    public long? Id { get; set; }

    public string? UomName { get; set; }
}

public record RewardProgramRankDto
{
    public long? Id { get; set; }

    public string? RankName { get; set; }
}

public record RewardProgramCreateDto
{
    public required string ProgramType { get; set; }

    public string? ProgramId { get; set; }

    public string? RewardProgramCode { get; set; }

    public required string RewardProgramName { get; set; }

    public string? RewardProgramDescription { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDisplay { get; set; }

    public bool? IsLimitedExchangeAll { get; set; }

    public int? QuotaLimitedExchangeAll { get; set; }

    public bool? IsLimitedExchangeOutlet { get; set; }

    public int? QuotaLimitedExchangeOutlet { get; set; }

    public string? MembershipType { get; set; }

    public int? Point { get; set; }

    public string? Banner { get; set; }

    public string? ConfigTurn { get; set; }

    public string? RewardId { get; set; }

    public string? LimitedExchangeType { get; set; }

    public sbyte? QuotaChange { get; set; }

    public string? ProductId { get; set; }

    public string? UomId { get; set; }

    public bool? ApplyLoyalty { get; set; }

    public string? BonusPlaysType { get; set; }

    public uint? PlayTurnNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public List<RankItemDto> RankItems { get; set; } = [];
}

public record RankItemDto
{
    public int? Id { get; set; }

    public int RankId { get; set; }

    public int? Point { get; set; }
}

public record RewardProgramDetailDto
{
    public string Id { get; set; }

    public string? ProgramType { get; set; }

    public string? ProgramId { get; set; }

    public string? RewardProgramCode { get; set; }

    public string? RewardProgramName { get; set; }

    public string? RewardProgramDescription { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDisplay { get; set; }

    public bool? IsLimitedExchangeAll { get; set; }

    public int? QuotaLimitedExchangeAll { get; set; }

    public bool? IsLimitedExchangeOutlet { get; set; }

    public int? QuotaLimitedExchangeOutlet { get; set; }

    public string? MembershipType { get; set; }

    public int? Point { get; set; }

    public string? Banner { get; set; }

    public string? ConfigTurn { get; set; }

    public string? RewardId { get; set; }

    public string? LimitedExchangeType { get; set; }

    public sbyte? QuotaChange { get; set; }

    public int? ProductId { get; set; }

    public int? UomId { get; set; }

    public bool? ApplyLoyalty { get; set; }

    public string? BonusPlaysType { get; set; }

    public uint? PlayTurnNumber { get; set; }

    public DateTime? StartDate { get; set; }

    public DateTime? EndDate { get; set; }

    public List<RankItemDto> RankItems { get; set; } = [];
}

public record RewardProgramBranchDto
{
    public int Id { get; set; }

    public string? BranchCode { get; set; }

    public string? BranchName { get; set; }

    public string? CompanyName { get; set; }

    public string? CompanyCode { get; set; }
}

public record GiftLimitCreateDto
{
    public int BranchId { get; set; }

    public int? Quota { get; set; }
}

public record PageGiftLimitExcelDto
{
    public string CompanyBranchCode { get; init; } = string.Empty;

    public string? LimitedExchangeQuota { get; set; }

    public string? CompanyBranchName { get; init; }

    public string? ErrorMessage { get; init; }
}

public record GiftLimitPagedDto
{
    public long Id { get; set; }

    public int? Quota { get; set; }

    public string? BranchCode { get; set; }

    public string? BranchName { get; set; }

    public int? RedeemSuccess { get; set; }
}

//public class GiftLimitTransactionDto
//{
//    public int Id { get; set; }
//    public string? Code { get; set; }

//    public string? Description { get; set; }

//    public string? Type { get; set; }

//    public DateTime? CreatedAt { get; set; }

//    public string? ErrorDescription { get; set; }

//    public string? LinkResult { get; set; }

//    public int? Size { get; set; }

//    public ImportExportStatus? Status { get; set; }
//}