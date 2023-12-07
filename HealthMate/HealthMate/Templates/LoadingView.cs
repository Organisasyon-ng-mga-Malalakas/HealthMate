using CommunityToolkit.Maui.Markup;
using FFImageLoading.Maui;
using Color = Microsoft.Maui.Graphics.Color;

namespace HealthMate.Templates;

public class LoadingView : ContentView
{
	public LoadingView()
	{
		BindingContext = this;
		Content = new VerticalStackLayout
		{
			Spacing = 0,
			Children =
			{
				new CachedImage
				{
					Scale = 0.5,
					Source = "loading.gif"
				},

				new Label
				{
					FontFamily = (string)Application.Current.Resources["Regular"],
					TextColor = (Color)Application.Current.Resources["Pink"]
				}
				.Bind(Label.TextProperty, nameof(LoadingText), source: this)
				.CenterHorizontal()
				.Margins(top: -30)
			}
		}
		.Center();
		//.Bind(IsVisibleProperty, nameof(IsLoading), source: this);

		//this.Bind(IsVisibleProperty, nameof(IsLoading), source: this);
	}

	//public static readonly BindableProperty IsLoadingProperty =
	//	BindableProperty.Create(
	//	nameof(IsLoading),
	//	typeof(bool),
	//	typeof(LoadingView),
	//	false);
	//public bool IsLoading
	//{
	//	get => (bool)GetValue(IsLoadingProperty);
	//	set => SetValue(IsLoadingProperty, value);
	//}

	public static readonly BindableProperty LoadingTextProperty =
		BindableProperty.Create(
		nameof(LoadingText),
		typeof(string),
		typeof(LoadingView),
		"Loading...");
	public string LoadingText
	{
		get => (string)GetValue(LoadingTextProperty);
		set => SetValue(LoadingTextProperty, value);
	}
}