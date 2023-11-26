using CommunityToolkit.Maui.Markup;
using HealthMate.Constants;
using HealthMate.Services;

namespace HealthMate.Templates;

public class BackButton : ContentView
{
    public BackButton()
    {
        var navigationService = Application.Current.MainPage.Handler.MauiContext.Services.GetService<NavigationService>();
        Content = new Label
        {
            Margin = new Thickness(16, 10),
            FontFamily = (string)Application.Current.Resources["FARegular"],
            FontSize = 20,
            Text = FontAwesomeIcons.ArrowLeft
        }
        .TapGesture(async () => await navigationService.PopAsync());
    }
}