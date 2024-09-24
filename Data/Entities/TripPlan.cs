using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TravelItineraryProject.Data.Entities;
[BsonConllection("tripPlan")]
public class TripPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string title { get; set; }
    public string description { get; set; }
    public DateTime startDate { get; set; } 
    public DateTime endDate { get; set; } 
    public int dayNumber { get; set; }
    public string dayItineraryId { get; set; }
}