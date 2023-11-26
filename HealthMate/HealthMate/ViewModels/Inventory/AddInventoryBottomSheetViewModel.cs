using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Platforms.Android.Services;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;

public partial class AddInventoryBottomSheetViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly DatabaseService _databaseService;
    private readonly KeyboardService _keyboardService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    [Required]
    private string brandName;

    [ObservableProperty]
    private string description;

    [ObservableProperty]
    [Required]
    private double dosage;

    [ObservableProperty]
    private ObservableCollection<Dosage> dosages;

    [ObservableProperty]
    private bool isMedicationTypePickerEnabled;

    [ObservableProperty]
    private ObservableCollection<MedicationType> medicationTypes;

    [ObservableProperty]
    [Required]
    private string medicineName;

    [ObservableProperty]
    [Required]
    private Dosage? selectedDosage = null;

    [ObservableProperty]
    [Required]
    private MedicationType? selectedmedicationType = null;

    [ObservableProperty]
    [Required]
    private double stock;

    public AddInventoryBottomSheetViewModel(BottomSheetService bottomSheetService,
        DatabaseService databaseService,
        KeyboardService keyboardService,
        NavigationService navigationService,
        RealmService realmService) : base(navigationService)
    {
        _bottomSheetService = bottomSheetService;
        _databaseService = databaseService;
        _keyboardService = keyboardService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task AddInventory()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        await _realmService.Upsert(new InventoryTable
        {
            InventoryId = ObjectId.GenerateNewId(),
            BrandName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(BrandName).Trim(),
            Description = Description,
            Dosage = Dosage,
            DosageUnit = (int)SelectedDosage,
            IsDeleted = false,
            MedicationType = (int)SelectedmedicationType,
            MedicineName = MedicineName,
            Stock = Stock
        });

        await CloseBottomSheet();
    }

    [RelayCommand]
    private async Task CloseBottomSheet()
    {
        _keyboardService.HideKeyboard();
        await _bottomSheetService.CloseBottomSheet();
    }

    public override void OnNavigatedTo()
    {
        Dosages = new ObservableCollection<Dosage>(Enum.GetValues<Dosage>());
        //MedicationTypes = new ObservableCollection<MedicationType>(Enum.GetValues<MedicationType>());
    }

    partial void OnSelectedDosageChanged(Dosage? value)
    {
        if (value == null)
            return;
        IsMedicationTypePickerEnabled = value != null;
        MedicationTypes = new ObservableCollection<MedicationType>(value.Value.AllowedMedicationTypes());
    }
}