using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Enums;
using HealthMate.EventArgs;
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
	private readonly PopupService _popupService;
	private readonly RealmService _realmService;

	[ObservableProperty]
	private ObservableCollection<InventoryGroup> inventory;
	[ObservableProperty]
	private bool isEmptyViewVisible;

	public InventoryPageViewModel(BottomSheetService bottomSheetService,
		NavigationService navigationService,
		PopupService popupService,
		RealmService realmService) : base(navigationService)
	{
		_bottomSheetService = bottomSheetService;
		_popupService = popupService;
		_realmService = realmService;

		WeakReferenceMessenger.Default.Register<InventoryDeletingEventArgs>(this, OnItemDeleting);
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

		if (changes.InsertedIndices.Length != 0)
			foreach (var item in changes.InsertedIndices)
			{
				if (sender.ElementAt(item).IsDeleted)
					return;

				var medicationType = ((MedicationType)sender[item].MedicationType).ToString();
				var correctGroup = Inventory.FirstOrDefault(_ => _.Category == medicationType);
				if (correctGroup is not InventoryGroup unwrappedGroup)
				{
					var inventory = new ObservableCollection<InventoryTable> { sender[item] };
					Inventory.Add(new InventoryGroup(medicationType, inventory));
					IsEmptyViewVisible = !Inventory.Any();
					return;
				}

				var indexOfCorrectGroup = Inventory.IndexOf(unwrappedGroup);
				Inventory[indexOfCorrectGroup].Add(sender[item]);
				IsEmptyViewVisible = !Inventory.Any();
			}
	}

	private void OnItemDeleting(object sender, InventoryDeletingEventArgs args)
	{
		var correctGroup = Inventory.FirstOrDefault(_ => _.Category == args.MedicationType);
		if (correctGroup is not null)
		{
			var itemToRemove = correctGroup.FirstOrDefault(_ => _.InventoryId == args.InventoryId);
			if (itemToRemove is not null)
			{
				correctGroup.Remove(itemToRemove);

				// Optionally, remove the group if it's now empty
				if (!correctGroup.Any())
					Inventory.Remove(correctGroup);
			}
		}

		IsEmptyViewVisible = !Inventory.Any();
	}

	public override async void OnNavigatedTo()
	{
		Inventory ??= [];
		var inventories = await _realmService.FindAll<InventoryTable>();
		RealmChangesNotification = inventories.SubscribeForNotifications(ListenForRealmChange);

		var realmInventoriesList = inventories.ToList().Where(_ => !_.IsDeleted);
		foreach (var newInventory in realmInventoriesList)
		{
			var groupName = ((MedicationType)newInventory.MedicationType).ToString();
			var existingGroup = Inventory.FirstOrDefault(g => g.Category == groupName);
			if (existingGroup == null)
			{
				// Group doesn't exist, create a new group and add the item
				var newGroup = new InventoryGroup(groupName, [newInventory]);
				Inventory.Add(newGroup);
			}
			else
				// Group exists, check if item already exists within the group based on InventoryId
				if (!existingGroup.Any(item => item.InventoryId == newInventory.InventoryId))
				// Item doesn't exist, add it to the group
				existingGroup.Add(newInventory);
		}

		#region Faker
		//var fakeInventory = new Faker<InventoryTable>()
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
		//    var listToAdd = fakeInventory.Where(_ => _.MedicationType == (int)item);
		//    if (listToAdd.Any())
		//        Inventory.Add(new InventoryGroup(item.ToString(), new ObservableCollection<InventoryTable>(listToAdd)));
		//}
		#endregion
		IsEmptyViewVisible = !Inventory.Any();
	}

	[RelayCommand]
	private async Task OpenInventoryDetailPopup(InventoryTable inventory)
	{
		await _popupService.ShowPopup<MedicineDetailPopup>(inventory);
	}
}