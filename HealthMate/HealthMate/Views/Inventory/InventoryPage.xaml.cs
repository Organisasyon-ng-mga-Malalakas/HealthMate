using HealthMate.Templates;
using HealthMate.ViewModels.Inventory;

namespace HealthMate.Views.Inventory;

public partial class InventoryPage : BasePage<InventoryPageViewModel>
{
    public InventoryPage(InventoryPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        //Inventories.DataSource.GroupDescriptors.Add(new GroupDescriptor()
        //{
        //    KeySelector = (object inventoryTable) =>
        //    {
        //        var inventory = inventoryTable as InventoryTable;
        //        return ((MedicationType)inventory.MedicationType).ToString();
        //    }
        //});
    }
}