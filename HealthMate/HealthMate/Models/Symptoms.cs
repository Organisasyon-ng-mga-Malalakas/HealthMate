using System.Text.Json.Serialization;

namespace HealthMate.Models;
public class Symptoms
{
    [JsonPropertyName("id")]
    public long Id { get; set; }

    [JsonPropertyName("name")]
    public string Name { get; set; }

    [JsonPropertyName("is_critical")]
    public bool IsCritical { get; set; }
}