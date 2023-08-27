using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker;

namespace HealthMate.Views.SymptomChecker;

public partial class SymptomCheckerPage : BasePage<SymptomCheckerPageViewModel>
{
    public SymptomCheckerPage(SymptomCheckerPageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
    }
}