using HealthMate.Models;

namespace HealthMate.Templates;

public class QuestionDataTemplateSelector : DataTemplateSelector
{
	public DataTemplate MultipleChoiceDataTemplate { get; set; }
	public DataTemplate NumericAnswerDataTemplate { get; set; }
	protected override DataTemplate OnSelectTemplate(object item, BindableObject container)
	{
		var model = (ActualQuestion)item;
		return model.Choices == null || model.Choices.Count == 0 ? NumericAnswerDataTemplate : MultipleChoiceDataTemplate;
	}
}