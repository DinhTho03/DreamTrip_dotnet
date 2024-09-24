using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using brandportal_dotnet.Data.Utils;

namespace TravelItineraryProject.Data.Entities;
[BsonConllection("comments")]
public class Comments
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string text { get; set; }
    public string url { get; set; }
    public DateTime createDate { get; set; }
    public DateTime updateDate { get; set; }
    public string userId { get; set; }
    public string servicePlanId { get; set; }
}