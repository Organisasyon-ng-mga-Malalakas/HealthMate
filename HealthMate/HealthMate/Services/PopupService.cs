using Mopups.Interfaces;
using Mopups.Pages;

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

    public async Task ShowPopup<TPopup>() where TPopup : PopupPage
    {
        var popup = ActivatorUtilities.CreateInstance<TPopup>(_serviceProvider);
        await _popupNavigation.PushAsync(popup);
    }
}
