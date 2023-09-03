using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Services;
using HealthMate.Views.Inventory;
using Realms;
using System.Collections.ObjectModel;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;
public partial class InventoryPageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ObservableCollection<InventoryTable> inventory;

    public InventoryPageViewModel(BottomSheetService bottomSheetService,
        PopupService popupService,
        RealmService realmService)
    {
        _bottomSheetService = bottomSheetService;
        _popupService = popupService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task AddInventory()
    {
        await _bottomSheetService.OpenBottomSheet<AddInventoryBottomSheet>();

        //var fakeInventory = new Faker<Models.Tables.Inventory>()
        //   .RuleFor(p => p.BrandName, v => v.Name.FirstName())
        //   .RuleFor(p => p.MedicineName, v => v.Name.LastName())
        //   .RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
        //   .RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
        //   .RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
        //   .RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
        //   .RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
        //   .RuleFor(p => p.InventoryId, ObjectId.GenerateNewId())
        //   .Generate(1);
        //await _realmService.Upsert(fakeInventory[0]);
        //Inventory.Add(fakeInventory[0]);
    }

    private void ListenForRealmChange(IRealmCollection<InventoryTable> sender, ChangeSet changes)
    {
        if (changes == null)
            return;

        if (changes.DeletedIndices.Any())
            foreach (var item in changes.DeletedIndices)
                Inventory.RemoveAt(item);

        if (changes.InsertedIndices.Any())
            foreach (var item in changes.InsertedIndices)
                Inventory.Add(sender[item]);
    }

    public override async void OnNavigatedTo()
    {
        Inventory ??= new ObservableCollection<InventoryTable>();
        var inventories = await _realmService.FindAll<InventoryTable>();
        RealmChangesNotification = inventories.SubscribeForNotifications(ListenForRealmChange);

        var realmInventoriesList = inventories.ToList();
        var inventoriesList = Inventory.ToList();
        var itemsNotInInventory = realmInventoriesList.Where(realm => !inventoriesList.Any(schedules => schedules.InventoryId == realm.InventoryId));
        foreach (var item in itemsNotInInventory)
            Inventory.Add(item);

        #region Faker
        //var fakeInventory = new Faker<Models.Tables.Inventory>()
        //    .RuleFor(p => p.BrandName, v => v.Name.FirstName())
        //    .RuleFor(p => p.MedicineName, v => v.Name.LastName())
        //    .RuleFor(p => p.Dosage, v => v.Random.Int(0, 500))
        //    .RuleFor(p => p.DosageUnit, v => v.Random.Int(0, 8))
        //    .RuleFor(p => p.Stock, v => v.Random.Int(1, 100))
        //    .RuleFor(p => p.MedicationType, v => v.Random.Int(0, 9))
        //    .RuleFor(p => p.Description, v => v.Lorem.Paragraph(5))
        //    .Generate(35);
        //foreach (var item in Enum.GetValues<MedicationType>())
        //{
        //    //var enumRepresentation = Enum.Parse<MedicationType>(item);
        //    var listToAdd = fakeInventory.Where(_ => _.MedicationType == (int)item);
        //    if (listToAdd.Any())
        //        Inventory.Add(new InventoryGroup(item.ToString(), new ObservableCollection<InventoryTable>(listToAdd)));
        //}
        #endregion
    }

    [RelayCommand]
    private async Task OpenInventoryDetailPopup(Syncfusion.Maui.ListView.ItemTappedEventArgs args)
    {
        var inventory = (InventoryTable)args.DataItem;
        await _popupService.ShowPopup<MedicineDetailPopup>(inventory);
    }
}