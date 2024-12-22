using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;
[BsonConllection("gameLog")]
public class GameLog
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string GameId { get; set; }

    public string UserId { get; set; }

    // public string OutletId { get; set; }

    /// <summary>
    /// chương trình loyalty
    /// </summary>
    public string? ProgramId { get; set; }

    /// <summary>
    /// hạng thành viên
    /// </summary>
    // public int? RankId { get; set; }

    /// <summary>
    /// quay cho lượt đổi nào nếu là dạng không cần đổi lượt quay
    /// </summary>
    // public int? TransferId { get; set; }

    /// <summary>
    /// nếu lượt quay đó trúng giải
    /// </summary>
    public bool? IsWin { get; set; }

    /// <summary>
    /// giải thưởng nếu trúng
    /// </summary>
    public string? RewardProgramId { get; set; }

    /// <summary>
    /// dùng trong trường hợp muốn xóa kết quả
    /// </summary>
    public bool? IsPublic { get; set; }

    /// <summary>
    /// Ngày chơi game. dùng để filter cho dễ
    /// </summary>
    public DateOnly? Date { get; set; }

    public DateTime? CreatedAt { get; set; }

    public DateTime? UpdatedAt { get; set; }

    /// <summary>
    /// Nhà Phân Phối
    /// </summary>
    // public int? CompanyBranchId { get; set; }
}