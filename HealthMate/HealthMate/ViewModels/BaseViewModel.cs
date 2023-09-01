using CommunityToolkit.Mvvm.ComponentModel;
using System.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableValidator, IQueryAttributable
{
    protected IDisposable RealmChangesNotification { get; set; }
    public BaseViewModel()
    {
        ValidateAllProperties();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ReceiveParameters(query);
    }

    public virtual void OnNavigatedTo() { }

    public virtual void OnNavigatedFrom()
    {
        RealmChangesNotification?.Dispose();
    }

    protected override void OnPropertyChanged(PropertyChangedEventArgs e)
    {
        base.OnPropertyChanged(e);
        ValidateAllProperties();
    }

    protected virtual void ReceiveParameters(IDictionary<string, object> query) { }

    //public virtual void OnBottomSheetDismissed(Dictionary<string, object>? passedParameter) { }
}