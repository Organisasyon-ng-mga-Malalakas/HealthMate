using Newtonsoft.Json;

namespace HealthMate.Models;
public class Symptoms
{
    [JsonProperty("id")]
    public long Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("is_critical")]
    public bool IsCritical { get; set; }
}