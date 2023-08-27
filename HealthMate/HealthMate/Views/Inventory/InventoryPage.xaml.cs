using HealthMate.Templates;
using HealthMate.ViewModels.Inventory;

namespace HealthMate.Views.Inventory;

public partial class InventoryPage : BasePage<InventoryPageViewModel>
{
    public InventoryPage(InventoryPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}