using System.Text;
using System.Text.Json;

namespace HealthMate.Extensions;

public static class JsonExtension
{
	public static StringContent AsJSONSerializedObject<T>(this T objectToSerialize)
	{
		var json = JsonSerializer.Serialize(objectToSerialize);
		var encodedJSON = new StringContent(json, Encoding.UTF8, "application/json");

		return encodedJSON;
	}
}