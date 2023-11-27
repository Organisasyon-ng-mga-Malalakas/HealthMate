using HealthMate.Models;
using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker.BodyPicker.IllnessChecker;

namespace HealthMate.Views.SymptomChecker.BodyPicker.IllnessChecker;

public partial class IllnessInfoPopup : BasePopup<IllnessInfoPopupViewModel>
{
    public IllnessInfoPopup(IllnessInfoPopupViewModel vm, Diagnosis diagnosis, Services.HttpServices.Symptoms.BodyPart bodyPart) : base(vm)
    {
        InitializeComponent();
        vm.PassedBodyPart = bodyPart;
        vm.PassedDiagnosis = diagnosis;
    }
}