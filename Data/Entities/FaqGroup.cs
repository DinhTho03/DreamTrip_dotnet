using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using Volo.Abp.Domain.Entities;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("faqGroup")]
public class FaqGroup
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string? ParentId { get; set; }

    /// <summary>
    /// nếu loại là trang (dùng cho cms)
    /// </summary>
    public string? FaqGroupType { get; set; }

    /// <summary>
    /// tiêu đề
    /// </summary>
    public string? FaqGroupTitle { get; set; }

    /// <summary>
    /// vị trí hiển thị
    /// </summary>
    public uint? FaqGroupPosition { get; set; }

    public bool? IsActived { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}
