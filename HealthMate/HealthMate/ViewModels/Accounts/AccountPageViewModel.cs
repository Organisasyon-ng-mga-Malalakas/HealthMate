using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Constants;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using HealthMate.Views.Questions;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.ViewModels.Accounts;

public partial class AccountPageViewModel(NavigationService navigationService, HttpService httpService, UserService userService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private bool isSignup = true;

	#region Login
	[ObservableProperty]
	[EmailAddress]
	private string loginUsername;

	[ObservableProperty]
	private string loginPassword;

	[ObservableProperty]
	private bool isHidingLoginPassword = true;

	[ObservableProperty]
	private string loginPasswordIcon = FontAwesomeIcons.EyeSlash;
	#endregion

	#region Signup
	[ObservableProperty]
	private string signUpUsername;

	[ObservableProperty]
	[EmailAddress]
	private string signUpEmail;

	[ObservableProperty]
	private DateTime signUpBirthdate = DateTime.Now;

	[ObservableProperty]
	private string signUpSelectedGender = "Male";

	[ObservableProperty]
	private List<string> signUpGenders = ["Male", "Female"];

	[ObservableProperty]
	[MinLength(8)]
	private string signUpPassword;

	[ObservableProperty]
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
		await NavigationService.PushAsync(nameof(QuestionPage), new Dictionary<string, object>
		{
			{ "isGeneralQuestionnaires", true },
			{ "birthDate", SignUpBirthdate },
			{ "email", SignUpEmail },
			{ "gender", SignUpSelectedGender },
			{ "username", SignUpUsername }
		});

		/*
		 Birthdate = DateTime.Now,
		 Email = "",
		 Gender = "",
		 RealmUserId = ObjectId.GenerateNewId(),
		 UserId = "",
		 Username = ""
		 */

		//var isValidSignup = !string.IsNullOrWhiteSpace(SignUpUsername)
		//	&& !string.IsNullOrWhiteSpace(SignUpEmail)
		//	&& IsBirthdateValid(SignUpBirthdate)
		//	&& !string.IsNullOrWhiteSpace(SignUpPassword) && SignUpPassword.Length > 5
		//	&& SignUpPassword == SignUpConfirmPassword;

		//if (!isValidSignup)
		//{
		//	await Application.Current.MainPage.DisplayAlert("Couldn't sign up", "Please fill all the necessary fields in order to proceed.", "OK");
		//	return;
		//}

		//var userDetails = new UserCreate(SignUpUsername, SignUpEmail, SignUpPassword, SignUpBirthdate, SignUpSelectedGender);
		//var result = await userService.Signup(userDetails);

		//if (result != "success")
		//{
		//	await Application.Current.MainPage.DisplayAlert("Couldn't sign up", result, "OK");
		//	return;
		//}

		//await Application.Current.MainPage.DisplayAlert("Success", "You have successfuly created a account!\nYou may login now to proceed.", "OK");
		//return;

		//return isValidSignup
		//	? httpService.Signup(SignUpEmail, SignUpUsername, SignUpPassword)
		//	: Application.Current.MainPage.DisplayAlert("Couldn't sign up", "Please fill all the necessary fields in order to proceed.", "OK");
	}

	[RelayCommand]
	private Task Login()
	{
		var isValidLogin = !string.IsNullOrWhiteSpace(LoginUsername) && !string.IsNullOrWhiteSpace(LoginPassword);

		return isValidLogin
			? httpService.Login(LoginUsername, LoginPassword)
			: Application.Current.MainPage.DisplayAlert("Couldn't log in", "Please fill all the necessary fields in order to proceed.", "OK");
	}
	private bool IsBirthdateValid(DateTime birthdate)
	{
		if (birthdate > DateTime.Now || birthdate.Year < 1900)
		{
			return false;
		}

		var age = DateTime.Now.Year - birthdate.Year;
		if (birthdate > DateTime.Now.AddYears(-age))
		{
			age--;
		}

		if (age > 150)
		{
			return false;
		}

		// Check for specific invalid dates (e.g., February 31st)
		try
		{
			var dt = new DateTime(birthdate.Year, birthdate.Month, birthdate.Day);
		}
		catch (ArgumentOutOfRangeException)
		{
			return false;
		}

		// If all checks pass, consider it a valid birthdate
		return true;
	}
}