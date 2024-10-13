using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("loyReward")]
public class LoyReward
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    /// <summary>
    ///  discount: mã giảm giá, point: điểm tích lũy, gift : 1 loại quà tặng nào đó tự khao báo
    /// </summary>
    public string? Gift { get; set; }

    public string? Icon { get; set; }

    public string? Name { get; set; }

    /// <summary>
    /// 1 hiển thị list ở game vòng xoay, 0 ko hiển thị
    /// </summary>
    public bool? GameWheel { get; set; }

    public bool? IsWin { get; set; }

    public DateTime? CreatedAt { get; set; }
}