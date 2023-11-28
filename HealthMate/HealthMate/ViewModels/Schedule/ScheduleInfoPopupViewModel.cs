using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Services;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class ScheduleInfoPopupViewModel(NavigationService navigationService,
	PopupService popupService,
	RealmService realmService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	public int closeBtnColSpan;

	[ObservableProperty]
	public bool isMedsTakenBtnVisible;

	[ObservableProperty]
	private ScheduleTable passedSchedule;

	[RelayCommand]
	public async Task ClosePopup()
	{
		await popupService.ClosePopup();
	}

	[RelayCommand]
	public async Task MedsTaken()
	{
		await realmService.Write(() =>
		{
			PassedSchedule.ScheduleState = (int)ScheduleState.Taken;
			PassedSchedule.Inventory.Stock -= PassedSchedule.Quantity;
		});
		await ClosePopup();
	}

	public override void OnNavigatedTo()
	{
		CloseBtnColSpan = (ScheduleState)PassedSchedule.ScheduleState == ScheduleState.Taken ? 2 : 1;
		IsMedsTakenBtnVisible = (ScheduleState)PassedSchedule.ScheduleState == ScheduleState.Taken;
	}
}