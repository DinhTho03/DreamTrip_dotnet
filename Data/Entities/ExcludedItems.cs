using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace TravelItineraryProject.Data.Entities
{
    [BsonConllection("excludedItems")]
    public class ExcludedItems 
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string title { get; set; }
        public string servicePlanId { get; set; }
    }
}
