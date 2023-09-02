using HealthMate.Templates;
using HealthMate.ViewModels.Inventory;
using InventoryTable = HealthMate.Models.Tables.Inventory;

namespace HealthMate.Views.Inventory;

public partial class MedicineDetailPopup : BasePopup<MedicineDetailPopupViewModel>
{
    public MedicineDetailPopup(MedicineDetailPopupViewModel viewModel, InventoryTable passedInventory) : base(viewModel)
    {
        InitializeComponent();
        viewModel.PassedInventory = passedInventory;
    }
}