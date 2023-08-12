using CommunityToolkit.Mvvm.ComponentModel;
using HealthMate.Models;
using System.Collections.ObjectModel;

namespace HealthMate.ViewModels;
public partial class HomePageViewModel : BaseViewModel
{
    [ObservableProperty]
    private int selectedIndex = 0;
    [ObservableProperty]
    private ObservableCollection<TabItem> tabItems;

    public HomePageViewModel()
    {
    }

    //public override void OnNavigatedFrom(INavigationParameters parameters)
    //{

    //}

    //public override void OnNavigatedTo(INavigationParameters parameters)
    //{
    //    var icons = new string[3] { FontAwesomeIcons.CalendarClock, FontAwesomeIcons.Pills, FontAwesomeIcons.Person };
    //    var titles = new string[3] { "Schedule", "Inventory", "Symptom Checker" };
    //    TabItems = new ObservableCollection<TabItem>();

    //    for (var index = 0; index < 3; index++)
    //    {
    //        TabItems.Add(new TabItem
    //        {
    //            Icon = icons[index],
    //            IsSelected = index == 0,
    //            Title = titles[index]
    //        });
    //    }
    //}
}
