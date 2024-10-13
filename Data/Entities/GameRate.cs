using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("gameRate")]
public class GameRate
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string? GameId { get; set; }

    /// <summary>
    /// lần quay 1,2,3, -1 là các lần còn lại
    /// </summary>
    public int? Times { get; set; }

    public sbyte? PercentWin { get; set; }
}