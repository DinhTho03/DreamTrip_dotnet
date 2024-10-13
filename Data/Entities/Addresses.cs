using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("addresses")]
    public class Addresses
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string country { get; set; }
        public string province { get; set; }
        public string district { get; set; }
        public string destailStreet { get; set; }
        public string servicePlanId { get; set; }
    }
}
