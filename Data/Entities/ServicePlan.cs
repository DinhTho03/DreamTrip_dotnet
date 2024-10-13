using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities
{
    [BsonConllection("servicePlan")]
    public class ServicePlan
    {
        [BsonId]
        [BsonRepresentation(BsonType.ObjectId)]
        public string _Id { get; set; }
        public string title { get; set; }
        public string description { get; set; }
        public string url { get; set; }
        public DateTime? startDate { get; set; }
        public DateTime? endDate { get; set; }
        public string serviceId { get; set; }
    }
}
