using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Constants;
using HealthMate.Services;
using HealthMate.Services.HttpServices;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.ViewModels.Accounts;

public partial class AccountPageViewModel(NavigationService navigationService, HttpService httpService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private bool isSignup;

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
		var isValidSignup = !string.IsNullOrWhiteSpace(SignUpUsername)
			&& !string.IsNullOrWhiteSpace(SignUpEmail)
			&& !string.IsNullOrWhiteSpace(SignUpPassword) && SignUpPassword.Length > 7
			&& SignUpPassword == SignUpConfirmPassword;

		if (isValidSignup)
		{
			var test = await httpService.Signup(SignUpEmail, SignUpUsername, SignUpPassword);
		}
		else
		{
			await Application.Current.MainPage.DisplayAlert("Couldn't sign up", "Please fill all the necessary fields in order to proceed.", "OK");
			;
		}

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
}