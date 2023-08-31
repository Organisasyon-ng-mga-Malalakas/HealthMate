using HealthMate.Templates;
using HealthMate.ViewModels.Inventory;

namespace HealthMate.Views.Inventory;

public partial class AddInventoryBottomSheet : BaseBottomSheet<AddInventoryBottomSheetViewModel>
{
    public AddInventoryBottomSheet(AddInventoryBottomSheetViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}