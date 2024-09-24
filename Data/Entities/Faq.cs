using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("faq")]
public class Faq
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    /// <summary>
    /// nhóm ? nếu type khác faq thì null
    /// </summary>
    public string? FaqGroup { get; set; }

    /// <summary>
    /// faq : hỏi đáp, policy : chính sach bảo mật, terms : điều khoản sử dụng
    /// </summary>
    public string? FaqType { get; set; }

    /// <summary>
    /// title
    /// </summary>
    public string? FaqTitle { get; set; }

    /// <summary>
    /// nội dung
    /// </summary>
    public string? FaqContent { get; set; }

    /// <summary>
    /// thứ tự hiển thị
    /// </summary>
    public uint? FaqPosition { get; set; }

    public bool? IsActived { get; set; }

    public bool? IsDeleted { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}
