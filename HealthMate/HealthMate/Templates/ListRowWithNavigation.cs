using CommunityToolkit.Maui.Markup;
using Microsoft.Maui.Controls.Shapes;

namespace HealthMate.Templates;
public class ListRowWithNavigation : ContentView
{
	public ListRowWithNavigation()
	{
		BindingContext = this;

		Content = new HorizontalStackLayout
		{
			Spacing = 6,
			Children =
			{
				new Border
				{
					BackgroundColor = Color.FromArgb("FDE5F0"),
					StrokeShape = new RoundRectangle
					{
						CornerRadius = 8
					},
					Content = new Label
					{
						FontFamily = (string)Application.Current.Resources["FASolid"],
						FontSize = 20,
						HorizontalOptions = LayoutOptions.Center,
						TextColor = (Color)Application.Current.Resources["Pink"]
					}
					.Bind(Label.TextProperty, nameof(Icon), source: this)
					.Padding(5)
				}
				.Size(35),

				new ContentView()
				.Bind(ContentProperty, nameof(ListRowContent), source: this)

			}
		};
	}

	public static readonly BindableProperty IconProperty =
		BindableProperty.Create(
		nameof(Icon),
		typeof(string),
		typeof(ListRowWithNavigation),
		default(string));
	public string Icon
	{
		get => (string)GetValue(IconProperty);
		set => SetValue(IconProperty, value);
	}

	public static readonly BindableProperty ListRowContentProperty =
		BindableProperty.Create(
		nameof(ListRowContent),
		typeof(View),
		typeof(ListRowWithNavigation),
		default(View));
	public View ListRowContent
	{
		get => (View)GetValue(ListRowContentProperty);
		set => SetValue(ListRowContentProperty, value);
	}
}

/*
  <HorizontalStackLayout Spacing="6">
                    <Border BackgroundColor="#FDE5F0"
                            HeightRequest="35"
                            StrokeShape="RoundRectangle 8"
                            WidthRequest="35">
                        <Label Padding="5"
                               FontFamily="{StaticResource FASolid}"
                               FontSize="20"
                               HorizontalOptions="Center"
                               Text="{Static constants:FontAwesomeIcons.Envelope}"
                               TextColor="{StaticResource Pink}" />
                    </Border>

                    <controls:BorderlessEntry FontFamily="{StaticResource Regular}"
                                              FontSize="16"
                                              Placeholder="Email address"
                                              PlaceholderColor="Gray"
                                              TextColor="Gray"
                                              VerticalOptions="Center" />
                </HorizontalStackLayout>

 */