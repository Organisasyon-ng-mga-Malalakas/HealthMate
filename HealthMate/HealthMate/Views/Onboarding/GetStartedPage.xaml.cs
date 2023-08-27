using HealthMate.Templates;
using HealthMate.ViewModels.Onboarding;

namespace HealthMate.Views.Onboarding;

public partial class GetStartedPage : BasePage<GetStartedPageViewModel>
{
    public GetStartedPage(GetStartedPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
