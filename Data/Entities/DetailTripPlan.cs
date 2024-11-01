using brandportal_dotnet.Data.Utils;
using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
namespace brandportal_dotnet.Data.Entities;
using brandportal_dotnet.Data.Utils;
[BsonConllection("detailTripPlan")]
public class DetailTripPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }
    public double? Lat { get; set; }
    public double? Lng { get; set; }
    public string? Name { get; set; }
    public string? Photos { get; set; }
    public double? Rating { get; set; }
    public string? Distance { get; set; }
    public int? UserRatingsTotal { get; set; }
    public bool? HasExperienced { get; set; }
    public int NumberDay { get; set; }
    public string GroupTripPlanId { get; set; }
    public string? PlaceId { get; set; }
    public string? TimeActive { get; set; }
}