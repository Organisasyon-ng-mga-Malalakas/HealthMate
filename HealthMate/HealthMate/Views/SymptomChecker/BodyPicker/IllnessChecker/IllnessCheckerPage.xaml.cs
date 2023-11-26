using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;

namespace HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;

public partial class IllnessCheckerPage : BasePage<IllnessCheckerPageViewModel>
{
    public IllnessCheckerPage(IllnessCheckerPageViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}