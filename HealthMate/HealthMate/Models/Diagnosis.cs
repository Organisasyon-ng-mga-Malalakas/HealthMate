using CommunityToolkit.Mvvm.ComponentModel;
using System.Text.Json.Serialization;

namespace HealthMate.Models;

public partial class Diagnosis : ObservableObject
{
	[JsonIgnore]
	public bool IsSelected { get; set; }

	[JsonPropertyName("Issue")]
	public SymptomInfo Issue { get; set; }
}

public partial class SymptomInfo : ObservableObject
{
	[JsonIgnore]
	public bool IsSelected { get; set; }
	[JsonPropertyName("ID")]
	public int Id { get; set; }

	[JsonPropertyName("Name")]
	public string Name { get; set; }

	[JsonPropertyName("Accuracy")]
	public double Accuracy { get; set; }

	[JsonPropertyName("Icd")]
	public string Icd { get; set; }

	[JsonPropertyName("IcdName")]
	public string IcdName { get; set; }

	[JsonPropertyName("ProfName")]
	public string ProfName { get; set; }

	[JsonPropertyName("Ranking")]
	public long Ranking { get; set; }
}

//public class DiagnosisGroup(string name, List<Diagnosis> diagnoses) : List<Diagnosis>(diagnoses)
//{
//	public string Name { get; private set; } = name;
//}