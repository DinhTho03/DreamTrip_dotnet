using System.Text.Json.Serialization;

namespace brandportal_dotnet.Models;

public record ExtraConfig
{
    [JsonPropertyName("posm_sub_type")]
    public PosmSubType[]? PosmSubType { get; set; }
}

public record PosmSubType
{
    [JsonPropertyName("id")]
    public long? Id { get; set; }

    [JsonPropertyName("position")]
    public long? Position { get; set; }

    [JsonPropertyName("list_posm_sub_type")]
    public string[]? ListPosmSubType { get; set; }

    [JsonPropertyName("num_photos")]
    public long? NumPhotos { get; set; }

    [JsonPropertyName("posm_num_door")]
    public long[]? PosmNumDoor { get; set; }
}