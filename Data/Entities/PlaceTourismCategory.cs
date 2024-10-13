using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("placeTourismCategory")]
public class PlaceTourismCategory
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    // Tên của nhóm địa điểm: Ví dụ khách sạn, nhà hàng, quán cafe, ...
    public string Name { get; set; }
    
    public string Type { get; set; }

    // Ngày tạo nhóm địa điểm
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    // Ngày cập nhật nhóm địa điểm lần cuối
    public DateTime UpdatedAt { get; set; }
    public int UpdatedBy { get; set; }
}