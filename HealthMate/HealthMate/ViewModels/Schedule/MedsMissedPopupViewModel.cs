using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using FFImageLoading;
using HealthMate.Services;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class MedsMissedPopupViewModel(IMediaPicker mediaPicker,
	NavigationService navigationService,
	PopupService popupService,
	RealmService realmService) : BaseViewModel(navigationService)
{
	private string _base64;

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
	private async Task MedsTaken()
	{
		await realmService.Write(() =>
		{
			PassedSchedule.ScheduleState = 0;
			PassedSchedule.PhotoBase64 = _base64;
		});
		await ClosePopup();
	}

	[RelayCommand]
	private async Task TakePhoto()
	{
		// Pick photo
		var photo = await mediaPicker.PickPhotoAsync();
		if (photo == null)
			return;

		var photoStream = await photo.OpenReadAsync();
		ImageSource = ImageSource.FromStream(() => photoStream);

		var bytes = (await photo.OpenReadAsync()).ToByteArray();
		_base64 = Convert.ToBase64String(bytes);
	}
}