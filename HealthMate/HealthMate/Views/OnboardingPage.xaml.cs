using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class OnboardingPage : BasePage<OnboardingPageViewModel>
{
    public OnboardingPage(OnboardingPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
