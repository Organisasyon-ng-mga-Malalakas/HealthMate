using HealthMate.ViewModels;
using The49.Maui.BottomSheet;

namespace HealthMate.Templates;

public class BaseBottomSheet<TViewModel> : BottomSheet where TViewModel : BaseViewModel
{
    public BaseBottomSheet(in TViewModel viewModel)
    {
        BindingContext = viewModel;
        HasBackdrop = true;
        HasHandle = true;
        Detents = new List<Detent> { new ContentDetent() };
    }
}