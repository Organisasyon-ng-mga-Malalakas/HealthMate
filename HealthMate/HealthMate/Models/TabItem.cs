namespace HealthMate.Models;

public class TabItem
{
    public Color BgColor => IsSelected ? Color.FromArgb("FFEFF6") : Colors.Transparent;
    public Color Color => (Color)Application.Current.Resources[IsSelected ? "Pink" : "RegBlack"];
    public string Icon { get; set; }
    public bool IsSelected { get; set; }
    public string Title { get; set; }
}
