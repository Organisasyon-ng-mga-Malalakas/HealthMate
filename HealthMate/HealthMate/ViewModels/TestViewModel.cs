using CommunityToolkit.Mvvm.ComponentModel;

namespace HealthMate.ViewModels;
public partial class TestViewModel : ObservableObject, INavigationAware
{
    public void OnNavigatedFrom(INavigationParameters parameters)
    {

    }

    public void OnNavigatedTo(INavigationParameters parameters)
    {
        var parameters1 = parameters.GetValue<int>("Test");
    }
}
