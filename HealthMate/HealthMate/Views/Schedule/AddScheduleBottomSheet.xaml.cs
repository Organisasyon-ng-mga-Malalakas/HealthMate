using HealthMate.ViewModels.Schedule;
using The49.Maui.BottomSheet;

namespace HealthMate.Views.Schedule;

public partial class AddScheduleBottomSheet : BottomSheet
{
    public AddScheduleBottomSheet(AddScheduleBottomSheetViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}