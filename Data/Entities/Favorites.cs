using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities;
[BsonConllection("favorites")]
public class Favorites
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string userId { get; set; }
    public string itemId { get; set; }
    public string itemType { get; set; }
}