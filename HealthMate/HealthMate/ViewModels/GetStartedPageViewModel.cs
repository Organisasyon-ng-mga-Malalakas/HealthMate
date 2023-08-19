using CommunityToolkit.Mvvm.Input;
using HealthMate.Views;

namespace HealthMate.ViewModels;
public partial class GetStartedPageViewModel : BaseViewModel
{
    public GetStartedPageViewModel()
    {

    }

    [RelayCommand]
    private async Task GotoOnboarding()
    {
        await Shell.Current.GoToAsync($"{nameof(OnboardingPage)}", true);
    }
}
