using Mopups.Interfaces;
using Mopups.Pages;

namespace HealthMate.Services;

public class PopupService(IPopupNavigation popupNavigation, IServiceProvider serviceProvider)
{
	public async Task ClosePopup()
	{
		await popupNavigation.PopAsync();
	}

	public async Task ShowPopup<TPopup>(params object[] parameters) where TPopup : PopupPage
	{
		var popup = ActivatorUtilities.CreateInstance<TPopup>(serviceProvider, parameters);
		await popupNavigation.PushAsync(popup);
	}
}