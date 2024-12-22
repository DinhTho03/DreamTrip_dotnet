using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using brandportal_dotnet.Data.Utils;

namespace brandportal_dotnet.Data.Entities;

[BsonConllection("groupTripPlan")]
public class GroupTripPlan
{
    [BsonId]
    [BsonRepresentation(BsonType.ObjectId)]
    public string _Id { get; set; }

    public string Name { get; set; }
    public bool? IsExpired { get; set; }
    public DateTime? StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public DateTime? CreatedAt { get; set; }
    public bool? IsPublic { get; set; }
    public string UserId { get; set; }
    public string Departure { get; set; }
    public string Destination { get; set; }
    public double? PriceTotal { get; set; }
    public int? View { get; set; }
    public DateTime? StartDateShare { get; set; }
    public DateTime? EndDateShare { get; set; }
    public string? GroupTripPlanId { get; set; }
    public string? UserExperienceId { get; set; }
}