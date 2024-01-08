using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models.Tables;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Settings;
public partial class SettingsPageViewModel(IBrowser browser,
	NavigationService navigationService,
	IPreferences preferences,
	RealmService realmService,
	UserService userService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private string avatar;

	[ObservableProperty]
	private string emailAddress;

	[ObservableProperty]
	private string username;

	[ObservableProperty]
	private DateTime birthDate;

	[ObservableProperty]
	private ObservableCollection<string> genders;

	[ObservableProperty]
	private string selectedGender;

	[ObservableProperty]
	private DateTime maxDate = DateTime.Now;
	public override async void OnNavigatedTo()
	{
		Avatar = preferences.Get("Avatar", "male01");
		Genders = ["Male", "Female"];

		var userData = await realmService.FindAll<User>();
		if (userData.Any() && userData.First() is User firstUserData)
		{
			EmailAddress = firstUserData.Email;
			Username = firstUserData.Username;
			SelectedGender = firstUserData.Gender;
			BirthDate = firstUserData.Birthdate.Date;
		}
	}

	[RelayCommand]
	private async Task DeleteAccount()
	{
		var doesAcceptDelete = await Application.Current.MainPage.DisplayAlert("Delete account", "Are you really sure you want to delete your account? This action cannot be undone.", "OK", "Cancel");
		if (doesAcceptDelete)
		{
			var isDeleted = await userService.DeleteAccount();
			if (isDeleted)
				NavigationService.PopToRoot();
		}
	}

	[RelayCommand]
	private async Task SignOut()
	{
		var isSignOut = await Application.Current.MainPage.DisplayAlert("Sign out", "Would you like to sign out of your account?", "OK", "Cancel");
		if (isSignOut)
		{
			await userService.DeleteLocalDatabase();
			NavigationService.PopToRoot();
		}
	}

	[RelayCommand]
	private Task<bool> OpenHealthMateWebsite()
	{
		var uri = new Uri("https://healthmate-pup.web.app/home");
		return browser.OpenAsync(uri, BrowserLaunchMode.SystemPreferred);
	}

	[RelayCommand]
	private async Task UpdateUserInfo()
	{
		var userInfo = new User
		{
			Birthdate = new DateTimeOffset(BirthDate),
			Email = EmailAddress,
			Gender = SelectedGender,
			LocalUserId = ObjectId.GenerateNewId(),
			Username = Username
		};
		await realmService.Upsert(userInfo);
	}
}