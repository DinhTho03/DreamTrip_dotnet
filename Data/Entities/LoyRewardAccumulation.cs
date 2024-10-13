using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;
[BsonConllection("loyRewardAccumulation")]
public class LoyRewardAccumulation
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    /// <summary>
    /// loy program
    /// </summary>
    public string? ProgramId { get; set; }

    /// <summary>
    /// id bảng loy_reward
    /// </summary>
    public string? RewardId { get; set; }

    /// <summary>
    /// id chương trình tiêu điểm
    /// </summary>
    public string? RewardProgramId { get; set; }

    /// <summary>
    /// id sản phẩm
    /// </summary>
    public string? AccumulationProgramId { get; set; }

    public int? Quota { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}