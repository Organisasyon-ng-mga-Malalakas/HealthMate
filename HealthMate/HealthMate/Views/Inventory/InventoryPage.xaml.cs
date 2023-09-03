using HealthMate.Enums;
using HealthMate.Templates;
using HealthMate.ViewModels.Inventory;
using Syncfusion.Maui.DataSource;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.Views.Inventory;

public partial class InventoryPage : BasePage<InventoryPageViewModel>
{
    public InventoryPage(InventoryPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        Inventories.DataSource.GroupDescriptors.Add(new GroupDescriptor()
        {
            KeySelector = (object inventoryTable) =>
            {
                var inventory = inventoryTable as InventoryTable;
                return ((MedicationType)inventory.MedicationType).ToString();
            }
        });
    }
}