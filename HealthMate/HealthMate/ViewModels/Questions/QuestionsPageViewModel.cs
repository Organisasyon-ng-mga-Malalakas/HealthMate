using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Services.QuestionServices;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Questions;

public partial class QuestionsPageViewModel(NavigationService navigationService, QuestionService questionService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private ObservableCollection<QuestionGroup> questions;

	public override void OnNavigatedTo()
	{
		var generalQuestions = questionService.CategoryAndQuestion
			.Where(_ => _.Category is
			"Constitutional & General review of system" or
			"Constitutional and vital signs physical examination" or
			"Family history" or
			"Past medical history");

		var pickerQuestions = generalQuestions.Select(x => new QuestionGroup(x.Category, x.Questions));
		Questions = new ObservableCollection<QuestionGroup>(pickerQuestions);
	}

	[RelayCommand]
	private void GetAnswers()
	{
		var answersDictionary = new Dictionary<string, double>();
		foreach (var group in Questions)
		{
			foreach (var question in group)
			{
				// Check if the question has a selected choice
				if (question.SelectedChoice != null)
					answersDictionary[question.Name] = question.SelectedChoice.Value;
				// Check if the question has a numeric answer
				else if (question.NumericAnswer != null)
					answersDictionary[question.Name] = question.NumericAnswer.Value;
			}
		}

		var asa = 1;
	}
}