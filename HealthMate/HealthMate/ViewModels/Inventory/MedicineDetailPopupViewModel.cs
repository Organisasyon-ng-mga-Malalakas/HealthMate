using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using CommunityToolkit.Mvvm.Messaging;
using HealthMate.Enums;
using HealthMate.EventArgs;
using HealthMate.Services;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.ViewModels.Inventory;

public partial class MedicineDetailPopupViewModel : BaseViewModel
{
    private readonly PopupService _popupService;
    private readonly RealmService _realmService;

    [ObservableProperty]
    private InventoryTable passedInventory;

    public MedicineDetailPopupViewModel(PopupService popupService, RealmService realmService)
    {
        _popupService = popupService;
        _realmService = realmService;
    }

    [RelayCommand]
    private async Task ClosePopup()
    {
        await _popupService.ClosePopup();
    }

    [RelayCommand]
    private async Task DecrementStock()
    {
        if (PassedInventory.Stock < 0)
            return;

        await _realmService.Write(() => PassedInventory.Stock--);
    }

    [RelayCommand]
    private async Task DeleteInventory()
    {
        WeakReferenceMessenger.Default.Send(new InventoryDeletingEventArgs(PassedInventory.InventoryId, ((MedicationType)PassedInventory.MedicationType).ToString()));
        await _realmService.Write(() => PassedInventory.IsDeleted = true);
        await ClosePopup();
    }

    [RelayCommand]
    private async Task IncrementStock()
    {
        await _realmService.Write(() => PassedInventory.Stock++);
    }
}