using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities;
using brandportal_dotnet.Data.Utils;
public class DetailTripPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public string location { get; set; }
    public string nameService { get; set; }
    public string description { get; set; }
    public string price { get; set; }
    public DateTime startDate { get; set; }
    public DateTime endDate { get; set; }
    public string tripPlanId { get; set; }
}