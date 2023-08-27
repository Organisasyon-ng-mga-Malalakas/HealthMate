using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableObject, IQueryAttributable
{
    public BaseViewModel() { }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ReceiveParameters(query);
    }

    public virtual void OnNavigatedTo() { }

    protected virtual void ReceiveParameters(IDictionary<string, object> query) { }
}