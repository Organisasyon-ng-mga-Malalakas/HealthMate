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

	[ObservableProperty]
	private ImageSource imageSource;

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

		if (string.IsNullOrWhiteSpace(PassedSchedule.PhotoBase64))
			return;

		var stream = new MemoryStream(Convert.FromBase64String(PassedSchedule.PhotoBase64));
		ImageSource = ImageSource.FromStream(() => stream);
	}
}