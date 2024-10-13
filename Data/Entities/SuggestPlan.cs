using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("suggestPlan")]
    public class SuggestPlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }

        public string? Type { get; set; }
        public string? Name { get; set; }
        public sbyte? Order { get; set; }
        public DateTime? UpdatedAt { get; set; }
        public int? UpdatedBy { get; set; }
    }
}
