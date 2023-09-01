using HealthMate.Templates;
using HealthMate.ViewModels.Inventory;

namespace HealthMate.Views.Inventory;

public partial class MedicineDetailPopup : BasePopup<MedicineDetailPopupViewModel>
{
    public MedicineDetailPopup(MedicineDetailPopupViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}