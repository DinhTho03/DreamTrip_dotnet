using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using brandportal_dotnet.Data.Utils;

namespace TravelItineraryProject.Data.Entities;
[BsonConllection("dayItinerary")]
public class DayItinerary
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public int dayNumber { get; set; }
    public string activities { get; set; }
    public string userId { get; set; }
}