namespace HealthMate;

public static class MauiProgram
{
    public static MauiApp CreateMauiApp()
    {
        var builder = MauiApp.CreateBuilder();
        builder
            .UsePrismApp<App>(PrismStartup.Configure)
            .ConfigureFonts(fonts =>
            {
                fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                fonts.AddFont("Avenir-Black.ttf", "Bold");
                fonts.AddFont("Avenir-Black.ttf", "Medium");
                fonts.AddFont("Avenir-Regular.ttf", "Regular");
            });

        return builder.Build();
    }
}
