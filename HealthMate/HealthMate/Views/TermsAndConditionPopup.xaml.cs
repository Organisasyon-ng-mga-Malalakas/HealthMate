using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class TermsAndConditionPopup : BasePopup<TermsAndConditionPopupViewModel>
{
    public TermsAndConditionPopup(TermsAndConditionPopupViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
