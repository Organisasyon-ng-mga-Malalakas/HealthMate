using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Enums;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views.Inventory;
using Realms;
using System.Collections.ObjectModel;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;
public partial class InventoryPageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly DatabaseService _databaseService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ObservableCollection<InventoryGroup> inventory;

    public InventoryPageViewModel(BottomSheetService bottomSheetService,
        DatabaseService databaseService,
        RealmService realmService)
    {
        _bottomSheetService = bottomSheetService;
        _databaseService = databaseService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task AddInventory()
    {
        await _bottomSheetService.OpenBottomSheet<AddInventoryBottomSheet>(this);
    }

    public override async void OnNavigatedTo()
    {
        Inventory = new ObservableCollection<InventoryGroup>();
        var allItems = await _realmService.FindAll<InventoryTable>();
        RealmChangesNotification = allItems.SubscribeForNotifications(ListenForRealmChange);
        foreach (var item in Enum.GetValues<MedicationType>())
        {
            var listToAdd = allItems.Where(_ => _.MedicationType == (int)item);
            if (listToAdd.Any())
                Inventory.Add(new InventoryGroup(item.ToString(), new ObservableCollection<InventoryTable>(listToAdd)));
        }

        //var fakeInventory = new Faker<Models.Tables.Inventory>()
        //    .RuleFor(p => p.BrandName, v => v.Name.FirstName())
        //    .RuleFor(p => p.MedicineName, v => v.Name.LastName())
        //    .RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
        //    .RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
        //    .RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
        //    .RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
        //    .RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
        //    .Generate(35);
        //foreach (var item in medicationTypes)
        //{
        //    var enumRepresentation = Enum.Parse<MedicationType>(item);
        //    var listToAdd = fakeInventory.Where(_ => _.MedicationType == (int)enumRepresentation).ToList();
        //    if (listToAdd.Any())
        //        Inventory.Add(new InventoryGroup(item, listToAdd));
        //}
    }

    private void ListenForRealmChange(IRealmCollection<InventoryTable> sender, ChangeSet changes)
    {
        if (changes == null)
            return;

        foreach (var item in changes.InsertedIndices)
        {
            var medicationType = ((MedicationType)sender[item].MedicationType).ToString();
            var correctGroup = Inventory.FirstOrDefault(_ => _.GroupName == medicationType);
            if (correctGroup is not InventoryGroup unwrappedGroup)
            {
                var inventory = new ObservableCollection<InventoryTable> { sender[item] };
                Inventory.Add(new InventoryGroup(medicationType, inventory));
                return;
            }

            var indexOfCorrectGroup = Inventory.IndexOf(unwrappedGroup);
            Inventory[indexOfCorrectGroup].Add(sender[item]);
        }
    }
}
