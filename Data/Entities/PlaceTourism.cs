using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("placeTourism")]
public class PlaceTourism
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string Name { get; set; }

    public string? PlaceTourismGroupId { get; set; }

    // Địa chỉ của địa điểm du lịch
    public string Location { get; set; }

    // Vĩ độ của địa điểm, dùng để tích hợp bản đồ
    public string? Latitude { get; set; }

    // Kinh độ của địa điểm, dùng để tích hợp bản đồ
    public string? Longitude { get; set; }

    // Mô tả chi tiết về địa điểm
    public string? Description { get; set; }

    // Đánh giá trung bình của địa điểm (thường từ 1-5 sao)
    public double? Rating { get; set; }

    // Giờ mở cửa của địa điểm (nếu có) 
    public string? OpeningTime { get; set; } // Giờ mở cửa
    // Giờ đóng cửa của địa điểm (nếu có) 
    public string? ClosingTime { get; set; } // Giờ đóng cửa


    // Phí thấp nhất khi vào cửa (nếu có) 
    public double? MinEntryFee { get; set; }
    // Phí cao nhất khi vào cửa (nếu có) 
    public double? MaxEntryFee { get; set; }

    // Ngày địa điểm được thêm vào hệ thống
    public DateTime? CreatedAt { get; set; }

    // Ngày cập nhật lần cuối thông tin địa điểm
    public DateTime? UpdatedAt { get; set; }

    // Trạng thái hoạt động của địa điểm du lịch
    public bool IsActive { get; set; }
    // Trạng thái xóa của địa điểm du lịch
    public bool IsDeleted { get; set; }
}