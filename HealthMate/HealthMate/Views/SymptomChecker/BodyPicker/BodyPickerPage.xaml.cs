using HealthMate.Templates;
using HealthMate.ViewModels.SymptomChecker.BodyPicker;

namespace HealthMate.Views.SymptomChecker.BodyPicker;

public partial class BodyPickerPage : BasePage<BodyPickerPageViewModel>
{
    public BodyPickerPage(BodyPickerPageViewModel vm) : base(vm)
    {
        InitializeComponent();
    }
}