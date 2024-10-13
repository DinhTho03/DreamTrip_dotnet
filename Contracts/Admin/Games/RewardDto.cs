namespace brandportal_dotnet.Contracts.Games;

public class RewardDto
{
    public string Id { get; set; }
    public string? RewardProgramGameId { get; set; }
    public string? RewardProductId { get; set; }
    public string? ProductId { get; set; }
    public string? ProductUomId { get; set; }
    public string? RewardAccumulationId { get; set; }
    public uint? WinRate { get; set; }
    public string? AccumulationProgramId { get; set; }
    public string? RewardType { get; set; }
    public short? Position { get; set; }
    public string? ProductName { get; set; }
    public string? ProductCode { get; set; }
    public string? RewardGiftId { get; set; }
    public string? GiftType { get; set; }
    public string? CateType { get; set; }
    public string? GiftName { get; set; }
    public string? GiftUom { get; set; }
    public int? QuotaGift { get; set; }
    public string? AccumulationProgramName { get; set; }
    public int? QuotaProduct { get; set; }
    public string? UnitCode { get; set; }
    public string UnitName { get; set; }   
    public sbyte? QuotaChange { get; set; }
    public uint? TotalBudgetQuota { get; set; }
    public string? Value { get; set; }
    public int? QuotaLimitedExchangeAll { get; set; }
    public bool? IsActive { get; set; }
    public string? Background { get; set; }
    public string? Image { get; set; }
    public string? ProductDisplayName { get; set; }
    public string? UomDisplayName { get; set; }
    public GameNotificationCreateDto GameNotificationData { get; set; }
}