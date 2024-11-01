using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("notificationPage")]
public class NotificationPage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string? Title { get; set; }
    public string? Description { get; set; }
    public string Type { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public bool IsActive { get; set; }
    public string PageId { get; set; }
}