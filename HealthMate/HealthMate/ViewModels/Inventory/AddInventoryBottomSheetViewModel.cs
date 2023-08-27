using CommunityToolkit.Mvvm.ComponentModel;
using HealthMate.Enums;
using HealthMate.Services;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Inventory;

public partial class AddInventoryBottomSheetViewModel : BaseViewModel
{
    private readonly DatabaseService _databaseService;

    [ObservableProperty]
    private ObservableCollection<Dosage> dosages;

    [ObservableProperty]
    private Dosage selectedDosage;

    [ObservableProperty]
    private ObservableCollection<MedicationType> medicationTypes;

    [ObservableProperty]
    private MedicationType selectedmedicationType;

    public AddInventoryBottomSheetViewModel(DatabaseService databaseService)
    {
        _databaseService = databaseService;
    }

    public override async void OnNavigatedTo()
    {
        await _databaseService.CreateTable<Models.Tables.Inventory>();
        Dosages = new ObservableCollection<Dosage>(Enum.GetValues<Dosage>());
        MedicationTypes = new ObservableCollection<MedicationType>(Enum.GetValues<MedicationType>());
    }
}