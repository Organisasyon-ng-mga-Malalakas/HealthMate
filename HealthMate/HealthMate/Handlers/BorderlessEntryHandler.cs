using AndroidX.AppCompat.Widget;
using HealthMate.Controls;
using Microsoft.Maui.Handlers;

namespace HealthMate.Handlers;
public partial class BorderlessEntryHandler : EntryHandler
{
    private static readonly PropertyMapper<BorderlessEntry, BorderlessEntryHandler> PropertyMapper = new(ViewMapper)
    {
        [nameof(BorderlessEntry.FontFamily)] = MapFont,
        [nameof(BorderlessEntry.FontSize)] = MapFont,
        [nameof(BorderlessEntry.Keyboard)] = MapKeyboard,
        [nameof(BorderlessEntry.TextColor)] = MapTextColor,
        [nameof(BorderlessEntry.Placeholder)] = MapPlaceholder,
        [nameof(BorderlessEntry.IsReadOnly)] = MapIsReadOnly
    };

    public BorderlessEntryHandler() : base(PropertyMapper) { }

    protected override AppCompatEditText CreatePlatformView()
    {
        return new AppCompatEditText(Context);
    }

    protected override void ConnectHandler(AppCompatEditText platformView)
    {
        base.ConnectHandler(platformView);
        platformView.Background = null;
        platformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        platformView.SetPadding(0, 0, 0, 0);
    }

    protected override void DisconnectHandler(AppCompatEditText platformView)
    {
        platformView?.Dispose();
        base.DisconnectHandler(platformView);
    }
}