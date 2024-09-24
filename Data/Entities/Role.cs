using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using brandportal_dotnet.Data.Utils;

namespace TravelItineraryProject.Data.Entities;

[BsonConllection("role")]
public class Role
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string name { get; set; }
}