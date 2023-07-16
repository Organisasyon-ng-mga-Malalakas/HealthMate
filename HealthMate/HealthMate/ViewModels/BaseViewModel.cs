using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableObject, INavigationAware
{
    protected INavigationService NavigationService;

    public BaseViewModel(INavigationService navigationService)
    {
        NavigationService = navigationService;
    }

    public virtual void OnNavigatedFrom(INavigationParameters parameters)
    {
    }

    public virtual void OnNavigatedTo(INavigationParameters parameters)
    {
    }
}
