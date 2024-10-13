using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("images")]
    public class Images
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; } 
        public string url { get; set; }
        public string servicePlanId { get; set; }
    }
}
