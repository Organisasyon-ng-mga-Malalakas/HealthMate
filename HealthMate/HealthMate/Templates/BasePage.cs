using CommunityToolkit.Maui.Behaviors;
using HealthMate.ViewModels;

namespace HealthMate.Templates;
public abstract class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
    protected BasePage(in TViewModel viewModel)
    {
        BindingContext = viewModel;
        //OnViewInitialized();
        //viewModel.OnViewInitialized();
        Behaviors.Add(new StatusBarBehavior
        {
            StatusBarColor = Colors.White,
            StatusBarStyle = CommunityToolkit.Maui.Core.StatusBarStyle.DarkContent
        });
    }

    protected virtual void OnViewInitialized() { }
}