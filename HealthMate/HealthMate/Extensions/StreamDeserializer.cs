using Newtonsoft.Json;

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
}