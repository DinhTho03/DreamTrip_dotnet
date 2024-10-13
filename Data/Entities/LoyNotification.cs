using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("loyNotification")]
public class LoyNotification
{ 
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    /// <summary>
    /// game : game, accumulated : tích lũy, reward_program : chương trình tiêu điểm
    /// </summary>
    public string? NotificationType { get; set; }

    /// <summary>
    /// dùng trong brand, vị trí hiện tại làm key
    /// </summary>
    public int? SubId { get; set; }

    /// <summary>
    /// id theo chương trình
    /// </summary>
    public string? ObjectId { get; set; }

    /// <summary>
    /// id theo quà, con của object_id
    /// </summary>
    public string? ObjectSubId { get; set; }

    /// <summary>
    /// tiêu đề, vd Chúc mừng bạn trúng giải [:product_name:]
    /// </summary>
    public string? Title { get; set; }

    public string? TitleShow { get; set; }

    /// <summary>
    /// mô tả, vd Chúc mừng bạn trúng giải [:product_name:]
    /// </summary>
    public string? Description { get; set; }

    public string? DescriptionShow { get; set; }

    public string? Background { get; set; }

    /// <summary>
    /// params dùng để truyền vào text, kiểu json {cột trong db : giá trị trong title hoặc description}, vd {&quot;product_name&quot; : &quot;[:product_name:]&quot;}
    /// </summary>
    public string? ParamsShow { get; set; }

    /// <summary>
    /// thông báo này có gửi noti khong ? 1 có gửi noti, sử dụng key_notify để lấy params
    /// </summary>
    public sbyte? IsNotify { get; set; }

    /// <summary>
    /// Nếu thông báo có hành động ở nút, 1 có hành động dùng key_action để di chuyển
    /// </summary>
    public sbyte? IsAction { get; set; }

    /// <summary>
    /// key ở bảng dmspro_mys_template_notification
    /// </summary>
    public string? Key { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public int? UpdatedBy { get; set; }
}