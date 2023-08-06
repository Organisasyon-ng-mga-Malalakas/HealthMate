using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class HomePage : BasePage<HomePageViewModel>
{
    public HomePage(HomePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}
