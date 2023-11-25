using Newtonsoft.Json;

namespace HealthMate.Models;
public partial class DiagnosisInfo
{
    [JsonProperty("name")]
    public string Name { get; set; }

    [JsonProperty("description")]
    public string Description { get; set; }

    [JsonProperty("medical_term")]
    public string MedicalTerm { get; set; }

    [JsonProperty("treatment")]
    public string Treatment { get; set; }
}