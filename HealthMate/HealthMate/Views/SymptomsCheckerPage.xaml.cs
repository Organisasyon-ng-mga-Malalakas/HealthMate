using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class SymptomsCheckerPage : BasePage<SymptomsCheckerPageViewModel>
{
    public SymptomsCheckerPage(SymptomsCheckerPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}