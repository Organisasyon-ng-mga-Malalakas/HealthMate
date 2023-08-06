using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class GetStartedPage : BasePage<GetStartedPageViewModel>
{
    public GetStartedPage(GetStartedPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
