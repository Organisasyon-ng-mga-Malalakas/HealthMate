using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;

namespace HealthMate.ViewModels.Schedule;
public partial class MedsMissedPopupViewModel(NavigationService navigationService, PopupService popupService) : BaseViewModel(navigationService)
{
	[RelayCommand]
	public async Task ClosePopup()
	{
		await popupService.ClosePopup();
	}
}