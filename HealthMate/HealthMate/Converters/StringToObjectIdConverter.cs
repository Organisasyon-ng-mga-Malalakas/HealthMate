using MongoDB.Bson;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace HealthMate.Converters;
public class StringToObjectIdConverter : JsonConverter<ObjectId>
{
	public override ObjectId Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
	{
		if (reader.TokenType != JsonTokenType.String)
			throw new JsonException("Expected a string value to convert to ObjectId.");

		var stringValue = reader.GetString();
		// Assuming stringValue is in a format that can be converted to ObjectId
		return ObjectId.Parse(stringValue);
	}

	public override void Write(Utf8JsonWriter writer, ObjectId objectId, JsonSerializerOptions options)
	{
		// Write the ObjectId as a string
		writer.WriteStringValue(objectId.ToString());
	}
}
