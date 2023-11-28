using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;

namespace HealthMate.Templates;
public class SettingsItem : ContentView
{
	public SettingsItem()
	{
		BindingContext = this;

		Content = new VerticalStackLayout
		{
			Children =
			{
				new Label { TextColor = (Color)Application.Current.Resources["Pink"] }
				.Bind(Label.TextProperty, nameof(Header), source: this)
				.Font((string)Application.Current.Resources["Medium"], 20),

				new Border
				{
					Stroke = Colors.Gray,
					StrokeShape = new RoundRectangle { CornerRadius = 5 },
					StrokeThickness = 0.5,
				}
				.Bind(Border.ContentProperty, nameof(ChildView), source: this)
				.Margin(10, 5)
			}
		};
	}

	public static readonly BindableProperty ChildViewProperty =
		BindableProperty.Create(
		nameof(ChildView),
		typeof(View),
		typeof(SettingsItem),
		default(View));
	public View ChildView
	{
		get => (View)GetValue(ChildViewProperty);
		set => SetValue(ChildViewProperty, value);
	}

	public static readonly BindableProperty HeaderProperty =
		BindableProperty.Create(
		nameof(Header),
		typeof(string),
		typeof(SettingsItem),
		default(string));
	public string Header
	{
		get => (string)GetValue(HeaderProperty);
		set => SetValue(HeaderProperty, value);
	}
}

/*
 <Label FontFamily="{StaticResource Medium}"
               FontSize="20"
               Text="Email Address"
               TextColor="{StaticResource Pink}" />

        <Border Margin="10,5"
                Stroke="Gray"
                StrokeShape="RoundRectangle, 8"
                StrokeThickness="0.5">
            <controls:BorderlessEntry Margin="10"
                                      FontFamily="{StaticResource Regular}"
                                      FontSize="16"
                                      PlaceholderColor="Gray"
                                      TextColor="Gray"
                                      VerticalOptions="Center" />
        </Border>
 
 */