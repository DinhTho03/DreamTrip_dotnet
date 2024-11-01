
using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities;
[BsonConllection("endpointPage")]
public class EndpointPage
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string Name { get; set; }
    public string? Description { get; set; }
    public string? EndPointId { get; set; }
    public string? Type { get; set; }
    public DateTime? CreateAt { get; set; }
}