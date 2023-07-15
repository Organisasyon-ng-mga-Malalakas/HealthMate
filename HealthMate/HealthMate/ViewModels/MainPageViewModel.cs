using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using HealthMate.Views;

namespace HealthMate.ViewModels;

public partial class MainPageViewModel : ObservableObject
{
    private readonly INavigationService _navigationService;
    private ISemanticScreenReader _screenReader { get; }

    public MainPageViewModel(ISemanticScreenReader screenReader, INavigationService navigationService)
    {
        _navigationService = navigationService;
        _screenReader = screenReader;
    }

    [ObservableProperty]
    private int count;
    [ObservableProperty]
    private string text = "Click me";
    [ObservableProperty]
    private string title = "Main Page";

    [RelayCommand]
    private void CountChanged()
    {
        //Count++;
        //if (Count == 1)
        //    Text = "Clicked 1 time";
        //else if (Count > 1)
        //    Text = $"Clicked {Count} times";

        //_screenReader.Announce(Text);
        _navigationService.NavigateAsync(nameof(Test), new NavigationParameters
        {
            { "Test", 69 }
        });
    }
}
