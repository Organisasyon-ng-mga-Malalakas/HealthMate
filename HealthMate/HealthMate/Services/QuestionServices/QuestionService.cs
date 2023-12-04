using HealthMate.Extensions;
using HealthMate.Models;
using System.Reflection;

namespace HealthMate.Services.QuestionServices;

public class QuestionService
{
	public IEnumerable<CategoryAndQuestion> CategoryAndQuestion { get; }

	public QuestionService()
	{
		var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HealthMate.Services.QuestionServices.Questions.json");
		CategoryAndQuestion = stream.DeserializeStream<IEnumerable<CategoryAndQuestion>>();
	}
}
