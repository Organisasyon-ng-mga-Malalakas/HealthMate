using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableObject
{
    public BaseViewModel()
    {
        OnNavigatedTo();
    }

    protected virtual void OnNavigatedTo() { }

    public virtual void OnViewInitialized() { }
}