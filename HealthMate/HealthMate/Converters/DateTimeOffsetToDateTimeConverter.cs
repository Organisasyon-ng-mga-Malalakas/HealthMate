using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthMate.Converters;
public class DateTimeOffsetToDateTimeConverter : JsonConverter<DateTime>
{
	public override DateTime Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
			throw new JsonException("Expected a string token to convert to DateTime");

		var dateTimeOffset = DateTimeOffset.Parse(reader.GetString());
		return dateTimeOffset.DateTime; // Convert to DateTime
	}

	public override void Write(Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
	{
		// Write DateTime as DateTimeOffset string
		writer.WriteStringValue(new DateTimeOffset(value).ToString("o"));
	}
}
