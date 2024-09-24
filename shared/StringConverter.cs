using System;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace brandportal_dotnet.shared;

public class StringConverter : JsonConverter<string>
{
    public override string? Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options) =>
        reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32().ToString(),
            JsonTokenType.String => reader.GetString(),
            JsonTokenType.False => reader.GetBoolean().ToString(),
            JsonTokenType.True => reader.GetBoolean().ToString(),
            _ => throw new JsonException($"Unexpected token type: {reader.TokenType}")
        };


    public override void Write(Utf8JsonWriter writer, string value, JsonSerializerOptions options)
    {
        writer.WriteStringValue(value);
    }
}
