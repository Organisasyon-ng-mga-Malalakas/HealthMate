using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class InventoryPage : BasePage<InventoryPageViewModel>
{
    public InventoryPage(InventoryPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}