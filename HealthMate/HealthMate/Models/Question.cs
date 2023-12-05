using CommunityToolkit.Mvvm.ComponentModel;
using Newtonsoft.Json;

namespace HealthMate.Models;

public class QuestionGroup(string name, IEnumerable<ActualQuestion> questions) : List<ActualQuestion>(questions)
{
	public string Name { get; set; } = name;
	public IEnumerable<ActualQuestion> Questions { get; set; } = questions;
}

public class CategoryAndQuestion
{
	[JsonProperty("category")]
	public string Category { get; set; }
	[JsonProperty("questions")]
	public IEnumerable<ActualQuestion> Questions { get; set; }
}

public class ActualQuestion : ObservableObject
{
	[JsonProperty("text")]
	public string Text { get; set; }

	[JsonProperty("laytext")]
	public string Laytext { get; set; }

	[JsonProperty("name")]
	public string Name { get; set; }

	[JsonProperty("type")]
	public long Type { get; set; }

	[JsonProperty("min", NullValueHandling = NullValueHandling.Ignore)]
	public double? Min { get; set; }

	[JsonProperty("max", NullValueHandling = NullValueHandling.Ignore)]
	public double? Max { get; set; }

	[JsonProperty("default")]
	public double Default { get; set; }

	[JsonProperty("category")]
	public string Category { get; set; }

	[JsonProperty("choices", NullValueHandling = NullValueHandling.Ignore)]
	public List<Choice> Choices { get; set; }

	//#nullable enable
	[JsonIgnore]
	public Choice? SelectedChoice { get; set; }
	[JsonIgnore]
	public double? NumericAnswer { get; set; }
	//#nullable restore
}

public class Choice
{
	[JsonProperty("text")]
	public string Text { get; set; }

	[JsonProperty("laytext")]
	public string Laytext { get; set; }

	[JsonProperty("value")]
	public long Value { get; set; }

	[JsonProperty("relatedanswertag")]
	public string Relatedanswertag { get; set; }
}