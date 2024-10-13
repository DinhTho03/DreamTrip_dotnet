using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("page")]
    public class Page
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        public string? Type { get; set; }
        public string? PageName { get; set; }
        public sbyte? PageOrder { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}