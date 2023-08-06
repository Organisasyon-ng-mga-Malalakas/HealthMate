using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public partial class HomePageViewModel : BaseViewModel
{
    public HomePageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    [ObservableProperty]
    private int selectedIndex = 0;

    public override void OnNavigatedFrom(INavigationParameters parameters)
    {

    }

    public override void OnNavigatedTo(INavigationParameters parameters)
    {

    }
}
