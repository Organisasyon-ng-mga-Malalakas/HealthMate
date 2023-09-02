using HealthMate.Controls;
using Microsoft.Maui.Handlers;
using SearchView = AndroidX.AppCompat.Widget.SearchView;

namespace HealthMate.Handlers;
public class CustomSearchBarHandler : SearchBarHandler
{
    private static readonly PropertyMapper<CustomSearchBar, CustomSearchBarHandler> PropertyMapper = new(ViewMapper)
    {
        [nameof(CustomSearchBar.CancelButtonColor)] = MapCancelButtonColor,
        [nameof(CustomSearchBar.FontFamily)] = MapFont,
        [nameof(CustomSearchBar.TextColor)] = MapTextColor,
        [nameof(CustomSearchBar.IsTextPredictionEnabled)] = MapIsTextPredictionEnabled,
        [nameof(CustomSearchBar.Placeholder)] = MapPlaceholder,
        [nameof(CustomSearchBar.TextColor)] = MapTextColor
    };

    public CustomSearchBarHandler() : base(PropertyMapper) { }

    protected override SearchView CreatePlatformView()
    {
        return new SearchView(Context);
    }

    protected override void ConnectHandler(SearchView platformView)
    {
        base.ConnectHandler(platformView);
        //platformView.Background = null;
        //platformView.SetBackgroundColor(Android.Graphics.Color.Transparent);
        //platformView.SetPadding(0, 0, 0, 0);
        //platformView.BackgroundTintList = Android.Content.Res.ColorStateList.ValueOf(Colors.Transparent.ToAndroid());

        //var searchIcon = platformView.FindViewById<ImageView>(Resource.Id.search_mag_icon);
        //searchIcon.SetImageDrawable(null);
        //platformView.RemoveView(searchIcon);
    }

    protected override void DisconnectHandler(SearchView platformView)
    {
        platformView?.Dispose();
        base.DisconnectHandler(platformView);
    }
}