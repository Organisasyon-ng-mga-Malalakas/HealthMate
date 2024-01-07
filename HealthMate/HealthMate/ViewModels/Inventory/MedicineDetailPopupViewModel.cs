using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Enums;
using HealthMate.EventArgs;
using HealthMate.Services;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;

public partial class MedicineDetailPopupViewModel(InventoryService inventoryService,
	NavigationService navigationService,
	PopupService popupService,
	RealmService realmService) : BaseViewModel(navigationService)
{
	[ObservableProperty]
	private InventoryTable passedInventory;

	[RelayCommand]
	private async Task ClosePopup()
	{
		await popupService.ClosePopup();
	}

	[RelayCommand]
	private async Task DecrementStock()
	{
		if (PassedInventory.Stock < 0)
			return;

		await realmService.Write(() => PassedInventory.Stock--);
		await inventoryService.UpsertInventory(null, PassedInventory);
	}

	[RelayCommand]
	private async Task DeleteInventory()
	{
		WeakReferenceMessenger.Default.Send(new InventoryDeletingEventArgs(PassedInventory.InventoryId, ((MedicationType)PassedInventory.MedicationType).ToString()));
		await realmService.Write(() => PassedInventory.IsDeleted = true);
		await inventoryService.DeleteInventory(PassedInventory);
		await ClosePopup();
	}

	[RelayCommand]
	private async Task IncrementStock()
	{
		await realmService.Write(() => PassedInventory.Stock++);
		await inventoryService.UpsertInventory(null, PassedInventory);
	}
}