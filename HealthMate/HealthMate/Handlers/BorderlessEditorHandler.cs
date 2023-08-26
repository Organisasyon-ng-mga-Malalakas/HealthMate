using AndroidX.AppCompat.Widget;
using HealthMate.Controls;
using Microsoft.Maui.Handlers;

namespace HealthMate.Handlers;
public partial class BorderlessEditorHandler : EditorHandler
{
    private static readonly PropertyMapper<BorderlessEditor, BorderlessEditorHandler> PropertyMapper = new(ViewMapper)
    {
        [nameof(BorderlessEditor.FontFamily)] = MapFont,
        [nameof(BorderlessEditor.FontSize)] = MapFont,
        [nameof(BorderlessEditor.TextColor)] = MapTextColor
    };

    public BorderlessEditorHandler() : base(PropertyMapper) { }

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
