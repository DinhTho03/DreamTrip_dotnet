using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;
[BsonConllection("loyRewardProgramGame")]
public class LoyRewardProgramGame
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string? RewardProgramId { get; set; }

    /// <summary>
    /// id game
    /// </summary>
    public string? GameId { get; set; }

    public string? Image { get; set; }

    public string? Value { get; set; }

    public string? Background { get; set; }

    /// <summary>
    /// vị trí
    /// </summary>
    public short? Position { get; set; }

    /// <summary>
    /// Quà này là loại trúng thưởng không
    /// </summary>
    public bool? IsWin { get; set; }

    /// <summary>
    /// Hiển thị trên trang danh sách đổi điểm
    /// </summary>
    public bool? IsDisplay { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Số lượng quà tặng trong 1 phần quà
    /// </summary>
    public int? QuantityUnit { get; set; }

    /// <summary>
    /// Phần trăm trúng thưởng. Áp dụng game NPP
    /// </summary>
    public uint? WinRate { get; set; }

    /// <summary>
    /// Quota Khi áp dụng ngân sách tổng
    /// </summary>
    public uint? TotalBudgetQuota { get; set; }
}