using HealthMate.Templates;
using HealthMate.ViewModels.Onboarding;

namespace HealthMate.Views.Onboarding;

public partial class OnboardingPage : BasePage<OnboardingPageViewModel>
{
    public OnboardingPage(OnboardingPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
