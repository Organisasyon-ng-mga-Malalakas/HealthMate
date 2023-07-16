using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace HealthMate.ViewModels;

public partial class MainPageViewModel : BaseViewModel
{
    [ObservableProperty]
    private int count;
    [ObservableProperty]
    private string text = "Click me";
    [ObservableProperty]
    private string title = "Main Page";

    public MainPageViewModel(INavigationService navigationService) : base(navigationService)
    {
    }

    [RelayCommand]
    private void CountChanged()
    {
        //Count++;
        //if (Count == 1)
        //    Text = "Clicked 1 time";
        //else if (Count > 1)
        //    Text = $"Clicked {Count} times";

        //_screenReader.Announce(Text);
        //NavigationService.NavigateAsync(nameof(Test), new NavigationParameters
        //{
        //    { "Test", 69 }
        //});
    }
}
