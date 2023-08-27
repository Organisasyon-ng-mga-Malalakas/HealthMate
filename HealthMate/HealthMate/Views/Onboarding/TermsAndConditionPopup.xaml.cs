using HealthMate.Templates;
using HealthMate.ViewModels.Onboarding;

namespace HealthMate.Views.Onboarding;

public partial class TermsAndConditionPopup : BasePopup<TermsAndConditionPopupViewModel>
{
    public TermsAndConditionPopup(TermsAndConditionPopupViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
