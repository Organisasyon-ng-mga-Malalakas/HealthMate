using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models;
using HealthMate.Models.Tables;
using HealthMate.Services;
using HealthMate.Services.QuestionServices;
using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Questions;

public partial class QuestionsPageViewModel(NavigationService navigationService,
	QuestionService questionService,
	RealmService realmService,
	UserService userService) : BaseViewModel(navigationService)
{
	private bool _isGeneralQuestionnaires;

	[ObservableProperty]
	private ObservableCollection<QuestionGroup> questions;

	[RelayCommand]
	private async Task GetAnswersAndProceed()
	{
		var answersDictionary = new Dictionary<string, double>();
		foreach (var question in Questions.SelectMany(group => group))
			if (question.SelectedChoice != null) // Check if the question has a selected choice
				answersDictionary[question.Name] = question.SelectedChoice.Value;
			else if (question.NumericAnswer != null) // Check if the question has a numeric answer
				answersDictionary[question.Name] = question.NumericAnswer.Value;

		var user = await userService.GetLoggedUser();
		foreach (var item in answersDictionary)
			await realmService.Upsert(new Questionnaires
			{
				IsGeneralQuestionnaire = _isGeneralQuestionnaires,
				Key = item.Key,
				QuestionnaireId = ObjectId.GenerateNewId(),
				User = user,
				Value = item.Value
			});

		if (_isGeneralQuestionnaires)
			NavigationService.ChangeShellItem(3);
	}

	public override async void OnNavigatedTo()
	{
		var questions = _isGeneralQuestionnaires ? questionService.GeneralQuestions : questionService.CategoryAndQuestion;
		var groupedQuestions = questions.Select(_ => new QuestionGroup(_.Category, _.Questions));
		Questions = new ObservableCollection<QuestionGroup>(groupedQuestions);
		var passedUser = await userService.GetLoggedUser();

		if (passedUser is User user && user.Questionnaires is List<ActualQuestion> questionnaire && questionnaire.Count != 0)
		{
			// Get questions with answer already
			var questionsWithAnswer = questionnaire.Where(_ => _.NumericAnswer != null || _.SelectedChoice != null);

			// Pre-fill the answers
			foreach (var item in questionnaire)
			{
				var correctGroup = Questions.First(_ => _.Name == item.Category);

				if (item.NumericAnswer is double numericAnswer)
					correctGroup.Questions.First(_ => _.Name == item.Name).NumericAnswer = numericAnswer;
				else if (item.SelectedChoice is Choice choice)
					correctGroup.Questions.First(_ => _.Name == item.Name).SelectedChoice = choice;
			}
		}
	}

	protected override void ReceiveParameters(IDictionary<string, object> query)
	{
		_isGeneralQuestionnaires = (bool)query["isGeneralQuestionnaires"];
	}
}