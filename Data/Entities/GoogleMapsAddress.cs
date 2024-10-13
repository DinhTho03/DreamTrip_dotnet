using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("googleMapsAddress")]
    public class GoogleMapsAddress
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string location { get; set; }
        public string fotmattedAddress { get; set; }
        public string addressId { get; set; }

    }
}
