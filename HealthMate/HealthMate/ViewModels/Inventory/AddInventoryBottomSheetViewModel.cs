using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Services;
using MongoDB.Bson;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;

public partial class AddInventoryBottomSheetViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly DatabaseService _databaseService;
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
        RealmService realmService)
    {
        _bottomSheetService = bottomSheetService;
        _databaseService = databaseService;
        _realmService = realmService;
        ValidateAllProperties();
    }

    [RelayCommand]
    private async Task AddInventory()
    {
        ValidateAllProperties();
        if (HasErrors)
            return;

        await _realmService.Upsert(new InventoryTable
        {
            BrandName = CultureInfo.CurrentCulture.TextInfo.ToTitleCase(BrandName).Trim(),
            Description = Description,
            Dosage = Dosage,
            DosageUnit = (int)SelectedDosage,
            InventoryId = ObjectId.GenerateNewId(),
            MedicationType = (int)SelectedmedicationType,
            MedicineName = MedicineName,
            Stock = Stock
        });
        await CloseBottomSheet();
    }

    [RelayCommand]
    private async Task CloseBottomSheet()
    {
        await _bottomSheetService.CloseBottomSheet();
    }

    public override void OnNavigatedTo()
    {
        Dosages = new ObservableCollection<Dosage>(Enum.GetValues<Dosage>());
        MedicationTypes = new ObservableCollection<MedicationType>(Enum.GetValues<MedicationType>());
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        ValidateAllProperties();
    }
}