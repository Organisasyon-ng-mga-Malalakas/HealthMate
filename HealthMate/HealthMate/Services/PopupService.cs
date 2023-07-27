using Mopups.Interfaces;
using Mopups.Pages;

namespace HealthMate.Services;

public class PopupService
{
    private readonly IPopupNavigation _popupNavigation;

    public PopupService(IPopupNavigation popupNavigation)
    {
        _popupNavigation = popupNavigation;
    }

    public async Task ClosePopup()
    {
        await _popupNavigation.PopAsync();
    }

    public async Task ShowPopup<TPopup>() where TPopup : PopupPage, new()
    {
        var popup = new TPopup();
        await _popupNavigation.PushAsync(popup);
    }
}
