using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Enums;
using HealthMate.Models;
using HealthMate.Services;
using HealthMate.Views.Inventory;
using Realms;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;
public partial class InventoryPageViewModel : BaseViewModel
{
    private readonly BottomSheetService _bottomSheetService;
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private ObservableCollection<InventoryGroup> inventory;

    [ObservableProperty]
    private bool isActionBtnVisible;

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
    }

    private void ListenForRealmChange(IRealmCollection<InventoryTable> sender, ChangeSet changes)
    {
        if (changes == null)
            return;

        if (changes.InsertedIndices.Any())
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
                IsActionBtnVisible = Inventory.Any();
            }
    }

    private void OnInventoryCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
    {
        IsActionBtnVisible = Inventory.Any();
    }

    private void OnInventoryDelete(object _, InventoryTable deletedInventory)
    {
        try
        {
            var correctGroup = Inventory.FirstOrDefault(_ => _.GroupName == ((MedicationType)deletedInventory.MedicationType).ToString());
            if (correctGroup is not InventoryGroup unwrappedGroup)
                return;

            var indexOfCorrectGroup = Inventory.IndexOf(unwrappedGroup);
            Inventory[indexOfCorrectGroup].Remove(deletedInventory);

            if (!Inventory[indexOfCorrectGroup].Any())
                Inventory.Remove(Inventory[indexOfCorrectGroup]);

            IsActionBtnVisible = Inventory.Any();
        }
        catch (Exception)
        {

            throw;
        }
    }

    public override void OnNavigatedFrom()
    {
        base.OnNavigatedFrom();
        Inventory.CollectionChanged -= OnInventoryCollectionChanged;
    }

    public override async void OnNavigatedTo()
    {
        Inventory = new ObservableCollection<InventoryGroup>();
        Inventory.CollectionChanged += OnInventoryCollectionChanged;
        var allItems = await _realmService.FindAll<InventoryTable>();
        RealmChangesNotification = allItems.SubscribeForNotifications(ListenForRealmChange);
        foreach (var item in Enum.GetValues<MedicationType>())
        {
            var listToAdd = allItems.Where(_ => _.MedicationType == (int)item);
            if (listToAdd.Any())
                Inventory.Add(new InventoryGroup(item.ToString(), new ObservableCollection<InventoryTable>(listToAdd)));
        }

        if (!WeakReferenceMessenger.Default.IsRegistered<InventoryTable>(this))
            WeakReferenceMessenger.Default.Register<InventoryTable>(this, OnInventoryDelete);

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
        //foreach (var item in medicationTypes)
        //{
        //    var enumRepresentation = Enum.Parse<MedicationType>(item);
        //    var listToAdd = fakeInventory.Where(_ => _.MedicationType == (int)enumRepresentation).ToList();
        //    if (listToAdd.Any())
        //        Inventory.Add(new InventoryGroup(item, listToAdd));
        //}
        #endregion

        IsActionBtnVisible = Inventory.Any();
    }

    [RelayCommand]
    private async Task OpenInventoryDetailPopup(InventoryTable inventory)
    {
        await _popupService.ShowPopup<MedicineDetailPopup>(inventory);
    }
}