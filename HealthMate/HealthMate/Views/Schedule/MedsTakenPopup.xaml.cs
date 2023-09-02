using HealthMate.Templates;
using HealthMate.ViewModels.Schedule;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.Views.Schedule;

public partial class MedsTakenPopup : BasePopup<MedsTakenPopupViewModel>
{
    public MedsTakenPopup(MedsTakenPopupViewModel viewModel, ScheduleTable passedSchedule) : base(viewModel)
    {
        InitializeComponent();
        viewModel.PassedSchedule = passedSchedule;
    }
}