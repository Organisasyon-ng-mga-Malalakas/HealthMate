using CommunityToolkit.Mvvm.Input;

namespace HealthMate.ViewModels.SymptomChecker.BodyPicker;
public partial class BodyPickerPageViewModel : BaseViewModel
{
    public BodyPickerPageViewModel()
    {

    }

    [RelayCommand]
    private async Task PopPage()
    {
        await Shell.Current.GoToAsync("..", true);
    }
}
