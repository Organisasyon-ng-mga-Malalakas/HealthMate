using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;

namespace HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;

public partial class IllnessInfoPopup : BasePopup<IllnessInfoPopupViewModel>
{
    public IllnessInfoPopup(IllnessInfoPopupViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}