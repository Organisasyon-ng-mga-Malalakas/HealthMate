using HealthMate.Templates;
using HealthMate.ViewModels.Schedule;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.Views.Schedule;

public partial class ScheduleInfoPopup : BasePopup<ScheduleInfoPopupViewModel>
{
    public ScheduleInfoPopup(ScheduleInfoPopupViewModel viewModel, ScheduleTable schedule) : base(viewModel)
    {
        InitializeComponent();
        viewModel.PassedSchedule = schedule;
    }
}