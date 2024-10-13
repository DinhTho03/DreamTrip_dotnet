using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("placeTourismGroup")]
public class PlaceTourismGroup
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string PlaceTourismCateId { get; set; }

    // Tên của nhóm địa điểm: Ví dụ khách sạn Luxury Chánh Kiệt
    public string Name { get; set; }

    // Mô tả ngắn gọn về nhóm địa điểm
    public string Description { get; set; }

    // Ngày tạo nhóm địa điểm
    public DateTime CreatedAt { get; set; }
    public int CreatedBy { get; set; }

    // Ngày cập nhật nhóm địa điểm lần cuối
    public DateTime UpdatedAt { get; set; }
    public int UpdatedBy { get; set; }

    // Trạng thái hoạt động của nhóm địa điểm
    public bool IsActive { get; set; }
    // Trạng thái xóa của nhóm địa điểm
    public bool IsDeleted { get; set; }
}