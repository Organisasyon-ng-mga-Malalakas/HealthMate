using Newtonsoft.Json;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

namespace HealthMate.Extensions;
public static class StreamDeserializer
{
	public static T DeserializeStream<T>(this Stream stream)
	{
		using var streamReader = new StreamReader(stream);
		using var jsonTextReader = new JsonTextReader(streamReader);
		var serializer = new JsonSerializer();
		return serializer.Deserialize<T>(jsonTextReader);
	}

	public static ValueTask<T> SystemTextJsonDeserializeStream<T>(this Stream stream)
	{
		var result = SystemTextJsonSerializer.DeserializeAsync<T>(stream);
		return result;
	}
}