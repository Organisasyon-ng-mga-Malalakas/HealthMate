using CommunityToolkit.Maui.Behaviors;
using HealthMate.ViewModels;

namespace HealthMate.Templates;
public abstract class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
    private readonly TViewModel _viewModel;

    protected BasePage(in TViewModel viewModel)
    {
        BindingContext = _viewModel = viewModel;
        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.White,
            StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent
        });
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        _viewModel.OnNavigatedTo();
    }

    protected override void OnDisappearing()
    {
        base.OnDisappearing();
        _viewModel.OnNavigatedFrom();
    }
}