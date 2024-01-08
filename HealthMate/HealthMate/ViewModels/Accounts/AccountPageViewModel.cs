using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Constants;
using HealthMate.Models.Tables;
using HealthMate.Services;
using MongoDB.Bson;
using System.ComponentModel.DataAnnotations;
using ValidationResult = System.ComponentModel.DataAnnotations.ValidationResult;

namespace HealthMate.ViewModels.Accounts;

public partial class AccountPageViewModel(InventoryService inventoryService,
	NavigationService navigationService,
	IPreferences preferences,
	ScheduleService scheduleService,
	UserService userService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private bool isLoading;

	[ObservableProperty]
	private bool isSignup = true;

	[ObservableProperty]
	private DateTime maxDate = DateTime.Now;

	[ObservableProperty]
	private DateTime minDate = new(1900, 1, 1);

	#region Login
	[ObservableProperty]
	//[CustomValidation(typeof(AccountPageViewModel), nameof(ValidateLoginCredentials))]
	private string loginUsername;

	[ObservableProperty]
	//[CustomValidation(typeof(AccountPageViewModel), nameof(ValidateLoginCredentials))]
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
		// TODO: Uncomment this on production code
		var signupStatus = await userService.Signup(new User
		{
			Birthdate = SignUpBirthdate,
			Email = SignUpEmail,
			Gender = SignUpSelectedGender,
			Password = SignUpPassword,
			Username = SignUpUsername,
			LocalUserId = ObjectId.GenerateNewId()
		});
		//var faker = new Faker<User>()
		//	.RuleFor(p => p.Birthdate, v => v.Date.Past())
		//	.RuleFor(p => p.Email, v => v.Internet.Email())
		//	.RuleFor(p => p.Gender, v => v.PickRandom("Male", "Female"))
		//	.RuleFor(p => p.Password, v => v.Internet.Password())
		//	.RuleFor(p => p.Username, v => v.Internet.UserName())
		//	.RuleFor(p => p.LocalUserId, v => ObjectId.GenerateNewId())
		//	.Generate(1)[0];
		//var signupStatus = await userService.Signup(faker);
		IsLoading = false;

		if (signupStatus != "Success")
		{
			await Application.Current.MainPage.DisplayAlert("Couldn't sign up", $"An internal error has occured: {signupStatus}", "OK");
			return;
		}

		await Application.Current.MainPage.DisplayAlert("Success", "You have successfuly created an account! You may now login to HealthMate.", "OK")
			.ContinueWith(async _ =>
			{
				preferences.Set("HasUser", true);
				await NavigationService.PushAsync("///Tabs");
			});
	}

	[RelayCommand]
	private async Task Login()
	{
		var isValidLogin = !string.IsNullOrWhiteSpace(LoginUsername) && !string.IsNullOrWhiteSpace(LoginPassword);

		if (isValidLogin)
		{
			IsLoading = true;
			var loginSuccess = await userService.Login(LoginUsername, LoginPassword);
			if (loginSuccess)
			{
				await inventoryService.PopulateUserInventoryFromRemote();
				await scheduleService.PopulateUserScheduleFromRemote();
				preferences.Set("HasUser", true);
				IsLoading = false;
				await NavigationService.PushAsync("///Tabs");
				//TODO: https://stackoverflow.com/questions/72375482/shell-navigation-in-net-maui-to-a-page-with-bottom-tabs
			}
			else
				await Application.Current.MainPage.DisplayAlert("Couldn't log in", "Please check your login credentials if it's all correct.", "OK");
		}
		else
			await Application.Current.MainPage.DisplayAlert("Couldn't log in", "Please fill all the necessary fields in order to proceed.", "OK");
	}

	public static ValidationResult ValidateLoginCredentials(string entity, ValidationContext context)
	{
		var instance = (AccountPageViewModel)context.ObjectInstance;
		var isSignup = instance.IsSignup;

		return isSignup ? ValidationResult.Success : !string.IsNullOrWhiteSpace(entity) ? ValidationResult.Success : new("Validation error");
	}
}