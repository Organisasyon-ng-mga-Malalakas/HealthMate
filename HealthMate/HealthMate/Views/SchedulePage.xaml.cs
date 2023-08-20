using HealthMate.Templates;
using HealthMate.ViewModels;

namespace HealthMate.Views;

public partial class SchedulePage : BasePage<SchedulePageViewModel>
{
    public SchedulePage(SchedulePageViewModel viewModel) : base(viewModel)
    {
        InitializeComponent();
        var asa = test.HasVisualStateGroups();
    }
}