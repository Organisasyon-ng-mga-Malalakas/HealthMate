using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
        //NavigationService = navigationService;
        OnNavigatedTo();
    }

    //public virtual void OnNavigatedFrom(INavigationParameters parameters)
    //{
    //}

    public virtual void OnNavigatedTo()
    {
    }
}
