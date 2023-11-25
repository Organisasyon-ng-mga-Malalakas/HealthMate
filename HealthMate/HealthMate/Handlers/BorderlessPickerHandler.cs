using HealthMate.Controls;
using Microsoft.Maui.Handlers;
using Microsoft.Maui.Platform;

namespace HealthMate.Handlers;
public partial class BorderlessPickerHandler : PickerHandler
{
    private static readonly PropertyMapper<BorderlessPicker, BorderlessPickerHandler> PropertyMapper = new(ViewMapper)
    {
        [nameof(BorderlessPicker.FontFamily)] = MapFont,
        [nameof(BorderlessPicker.FontSize)] = MapFont,
        [nameof(BorderlessPicker.TextColor)] = MapTextColor,
        [nameof(BorderlessPicker.Title)] = MapTitle,
        [nameof(BorderlessPicker.IsEnabled)] = MapIsEnabled
    };
    public BorderlessPickerHandler() : base(PropertyMapper) { }

    protected override MauiPicker CreatePlatformView()
    {
        return new MauiPicker(Context);
    }

    protected override void ConnectHandler(MauiPicker platformView)
    {
        base.ConnectHandler(platformView);
        platformView.Background = null;
        platformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        platformView.SetPadding(0, 0, 0, 0);
    }

    protected override void DisconnectHandler(MauiPicker platformView)
    {
        if (platformView != null)
        {
            platformView?.Dispose();
            base.DisconnectHandler(platformView);
        }
    }
}