using HealthMate.Templates;
using HealthMate.ViewModels;
using Mopups.Interfaces;

namespace HealthMate.Services;

public class PopupService
{
    private readonly IContainerProvider _containerProvider;
    private readonly IPopupNavigation _popupNavigation;

    public PopupService(IContainerProvider containerProvider, IPopupNavigation popupNavigation)
    {
        _containerProvider = containerProvider;
        _popupNavigation = popupNavigation;
    }

    public async Task ClosePopup()
    {
        await _popupNavigation.PopAsync();
    }

    public async Task ShowPopup<TPopup, TPopupViewModel>()
        where TPopup : BasePopup<TPopupViewModel>
        where TPopupViewModel : BaseViewModel
    {
        var popupViewModel = _containerProvider.Resolve<TPopupViewModel>();
        var popup = (TPopup)Activator.CreateInstance(typeof(TPopup), popupViewModel);
        await _popupNavigation.PushAsync(popup);
    }
}
