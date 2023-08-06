using HealthMate.ViewModels;
using Mopups.Pages;
using FadeAnimation = Mopups.Animations.FadeAnimation;

namespace HealthMate.Templates;
public abstract class BasePopup<TViewModel> : PopupPage where TViewModel : BaseViewModel
{
    protected BasePopup(in TViewModel viewModel)
    {
        BindingContext = viewModel;
        Animation = new FadeAnimation
        {
            DurationIn = 250,
            DurationOut = 250,
            EasingIn = Easing.CubicIn,
            EasingOut = Easing.CubicOut
        };
    }
}
