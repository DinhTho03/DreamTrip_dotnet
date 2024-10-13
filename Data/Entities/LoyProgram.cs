using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("loyProgram")]
public class LoyProgram
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    /// <summary>
    /// Tên chương trình
    /// </summary>
    public string? Name { get; set; }

    /// <summary>
    /// Mô tả
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// số ngày chu kỳ
    /// </summary>
    public int? Periodically { get; set; }

    /// <summary>
    /// loại : hiện tại mặc định là day
    /// </summary>
    public string? PeriodicallyType { get; set; }

    /// <summary>
    /// Tên điểm loại phân hạng
    /// </summary>
    public string? NameAccumulationPoint { get; set; }

    /// <summary>
    /// Tên điểm loại tiêu điểm
    /// </summary>
    public string? NameAvailablePoint { get; set; }

    public bool? IsActive { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}