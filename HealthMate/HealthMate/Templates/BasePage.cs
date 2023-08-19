using HealthMate.ViewModels;

namespace HealthMate.Templates;
public abstract class BasePage<TViewModel> : ContentPage where TViewModel : BaseViewModel
{
    protected BasePage(in TViewModel viewModel)
    {
        BindingContext = viewModel;
        //OnViewInitialized();
        //viewModel.OnViewInitialized();
    }

    protected virtual void OnViewInitialized() { }
}