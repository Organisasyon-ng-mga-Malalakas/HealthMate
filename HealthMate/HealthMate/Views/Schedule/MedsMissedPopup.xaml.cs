using HealthMate.Templates;
using HealthMate.ViewModels.Schedule;

namespace HealthMate.Views.Schedule;

public partial class MedsMissedPopup : BasePopup<MedsMissedPopupViewModel>
{
    public MedsMissedPopup(MedsMissedPopupViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}