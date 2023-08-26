using HealthMate.ViewModels;
using The49.Maui.BottomSheet;

namespace HealthMate.Views;

public partial class MedicineScheduleBottomSheet : BottomSheet
{
    public MedicineScheduleBottomSheet(MedicineScheduleBottomSheetViewModel vm)
    {
        InitializeComponent();
        BindingContext = vm;
    }
}