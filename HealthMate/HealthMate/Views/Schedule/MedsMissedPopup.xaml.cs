using HealthMate.Templates;
using HealthMate.ViewModels.Schedule;
using ScheduleTable = HealthMate.Models.Tables.Schedule;

namespace HealthMate.Views.Schedule;

public partial class MedsMissedPopup : BasePopup<MedsMissedPopupViewModel>
{
	public MedsMissedPopup(MedsMissedPopupViewModel viewModel, ScheduleTable passedSchedule) : base(viewModel)
	{
		InitializeComponent();
		viewModel.PassedSchedule = passedSchedule;
	}
}