using System.Text.Json.Serialization;

namespace HealthMate.Models;
public class IssueInfo
{
	[JsonPropertyName("Description")]
	public string Description { get; set; }

	[JsonPropertyName("DescriptionShort")]
	public string DescriptionShort { get; set; }

	[JsonPropertyName("MedicalCondition")]
	public string MedicalCondition { get; set; }

	[JsonPropertyName("Name")]
	public string Name { get; set; }

	[JsonPropertyName("PossibleSymptoms")]
	public string PossibleSymptoms { get; set; }

	[JsonPropertyName("ProfName")]
	public string ProfName { get; set; }

	[JsonPropertyName("Synonyms")]
	public object Synonyms { get; set; }

	[JsonPropertyName("TreatmentDescription")]
	public string TreatmentDescription { get; set; }
}