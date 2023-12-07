using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Models.Tables;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Settings;
public partial class SettingsPageViewModel(NavigationService navigationService, RealmService realmService) : BaseViewModel(navigationService)
{
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
	private async void UpdateUserInfo()
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