using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("categories")]
    public class Categories
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string name { get; set; }

    }
}
