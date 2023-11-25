using CommunityToolkit.Mvvm.Input;

namespace HealthMate.ViewModels.SymptomChecker.DiseaseChecker;
public partial class DiseaseCheckerPageViewModel : BaseViewModel
{
    public DiseaseCheckerPageViewModel()
    {

    }

    [RelayCommand]
    private async Task PopPage()
    {
        await Shell.Current.GoToAsync("..", true);
    }
}
