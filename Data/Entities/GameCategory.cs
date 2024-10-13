using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("gameCategory")]
    public class GameCategory
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string CateType { get; set; } = null!;

        public string? CateName { get; set; }

    }
}
