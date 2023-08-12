using HealthMate.Templates;
using HealthMate.ViewModels;
using Mopups.Interfaces;

namespace HealthMate.Services;

public class PopupService
{
    private readonly IPopupNavigation _popupNavigation;
    private readonly IServiceProvider _serviceProvider;

    public PopupService(IPopupNavigation popupNavigation, IServiceProvider serviceProvider)
    {
        _popupNavigation = popupNavigation;
        _serviceProvider = serviceProvider;
    }

    public async Task ClosePopup()
    {
        await _popupNavigation.PopAsync();
    }

    public async Task ShowPopup<TPopup, TPopupViewModel>()
        where TPopup : BasePopup<TPopupViewModel>
        where TPopupViewModel : BaseViewModel
    {
        var popupViewModel = _serviceProvider.GetRequiredService<TPopupViewModel>();
        var popup = (TPopup)Activator.CreateInstance(typeof(TPopup), popupViewModel);
        await _popupNavigation.PushAsync(popup);
    }
}
