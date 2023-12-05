using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Fody;
using HealthMate.Models;
using HealthMate.Models.Tables;
using HealthMate.Services;
using HealthMate.Services.QuestionServices;
using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Questions;

public partial class QuestionsPageViewModel(NavigationService navigationService,
	QuestionService questionService,
	RealmService realmService) : BaseViewModel(navigationService)
{
	private bool _isGeneralQuestionnaires;

	[ObservableProperty]
	private UserTable passedUser;

	[ObservableProperty]
	private ObservableCollection<QuestionGroup> questions;

	[RelayCommand]
	[ConfigureAwait(false)]
	private async Task GetAnswersAndProceed()
	{
		var answersDictionary = new Dictionary<string, double>();
		foreach (var group in Questions)
			foreach (var question in group)
				if (question.SelectedChoice != null) // Check if the question has a selected choice
					answersDictionary[question.Name] = question.SelectedChoice.Value;
				else if (question.NumericAnswer != null) // Check if the question has a numeric answer
					answersDictionary[question.Name] = question.NumericAnswer.Value;

		if (_isGeneralQuestionnaires)
		{
			var user = new UserTable
			{
				Birthdate = DateTime.Now,
				Email = "",
				Gender = "",
				RealmUserId = ObjectId.GenerateNewId(),
				UserId = "",
				Username = ""
			};

			foreach (var item in answersDictionary)
				await UpsertQuestionnaire(true, item.Key, user, item.Value);

			await realmService.Upsert(user);
		}
		else
			foreach (var item in answersDictionary)
				await UpsertQuestionnaire(false, item.Key, PassedUser, item.Value);
	}

	public override void OnNavigatedTo()
	{
		var questions = _isGeneralQuestionnaires ? questionService.GeneralQuestions : questionService.CategoryAndQuestion;
		var groupedQuestions = questions.Select(_ => new QuestionGroup(_.Category, _.Questions));
		Questions = new ObservableCollection<QuestionGroup>(groupedQuestions);
	}

	protected override void ReceiveParameters(IDictionary<string, object> query)
	{
		_isGeneralQuestionnaires = (bool)query["isGeneralQuestionnaires"];
		if (query["passedUser"] is UserTable user)
			PassedUser = user;
	}

	private Task UpsertQuestionnaire(bool isGeneralQuestionnaire, string questionKey, UserTable user, double questionValue)
	{
		return realmService.Upsert(new Questionnaires
		{
			IsGeneralQuestionnaire = isGeneralQuestionnaire,
			Key = questionKey,
			QuestionnaireId = ObjectId.GenerateNewId(),
			User = user,
			Value = questionValue
		});
	}
}