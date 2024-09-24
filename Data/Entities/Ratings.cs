using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TravelItineraryProject.Data.Entities;

[BsonConllection("ratings")]
public class Ratings
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string userId { get; set; }
    public string servicePlanId { get; set; }
    public double value { get; set; }
}