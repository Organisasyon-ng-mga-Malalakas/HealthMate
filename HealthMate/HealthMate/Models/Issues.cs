using System.Text.Json.Serialization;

namespace HealthMate.Models;
public class Issues
{
	[JsonPropertyName("ID")]
	public int Id { get; set; }

	[JsonPropertyName("Name")]
	public string Name { get; set; }
}