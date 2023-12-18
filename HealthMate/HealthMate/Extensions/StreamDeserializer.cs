using Newtonsoft.Json;
using SystemTextJsonSerializer = System.Text.Json.JsonSerializer;

namespace HealthMate.Extensions;
public static class StreamDeserializer
{
	public static T NewtonsoftDeserializeStream<T>(this Stream stream)
	{
		using var streamReader = new StreamReader(stream);
		using var jsonTextReader = new JsonTextReader(streamReader);
		var serializer = new JsonSerializer();
		return serializer.Deserialize<T>(jsonTextReader);
	}

	public static T DeserializeStream<T>(this Stream stream)
	{
		var result = SystemTextJsonSerializer.Deserialize<T>(stream);
		return result;
	}

	public static ValueTask<T> DeserializeStreamAsync<T>(this Stream stream)
	{
		var result = SystemTextJsonSerializer.DeserializeAsync<T>(stream);
		return result;
	}
}