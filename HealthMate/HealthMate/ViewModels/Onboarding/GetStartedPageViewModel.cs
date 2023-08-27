using CommunityToolkit.Mvvm.Input;
using HealthMate.Views.Onboarding;

namespace HealthMate.ViewModels.Onboarding;
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
