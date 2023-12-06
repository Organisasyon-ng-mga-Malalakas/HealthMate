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
	RealmService realmService) : BaseViewModel(navigationService)
{
	private bool _isGeneralQuestionnaires;

	[ObservableProperty]
	private DateTime birthDate;

	[ObservableProperty]
	private string email;

	[ObservableProperty]
	private string gender;

	[ObservableProperty]
	private UserTable passedUser;

	[ObservableProperty]
	private string userName;

	[ObservableProperty]
	private ObservableCollection<QuestionGroup> questions;

	private void AppendQuestionnaire(bool isGeneralQuestionnaire, string questionKey, UserTable user, double questionValue)
	{
		user.Questionnaires.Add(new Questionnaires
		{
			IsGeneralQuestionnaire = isGeneralQuestionnaire,
			Key = questionKey,
			QuestionnaireId = ObjectId.GenerateNewId(),
			User = user,
			Value = questionValue
		});
	}

	[RelayCommand]
	private async Task GetAnswersAndProceed()
	{
		var answersDictionary = new Dictionary<string, double>();
		foreach (var question in Questions.SelectMany(group => group))
			if (question.SelectedChoice != null) // Check if the question has a selected choice
				answersDictionary[question.Name] = question.SelectedChoice.Value;
			else if (question.NumericAnswer != null) // Check if the question has a numeric answer
				answersDictionary[question.Name] = question.NumericAnswer.Value;

		var user = _isGeneralQuestionnaires
			? new UserTable
			{
				Birthdate = BirthDate,
				Email = Email,
				Gender = Gender,
				RealmUserId = ObjectId.GenerateNewId(),
				Username = UserName
			}
			: PassedUser;

		foreach (var item in answersDictionary)
			AppendQuestionnaire(_isGeneralQuestionnaires, item.Key, user, item.Value);

		await realmService.Upsert(user);

		if (_isGeneralQuestionnaires)
			NavigationService.ChangeShellItem(3);
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
		if (query.TryGetValue("passedUser", out var user) && user is UserTable parsedUser)
			PassedUser = parsedUser;

		if (query.TryGetValue("birthDate", out var birthDate) && birthDate is DateTime parsedBirthDate)
			BirthDate = parsedBirthDate;

		if (query.TryGetValue("email", out var email) && email is string parsedEmail)
			Email = parsedEmail;

		if (query.TryGetValue("gender", out var gender) && gender is string parsedGender)
			Gender = parsedGender;

		if (query.TryGetValue("username", out var username) && username is string parsedUsername)
			UserName = parsedUsername;
	}
}