using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Platforms.Android.Services;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using InventoryTable = HealthMate.Models.Tables.Inventory;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class AddScheduleBottomSheetViewModel(BottomSheetService bottomSheetService,
	InventoryService inventoryService,
	KeyboardService keyboardService,
	NavigationService navigationService,
	NotificationService notificationService,
	ScheduleService scheduleService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private DateTime endDate = DateTime.Now.AddDays(1);

	[ObservableProperty]
	private TimeSpan endTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1));

	[ObservableProperty]
	private ObservableCollection<InventoryTable> medicines;

	[ObservableProperty]
	private DateTime minimumDate = DateTime.Now;

	[ObservableProperty]
	private string notes;

	[ObservableProperty]
	private DateTime startDate = DateTime.Now;

	[ObservableProperty]
	private TimeSpan startTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromHours(1));

	[ObservableProperty]
	[Required]
	[Range(1, int.MaxValue)]
	private int takeEveryHr;

	[ObservableProperty]
	[Required]
	[Range(0, int.MaxValue)]
	private int takeEveryMin = 0;

	[ObservableProperty]
	[Required]
	[Range(0.1, double.MaxValue)]
	private double quantity;

	[ObservableProperty]
	[Required]
	private InventoryTable selectedMedicine;

	[RelayCommand]
	private async Task CloseBottomSheet()
	{
		keyboardService.HideKeyboard();
		await bottomSheetService.CloseBottomSheet();
	}

	[RelayCommand]
	private async Task CreateSchedule()
	{
		ValidateAllProperties();
		if (HasErrors)
			return;

		var startDateAndTime = new DateTime(StartDate.Year, StartDate.Month, StartDate.Day,
			StartTime.Hours, StartTime.Minutes, StartTime.Seconds);

		var interval = new TimeSpan(TakeEveryHr, TakeEveryMin, 0);

		var endDateAndTime = new DateTime(EndDate.Year, EndDate.Month, EndDate.Day,
			EndTime.Hours, EndTime.Minutes, EndTime.Seconds);

		var dates = new List<DateTime>();
		var currentDateandTime = startDateAndTime;
		while (currentDateandTime < endDateAndTime)
		{
			dates.Add(currentDateandTime);
			currentDateandTime = currentDateandTime.Add(interval);
		}

		var schedules = dates.Select(date => new ScheduleTable
		{
			Notes = Notes,
			ScheduleId = ObjectId.GenerateNewId(),
			ScheduleState = (int)ScheduleState.Pending,
			Inventory = SelectedMedicine,
			Quantity = Quantity,
			TimeToTake = new DateTimeOffset(date),
			UpdatedAt = DateTimeOffset.Now
		});
		await scheduleService.UpsertSchedule(schedules);

		var isNotificationEnabled = await notificationService.AskNotificationPermissionAsync();
		if (isNotificationEnabled)
		{
			var brandName = SelectedMedicine.BrandName;
			var medicineName = SelectedMedicine.MedicineName;
			var numberOfDose = SelectedMedicine.Dosage;
			var dosageType = ((Dosage)SelectedMedicine.DosageUnit).GetAcronym();
			var medicationReminders = new string[15]
			{
				$"Time for your medicine 🌟: Take {brandName} - {medicineName}, {numberOfDose} {dosageType}.",
				$"Daily reminder 📅: {brandName} - {medicineName}, {numberOfDose} {dosageType}. Keep up the good work!",
				$"Reminder 🕒: It's time for your {brandName} - {medicineName}, {numberOfDose} {dosageType}. Stay on track!",
				$"Health check 💚: Please take {numberOfDose} {dosageType} of {brandName} - {medicineName} now.",
				$"Your {numberOfDose} {dosageType} dose of {brandName} - {medicineName} is ready 🙌. Time to take it!",
				$"It's that special time for your medicine 🎉: {brandName} - {medicineName}, {numberOfDose} {dosageType}.",
				$"Stay on top of your health 💪: {brandName} - {medicineName}, {numberOfDose} {dosageType} is due now.",
				$"Health Check-in 🏥: {numberOfDose} {dosageType} of {brandName} - {medicineName} should be taken.",
				$"Friendly nudge 🌈: Time for your {brandName} - {medicineName}, {numberOfDose} {dosageType}.",
				$"Medication Alert ⏰: {numberOfDose} {dosageType} of {brandName} - {medicineName} is ready for you.",
				$"Your health matters 💖! It's time for {numberOfDose} {dosageType} of {brandName} - {medicineName}.",
				$"Care Reminder 💌: {numberOfDose} {dosageType} dose of {brandName} - {medicineName} time.",
				$"Wellness first 🌼: Time for {brandName} - {medicineName}, {numberOfDose} {dosageType}.",
				$"Take charge of your health today 💊: {brandName} - {medicineName}, {numberOfDose} {dosageType} needed.",
				$"Your prescription awaits 📜: Time for {numberOfDose} {dosageType} of {brandName} - {medicineName}."
			};

			foreach (var date in dates)
			{
				var selectedDescription = medicationReminders[Random.Shared.Next(14)];
				if (!string.IsNullOrWhiteSpace(Notes))
					selectedDescription += $"\n\nAdditional notes: {Notes}";

				//await notificationService.ScheduleNotification(selectedDescription, DateTime.Now.AddSeconds(5));
				await notificationService.ScheduleNotification(selectedDescription, date);
			}
		}

		await CloseBottomSheet();
	}

	public override async void OnNavigatedTo()
	{
		Medicines = new ObservableCollection<InventoryTable>(await inventoryService.GetInventoryForUser(_ => _.DeletedAt == null));
	}
}