using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Interfaces;
using HealthMate.Platforms.Android.Services;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using InventoryTable = HealthMate.Models.Tables.Inventory;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.ViewModels.Schedule;
public partial class AddScheduleBottomSheetViewModel : BaseViewModel
{
    private readonly IAlarmScheduler _alarmScheduler;
    private readonly BottomSheetService _bottomSheetService;
    private readonly KeyboardService _keyboardService;
    private readonly NotificationService _notificationService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ObservableCollection<InventoryTable> medicines;

    [ObservableProperty]
    private DateTime minimumDate = DateTime.Now;

    [ObservableProperty]
    private string notes;

    [ObservableProperty]
    private DateTime notificationDate = DateTime.Now;

    [ObservableProperty]
    private TimeSpan notificationTime = DateTime.Now.TimeOfDay.Add(TimeSpan.FromMinutes(1));

    [ObservableProperty]
    [Required]
    [Range(0.1, double.MaxValue)]
    private double quantity;

    [ObservableProperty]
    [Required]
    private InventoryTable selectedMedicine;

    public AddScheduleBottomSheetViewModel(IAlarmScheduler alarmScheduler,
        BottomSheetService bottomSheetService,
        KeyboardService keyboardService,
        NotificationService notificationService,
        RealmService realmService)
    {
        _alarmScheduler = alarmScheduler;
        _bottomSheetService = bottomSheetService;
        _keyboardService = keyboardService;
        _notificationService = notificationService;
        _realmService = realmService;

        _alarmScheduler.ScheduleAlarm(DateTime.Now.AddSeconds(5));
    }

    [RelayCommand]
    private async Task CloseBottomSheet()
    {
        _keyboardService.HideKeyboard();
        await _bottomSheetService.CloseBottomSheet();
    }

    [RelayCommand]
    private async Task CreateSchedule()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        var cleanDateAndTime = new DateTime(NotificationDate.Year,
            NotificationDate.Month,
            NotificationDate.Day,
            NotificationTime.Hours,
            NotificationTime.Minutes,
            NotificationTime.Seconds);

        await _realmService.Upsert(new ScheduleTable
        {
            ScheduleId = ObjectId.GenerateNewId(),
            ScheduleState = (int)ScheduleState.Pending,
            Inventory = SelectedMedicine,
            Quantity = Quantity,
            TimeToTake = new DateTimeOffset(cleanDateAndTime)
        });

        var isNotificationEnabled = await _notificationService.AskNotificationPermissionAsync();
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

            var selectedDescription = medicationReminders[Random.Shared.Next(14)];
            if (!string.IsNullOrWhiteSpace(Notes))
                selectedDescription += $"\n\nAdditional notes: {Notes}";

            await _notificationService.ScheduleNotification(selectedDescription, cleanDateAndTime);
        }

        await CloseBottomSheet();
    }

    public override async void OnNavigatedTo()
    {
        var medicines = await _realmService.Find<InventoryTable>(_ => !_.IsDeleted);
        Medicines = new ObservableCollection<InventoryTable>(medicines);
    }
}