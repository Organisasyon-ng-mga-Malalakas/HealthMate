using Newtonsoft.Json;

namespace HealthMate.Models;

public class RootDiagnosis
{
    [JsonProperty("diagnosis")]
    public IEnumerable<Diagnosis> Diagnosis { get; set; }

    [JsonProperty("similar_symptoms")]
    public IEnumerable<Diagnosis> SimilarIllness { get; set; }
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

public class DiagnosisGroup : List<Diagnosis>
{
    public string Name { get; private set; }

    public DiagnosisGroup(string name, List<Diagnosis> diagnoses) : base(diagnoses)
    {
        Name = name;
    }
}

//public class SimilarSymptom
//{
//    [JsonProperty("id")]
//    public int Id { get; set; }

//    [JsonProperty("name")]
//    public string Name { get; set; }
//}