using CommunityToolkit.Maui;
using Mopups.Hosting;
using Syncfusion.Maui.Core.Hosting;

namespace HealthMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UsePrismApp<App>(PrismStartup.Configure)
            .UseMauiCommunityToolkit()
            .ConfigureSyncfusionCore()
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Avenir-Black.ttf", "Bold");
                fonts.AddFont("Avenir-Heavy.ttf", "Medium");
                fonts.AddFont("Avenir-Regular.ttf", "Regular");
                fonts.AddFont("FontAwesome-Pro-Light-300.otf", "FALight");
                fonts.AddFont("FontAwesome-Pro-Regular-400.otf", "FARegular");
                fonts.AddFont("FontAwesome-Pro-Solid-900.otf", "FASolid");
                fonts.AddFont("FontAwesome-Pro-Thin-100.otf", "FAThin");
            })
            .ConfigureMopups();

        return builder.Build();
    }
}
