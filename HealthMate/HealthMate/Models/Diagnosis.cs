using Newtonsoft.Json;

namespace HealthMate.Models;

public class RootDiagnosis
{
    [JsonProperty("diagnosis")]
    public IEnumerable<Diagnosis> Diagnosis { get; set; }

    [JsonProperty("similar_symptoms")]
    public IEnumerable<SimilarSymptom> SimilarSymptoms { get; set; }
}

public class Diagnosis
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("accuracy")]
    public double Accuracy { get; set; }
}

public class SimilarSymptom
{
    [JsonProperty("id")]
    public int Id { get; set; }

    [JsonProperty("name")]
    public string Name { get; set; }
}