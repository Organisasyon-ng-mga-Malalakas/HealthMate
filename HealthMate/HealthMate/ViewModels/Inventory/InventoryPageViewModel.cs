using Bogus;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Models;
using HealthMate.Views.Inventory;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels.Inventory;
public partial class InventoryPageViewModel : BaseViewModel
{
    private readonly AddInventoryBottomSheetViewModel _addInventoryBottomSheetViewModel;

    [ObservableProperty]
    private ObservableCollection<InventoryGroup> inventory;

    public InventoryPageViewModel(AddInventoryBottomSheetViewModel addInventoryBottomSheetViewModel)
    {
        _addInventoryBottomSheetViewModel = addInventoryBottomSheetViewModel;
    }

    [RelayCommand]
    private async Task AddInventory()
    {
        var addInventory = new AddInventoryBottomSheet(_addInventoryBottomSheetViewModel);
        addInventory.Showing += (_, _) => addInventory.Controller.Behavior.DisableShapeAnimations();
        await addInventory.ShowAsync(true);
    }

    public override void OnNavigatedTo()
    {
        var fakeInventory = new Faker<Models.Tables.Inventory>()
            .RuleFor(p => p.BrandName, v => v.Name.FirstName())
            .RuleFor(p => p.MedicineName, v => v.Name.LastName())
            .RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
            .RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
            .RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
            .RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
            .RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
            .Generate(35);

        Inventory = new ObservableCollection<InventoryGroup>();
        var medicationTypes = Enum.GetValues<MedicationType>().Select(_ => _.ToString());
        foreach (var item in medicationTypes)
        {
            var enumRepresentation = Enum.Parse<MedicationType>(item);
            var listToAdd = fakeInventory.Where(_ => _.MedicationType == (int)enumRepresentation).ToList();
            if (listToAdd.Any())
                Inventory.Add(new InventoryGroup(item, listToAdd));
        }
    }
}
