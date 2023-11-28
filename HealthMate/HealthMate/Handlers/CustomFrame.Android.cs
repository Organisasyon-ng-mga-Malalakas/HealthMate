using Android.Content;
using Android.Graphics.Drawables;
using HealthMate.Controls;
using Microsoft.Maui.Controls.Handlers.Compatibility;
using Microsoft.Maui.Controls.Platform;
using Microsoft.Maui.Platform;
using System.ComponentModel;

namespace HealthMate.Platforms.Android.Renderers;
public class CustomFrameAndroid(Context context) : FrameRenderer(context)
{
	protected override void OnElementChanged(ElementChangedEventArgs<Frame> e)
	{
		base.OnElementChanged(e);

		if (e.NewElement != null && Control != null)
		{
			UpdateCornerRadius();
		}
	}

	protected override void OnElementPropertyChanged(object sender, PropertyChangedEventArgs e)
	{
		base.OnElementPropertyChanged(sender, e);

		if (e.PropertyName is (nameof(CustomFrame.CornerRadius)) or
			(nameof(CustomFrame)))
		{
			UpdateCornerRadius();
		}
	}

	private void UpdateCornerRadius()
	{
		if (Control.Background is GradientDrawable backgroundGradient)
		{
			var cornerRadius = (Element as CustomFrame)?.CornerRadius;
			if (!cornerRadius.HasValue)
			{
				return;
			}

			var topLeftCorner = Context.ToPixels(cornerRadius.Value.TopLeft);
			var topRightCorner = Context.ToPixels(cornerRadius.Value.TopRight);
			var bottomLeftCorner = Context.ToPixels(cornerRadius.Value.BottomLeft);
			var bottomRightCorner = Context.ToPixels(cornerRadius.Value.BottomRight);

			var cornerRadii = new[]
			{
				topLeftCorner,
				topLeftCorner,

				topRightCorner,
				topRightCorner,

				bottomRightCorner,
				bottomRightCorner,

				bottomLeftCorner,
				bottomLeftCorner,
			};

			backgroundGradient.SetCornerRadii(cornerRadii);
		}
	}
}