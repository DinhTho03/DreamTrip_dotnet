using System.Text.Json.Serialization;

namespace brandportal_dotnet.Models;

public record ColumnOrder
{
    [JsonPropertyName("k")] public string Key { get; set; }

    [JsonPropertyName("d")] public string Direction { get; set; }
}