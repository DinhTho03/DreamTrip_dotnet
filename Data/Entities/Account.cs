using brandportal_dotnet.Data.Utils;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Bson;

namespace TravelItineraryProject.Data.Entities
{
    [BsonConllection("account")]
    public class Account
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string email { get; set; }
        public string phone { get; set; }
        public string password { get; set; }
        public string passwordRT { get; set; }
        public string roleId { get; set; }
        public DateTime registerDate { get; set; }
    }
}
