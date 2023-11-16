using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker;

namespace HealthMate.Views.SymptomChecker;

public partial class DisclaimerPopup : BasePopup<DisclaimerPopupViewModel>
{
    public DisclaimerPopup(DisclaimerPopupViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}