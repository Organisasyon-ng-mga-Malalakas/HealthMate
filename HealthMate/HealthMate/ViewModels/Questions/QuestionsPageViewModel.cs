using CommunityToolkit.Mvvm.ComponentModel;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Services.QuestionServices;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Questions;

public partial class QuestionsPageViewModel(NavigationService navigationService, QuestionService questionService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private ObservableCollection<TestGroup> questions;

	public override void OnNavigatedTo()
	{
		var generalQuestions = questionService.CategoryAndQuestion
			.Where(_ => _.Category is
			"Constitutional & General review of system" or
			"Constitutional and vital signs physical examination" or
			"Family history" or
			"Past medical history");
		var test = generalQuestions.Select(x => new TestGroup(x.Category, x.Questions.ToList()));
		Questions = new ObservableCollection<TestGroup>(test);

	}
}
/*
 Constitutional & General review of system
Constitutional and vital signs physical examination
Family history
Past medical history
 */