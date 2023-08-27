using HealthMate.ViewModels.Inventory;
using The49.Maui.BottomSheet;

namespace HealthMate.Views.Inventory;

public partial class AddInventoryBottomSheet : BottomSheet
{
    public AddInventoryBottomSheet(AddInventoryBottomSheetViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}