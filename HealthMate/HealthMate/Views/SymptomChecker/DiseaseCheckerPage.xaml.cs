using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker.DiseaseChecker;

namespace HealthMate.Views.SymptomChecker;

public partial class DiseaseCheckerPage : BasePage<DiseaseCheckerPageViewModel>
{
    public DiseaseCheckerPage(DiseaseCheckerPageViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}