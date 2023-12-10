using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Constants;
using HealthMate.Models.Tables;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using HealthMate.Views.Questions;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace HealthMate.ViewModels.Accounts;

public partial class AccountPageViewModel(NavigationService navigationService, HttpService httpService, UserService userService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private bool isLoading = false;

	[ObservableProperty]
	private bool isSignup = true;

	[ObservableProperty]
	private DateTime maxDate = DateTime.Now;

	[ObservableProperty]
	private DateTime minDate = new(1900, 1, 1);

	#region Login
	[ObservableProperty]
	[CustomValidation(typeof(AccountPageViewModel), nameof(ValidateLoginCredentials))]
	private string loginUsername;

	[ObservableProperty]
	[CustomValidation(typeof(AccountPageViewModel), nameof(ValidateLoginCredentials))]
	private string loginPassword;

	[ObservableProperty]
	private bool isHidingLoginPassword = true;

	[ObservableProperty]
	private string loginPasswordIcon = FontAwesomeIcons.EyeSlash;
	#endregion

	#region Signup
	[ObservableProperty]
	[Required]
	private string signUpUsername;

	[ObservableProperty]
	[Required]
	//[GeneratedRegex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase, 250)]
	[RegularExpression(@"^[^@\s]+@[^@\s]+\.[^@\s]+$")]
	private string signUpEmail;

	[ObservableProperty]
	[Required]
	private DateTime signUpBirthdate = DateTime.Now;

	[ObservableProperty]
	[Required]
	private string signUpSelectedGender = "Male";

	[ObservableProperty]
	private List<string> signUpGenders = ["Male", "Female"];

	[ObservableProperty]
	[MinLength(8)]
	[Required]
	private string signUpPassword;

	[ObservableProperty]
	[MinLength(8)]
	[Required]
	private string signUpConfirmPassword;

	[ObservableProperty]
	private bool isHidingSignupPassword = true;

	[ObservableProperty]
	private string signUpPasswordIcon = FontAwesomeIcons.EyeSlash;

	[ObservableProperty]
	private bool isHidingSignupConfirmPassword = true;

	[ObservableProperty]
	private string signUpConfirmPasswordIcon = FontAwesomeIcons.EyeSlash;
	#endregion

	[RelayCommand]
	private void ChangeAccountState(bool isSignup)
	{
		IsSignup = isSignup;
	}

	[RelayCommand]
	private void ToggleLoginPasswordVisibility()
	{
		IsHidingLoginPassword = !IsHidingLoginPassword;
		LoginPasswordIcon = IsHidingLoginPassword ? FontAwesomeIcons.EyeSlash : FontAwesomeIcons.Eye;
	}

	[RelayCommand]
	private void ToggleSignupPasswordVisibility()
	{
		IsHidingSignupPassword = !IsHidingSignupPassword;
		SignUpPasswordIcon = IsHidingSignupPassword ? FontAwesomeIcons.EyeSlash : FontAwesomeIcons.Eye;
	}

	[RelayCommand]
	private void ToggleSignupConfirmPasswordVisibility()
	{
		IsHidingSignupConfirmPassword = !IsHidingSignupConfirmPassword;
		SignUpConfirmPasswordIcon = IsHidingSignupConfirmPassword ? FontAwesomeIcons.EyeSlash : FontAwesomeIcons.Eye;
	}

	[RelayCommand]
	private async Task Signup()
	{
		IsLoading = true;

		var localId = ObjectId.GenerateNewId();
		var existingUser = await userService.GetLoggedUser();
		if (existingUser != null)
		{
			// prevent creating multiple entry of User in Realm
			localId = existingUser.LocalUserId;
		}

		// TODO: Uncomment this on production code
		//var signupstatus = await userservice.signup(new user
		//{
		//	birthdate = signupbirthdate,
		//	email = signupemail,
		//	gender = signupselectedgender,
		//	password = signuppassword,
		//	username = signupusername,
		//	localuserid = localid
		//});
		var faker = new Faker<User>()
			.RuleFor(p => p.Birthdate, v => v.Date.Past())
			.RuleFor(p => p.Email, v => v.Internet.Email())
			.RuleFor(p => p.Gender, v => v.PickRandom("Male", "Female"))
			.RuleFor(p => p.Password, v => v.Internet.Password())
			.RuleFor(p => p.Username, v => v.Internet.UserName())
			.RuleFor(p => p.LocalUserId, v => localId)
			.Generate(1)[0];
		var signupStatus = await userService.Signup(faker);
		IsLoading = false;

		if (signupStatus != "Success")
		{
			await Application.Current.MainPage.DisplayAlert("Couldn't sign up", $"An internal error has occured: {signupStatus}", "OK");
			return;
		}

		var successAlertAccepted = await Application.Current.MainPage.DisplayAlert("Success", "You have successfuly created an account! You may answer the following questions now or later.", "OK", "Later");
		if (successAlertAccepted)
			await NavigationService.PushAsync(nameof(QuestionPage), new Dictionary<string, object>
			{
				{ "isGeneralQuestionnaires", true }
			});
		else
			NavigationService.ChangeShellItem(3);
	}

	[RelayCommand]
	private Task Login()
	{
		var isValidLogin = !string.IsNullOrWhiteSpace(LoginUsername) && !string.IsNullOrWhiteSpace(LoginPassword);

		return isValidLogin
			? userService.Login(LoginUsername, LoginPassword)
			: Application.Current.MainPage.DisplayAlert("Couldn't log in", "Please fill all the necessary fields in order to proceed.", "OK");
	}

	public static ValidationResult ValidateLoginCredentials(string entity, ValidationContext context)
	{
		var instance = (AccountPageViewModel)context.ObjectInstance;
		var isSignup = instance.IsSignup;

		return isSignup ? ValidationResult.Success : !string.IsNullOrWhiteSpace(entity) ? ValidationResult.Success : new("Validation error");
	}
}