using HealthMate.Extensions;
using HealthMate.Models;
using System.Reflection;

namespace HealthMate.Services.QuestionServices;

public class QuestionService
{
	public IEnumerable<CategoryAndQuestion> CategoryAndQuestion { get; }
	public IEnumerable<CategoryAndQuestion> GeneralQuestions { get; }
	public Dictionary<string,ActualQuestion> QuestionsLookup { get; }

	public QuestionService()
	{
		var stream = Assembly.GetExecutingAssembly().GetManifestResourceStream("HealthMate.Services.QuestionServices.Questions.json");
		CategoryAndQuestion = stream.DeserializeStream<IEnumerable<CategoryAndQuestion>>();
		GeneralQuestions = CategoryAndQuestion
			.Where(_ => _.Category is "Constitutional & General review of system" or
			"Constitutional and vital signs physical examination" or
			"Family history" or
			"Past medical history");

		QuestionsLookup = new Dictionary<string, ActualQuestion>();

		foreach (var category in CategoryAndQuestion)
		{
			foreach (var question in category.Questions)
			{
				QuestionsLookup[question.Name] = question;
			}
		}
	}
}
