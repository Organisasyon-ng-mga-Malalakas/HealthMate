using CommunityToolkit.Mvvm.ComponentModel;
using HealthMate.Services;
using System.ComponentModel;

namespace HealthMate.ViewModels;
public abstract class BaseViewModel : ObservableValidator, IQueryAttributable
{
    protected IDisposable RealmChangesNotification { get; set; }
    protected NavigationService NavigationService { get; }

    public BaseViewModel(NavigationService navigationService)
    {
        NavigationService = navigationService;
        Initialization();
        ValidateAllProperties();
    }

    public void ApplyQueryAttributes(IDictionary<string, object> query)
    {
        ReceiveParameters(query);
    }

    protected virtual void Initialization() { }

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