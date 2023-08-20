using HealthMate.Templates;
using Microsoft.Maui.Handlers;

namespace HealthMate.Platforms.Android.Renderers;
public class BorderlessPickerRenderer : ViewHandler<BorderlessPicker, PickerHandler>
{
    public BorderlessPickerRenderer(IPropertyMapper mapper, CommandMapper commandMapper = null) : base(mapper, commandMapper)
    {
    }

    //protected override void OnElementChanged(ElementChangedEventArgs<Picker> e)
    //{
    //    base.OnElementChanged(e);
    //    if (e.OldElement == null)
    //    {
    //        Control.Background = null;

    //        var layoutParams = new MarginLayoutParams(Control.LayoutParameters);
    //        layoutParams.SetMargins(0, 0, 0, 0);
    //        LayoutParameters = layoutParams;
    //        Control.LayoutParameters = layoutParams;
    //        Control.SetPadding(0, 0, 0, 0);
    //        SetPadding(0, 0, 0, 0);
    //    }
    //}
    protected override PickerHandler CreatePlatformView()
    {
        return new PickerHandler(Context, VirtualView);
    }
}
