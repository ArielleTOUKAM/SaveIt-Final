using Microsoft.Maui.Controls.Shapes;
using SaveIt.ViewModels;

namespace SaveIt;

public partial class MainPage : ContentPage
{
    private const string Blue = "#0D5BD7";
    private const string PaleBlue = "#EAF2FF";
    private const string Ink = "#182230";
    private const string Muted = "#66758A";
    private const string Line = "#D8E3F5";
    private const string Canvas = "#F3F5FA";
    private const string Green = "#15965B";
    private const string Red = "#E5484D";
    private const string Orange = "#C76A00";

    public MainViewModel ViewModel { get; }

    public MainPage()
    {
        InitializeComponent();
        ViewModel = new MainViewModel();
        BindingContext = ViewModel;

        ViewModel.PropertyChanged += (_, e) =>
        {
            if (e.PropertyName is nameof(MainViewModel.CurrentScreen) or nameof(MainViewModel.SelectedTab))
            {
                MainThread.BeginInvokeOnMainThread(RenderScreen);
            }
        };

        Root.SizeChanged += (_, _) =>
        {
            if (Root.Width > 0)
                Host.WidthRequest = Math.Min(390, Root.Width);
        };

        RenderScreen();
    }

    private void RenderScreen()
    {
        Host.Children.Clear();
        Host.BackgroundColor = ViewModel.CurrentScreen == "Splash" ? Colors.White : Color.FromArgb(Canvas);

        var page = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition { Height = GridLength.Star },
                new RowDefinition { Height = ViewModel.IsTabVisible ? GridLength.Auto : new GridLength(0) }
            }
        };

        page.Add(BuildScreen(ViewModel.CurrentScreen), 0, 0);

        if (ViewModel.IsTabVisible)
            page.Add(BuildBottomNavigation(), 0, 1);

        Host.Add(page);
    }

    private View BuildScreen(string screen) => screen switch
    {
        "Splash" => BuildSplash(),
        "Login" => BuildLogin(),
        "Registration" => BuildRegistration(),
        "Home" => BuildHome(),
        "Report" => BuildReport(),
        "Details" => BuildDetails(),
        "Matches" => BuildMatches(),
        "Notifications" => BuildNotifications(),
        "Conversation" => BuildConversation(),
        "Profile" => BuildProfile(),
        _ => BuildHome()
    };

    private View BuildBottomNavigation()
    {
        var tabs = new (string Name, string Icon, string TabKey)[]
        {
            ("Home", "⌂", "Home"),
            ("Reports", "📄", "Reports"),
            ("Matches", "⚡", "Matches"),
            ("Notifications", "🔔", "Notifications"),
            ("Profile", "👤", "Profile")
        };

        var bar = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            BackgroundColor = Colors.White,
            Padding = new Thickness(4, 6, 4, 8),
            ColumnSpacing = 0
        };

        for (var i = 0; i < tabs.Length; i++)
        {
            var tab = tabs[i];
            var active = ViewModel.SelectedTab == tab.TabKey;
            var tabContent = new VerticalStackLayout
            {
                Spacing = 2,
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    Text(tab.Icon, 16, false, active ? Blue : Muted),
                    Text(tab.Name, 10, active, active ? Blue : Muted)
                }
            };
            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => ViewModel.SelectTabCommand.Execute(tab.TabKey);
            tabContent.GestureRecognizers.Add(tap);
            bar.Add(tabContent, i, 0);
        }

        return new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(Line),
            StrokeThickness = 1,
            Content = bar
        };
    }

    private static ScrollView Scroll(View content) => new()
    {
        Content = new VerticalStackLayout
        {
            Padding = new Thickness(16, 12, 16, 24),
            Spacing = 14,
            Children = { content }
        }
    };

    private static Label Text(string value, double size = 14, bool bold = false, string color = Ink) => new()
    {
        Text = value,
        FontSize = size,
        FontFamily = bold ? "OpenSansSemibold" : "OpenSansRegular",
        TextColor = Color.FromArgb(color),
        LineBreakMode = LineBreakMode.WordWrap,
        VerticalTextAlignment = TextAlignment.Center
    };

    private static Label Text(string value, double size, bool bold, Color color) => new()
    {
        Text = value,
        FontSize = size,
        FontFamily = bold ? "OpenSansSemibold" : "OpenSansRegular",
        TextColor = color,
        LineBreakMode = LineBreakMode.WordWrap,
        VerticalTextAlignment = TextAlignment.Center
    };

    private static Button ActionButton(string title, Action action, bool primary = true, string? colorOverride = null)
    {
        var bg = colorOverride != null ? colorOverride : (primary ? Blue : PaleBlue);
        var fg = primary ? Colors.White : Color.FromArgb(Blue);
        var button = new Button
        {
            Text = title,
            FontSize = 15,
            FontFamily = "OpenSansSemibold",
            TextColor = fg,
            BackgroundColor = Color.FromArgb(bg),
            BorderColor = primary ? Color.FromArgb(bg) : Color.FromArgb(Line),
            BorderWidth = primary ? 0 : 1.5,
            CornerRadius = 12,
            HeightRequest = 52,
            Padding = new Thickness(16, 12),
            CharacterSpacing = 0.3
        };
        button.Clicked += (_, _) => action();
        return button;
    }

    private static Border Card(View content, string background = "#FFFFFF", double radius = 16, double shadowRadius = 8) => new()
    {
        BackgroundColor = Color.FromArgb(background),
        Stroke = Color.FromArgb(Line),
        StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(radius) },
        Padding = 16,
        Content = content,
        Shadow = new Shadow { Brush = Colors.Black, Offset = new Point(0, 2), Opacity = 0.08f, Radius = (float)shadowRadius }
    };

    private static Border InputField(Entry entry, string icon = "", string background = "#FFFFFF") => new()
    {
        BackgroundColor = Color.FromArgb(background),
        Stroke = Color.FromArgb(Line),
        StrokeThickness = 1.5,
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
        Padding = new Thickness(14, 12),
        Content = new HorizontalStackLayout
        {
            Spacing = 10,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                icon != "" ? Text(icon, 16, false, Muted) : new Label { WidthRequest = 0 },
                entry
            }
        }
    };

    private static Border StatusBadge(string text, string textColor, string bgColor) => new()
    {
        BackgroundColor = Color.FromArgb(bgColor),
        Stroke = Color.FromArgb(textColor),
        StrokeThickness = 1.5,
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(8) },
        Padding = new Thickness(10, 4),
        HorizontalOptions = LayoutOptions.Start,
        Content = Text(text, 12, true, textColor)
    };
}
