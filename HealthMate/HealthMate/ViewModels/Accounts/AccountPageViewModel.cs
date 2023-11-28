using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using System.ComponentModel.DataAnnotations;

namespace HealthMate.ViewModels.Accounts;

public partial class AccountPageViewModel(NavigationService navigationService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private string confirmPassword;

	[ObservableProperty]
	[Required]
	[EmailAddress]
	private string emailAddress;

	[ObservableProperty]
	private bool isSignup;

	[ObservableProperty]
	[Required]
	private string password;

	[ObservableProperty]
	private string username;

	[RelayCommand]
	private void ChangeAccountState(bool isSignup)
	{
		IsSignup = isSignup;
	}
}
