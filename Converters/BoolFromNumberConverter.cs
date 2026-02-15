using System.Text.Json;
using System.Text.Json.Serialization;

namespace CsGoEmpire.Api.Converters;

/// <summary>
/// Converts a JSON number (0 or 1) to a <see cref="bool"/> and vice-versa.
/// The CSGOEmpire API returns some boolean fields as numeric values.
/// </summary>
public sealed class BoolFromNumberConverter : JsonConverter<bool>
{
    /// <inheritdoc />
    public override bool Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
    {
        return reader.TokenType switch
        {
            JsonTokenType.Number => reader.GetInt32() != 0,
            JsonTokenType.True => true,
            JsonTokenType.False => false,
            _ => throw new JsonException($"Unexpected token type {reader.TokenType} when parsing boolean.")
        };
    }

    /// <inheritdoc />
    public override void Write(Utf8JsonWriter writer, bool value, JsonSerializerOptions options)
    {
        writer.WriteBooleanValue(value);
    }
}
