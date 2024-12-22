using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("account")]
public class ShareTripPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string Id { get; set; } // ID chính của ShareTripPlan
    public string UserId { get; set; } // ID người dùng liên quan đến kế hoạch
    public string ParentId { get; set; } // ID cha (nếu có)
    
    // Các thuộc tính bổ sung có thể được thêm vào tùy nhu cầu
    public string PlanName { get; set; } // Tên của kế hoạch
    public DateTime CreatedDate { get; set; } // Ngày tạo kế hoạch

    // Constructor mặc định
    public ShareTripPlan()
    {
        CreatedDate = DateTime.UtcNow;
    }
}