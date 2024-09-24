using brandportal_dotnet.shared;
using System.Text.Json.Serialization;

namespace brandportal_dotnet.Models;

public record ColumnFilter
{
    [JsonPropertyName("k")] public string Key { get; set; }

    [JsonPropertyName("c")] public string Comparison { get; set; }
    [JsonConverter(typeof(StringConverter))]

    [JsonPropertyName("v")] public string Value { get; set; }
}
