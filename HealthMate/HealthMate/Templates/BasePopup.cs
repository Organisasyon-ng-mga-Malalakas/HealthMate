using HealthMate.ViewModels;
using Mopups.Pages;
using FadeAnimation = Mopups.Animations.FadeAnimation;

namespace HealthMate.Templates;
public abstract class BasePopup<TViewModel> : PopupPage where TViewModel : BaseViewModel
{
    private readonly TViewModel _viewModel;

    protected BasePopup(in TViewModel viewModel)
    {
        BindingContext = _viewModel = viewModel;
        Animation = new FadeAnimation
        {
            DurationIn = 250,
            DurationOut = 250,
            EasingIn = Easing.CubicIn,
            EasingOut = Easing.CubicOut
        };
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