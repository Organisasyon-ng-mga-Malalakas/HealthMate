using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableValidator, IQueryAttributable
{
    protected IDisposable RealmChangesNotification { get; set; }
    public BaseViewModel() { }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ReceiveParameters(query);
    }

    public virtual void OnNavigatedTo() { }

    public virtual void OnNavigatedFrom()
    {
        RealmChangesNotification?.Dispose();
    }

    protected virtual void ReceiveParameters(IDictionary<string, object> query) { }

    //public virtual void OnBottomSheetDismissed(Dictionary<string, object>? passedParameter) { }
}