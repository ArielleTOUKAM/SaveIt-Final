using Microsoft.Maui.Controls.Shapes;
using SaveIt.Models;

namespace SaveIt;

public partial class MainPage
{
    // Exact Brand Palette from Figma Mockup
    private const string FigmaBlue = "#1062DE";
    private const string FigmaLightBlue = "#EAF2FE";
    private const string FigmaAccentBlue = "#4D92F8";
    private const string FigmaTextDark = "#182230";
    private const string FigmaTextMuted = "#66758A";
    private const string FigmaBorder = "#DDE5F2";
    private const string FigmaRed = "#E5484D";
    private const string FigmaRedBg = "#FEECEE";
    private const string FigmaGreen = "#15965B";
    private const string FigmaGreenBg = "#EAF7ED";
    private const string FigmaCardBg = "#FFFFFF";

    // ==========================================
    // 01. SPLASH SCREEN (Page 1/10)
    // ==========================================
    private View BuildSplash()
    {
        // Custom Figma Logo: Cupped hand supporting a blue location pin
        var handAndPinLogo = new Grid
        {
            WidthRequest = 110,
            HeightRequest = 110,
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                // Light Blue Hand
                new Path
                {
                    Fill = Color.FromArgb(FigmaAccentBlue),
                    Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(
                        "M 20 62 C 20 54 28 46 38 46 C 44 46 48 50 54 55 C 60 59 66 60 74 58 C 80 57 86 51 86 44 C 86 42 88 40 91 42 C 94 44 95 48 93 54 C 88 68 76 78 58 78 C 42 78 30 72 20 62 Z")!
                },
                // Solid Royal Blue Pin
                new Path
                {
                    Fill = Color.FromArgb(FigmaBlue),
                    Data = (Geometry)new PathGeometryConverter().ConvertFromInvariantString(
                        "M 55 14 C 43 14 34 23 34 35 C 34 48 55 64 55 64 C 55 64 76 48 76 35 C 76 23 67 14 55 14 Z M 55 41 C 51.5 41 48.5 38 48.5 34.5 C 48.5 31 51.5 28 55 28 C 58.5 28 61.5 31 61.5 34.5 C 61.5 38 58.5 41 55 41 Z")!
                }
            }
        };

        var dotsLoader = new HorizontalStackLayout
        {
            Spacing = 7,
            HorizontalOptions = LayoutOptions.Center,
            Children =
            {
                new Ellipse { WidthRequest = 6, HeightRequest = 6, Fill = Color.FromArgb(FigmaBlue) },
                new Ellipse { WidthRequest = 6, HeightRequest = 6, Fill = Color.FromArgb(FigmaBlue) },
                new Ellipse { WidthRequest = 6, HeightRequest = 6, Fill = Color.FromArgb(FigmaBlue) }
            }
        };

        var centerContent = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Spacing = 10,
            Children =
            {
                handAndPinLogo,
                Text("SaveIt", 28, true, FigmaBlue),
                Text("Lost. Found. Reunited.", 13, false, FigmaTextMuted)
            }
        };

        var bottomLoader = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.End,
            Spacing = 8,
            Margin = new Thickness(0, 0, 0, 48),
            Children =
            {
                dotsLoader,
                Text("Loading...", 11, false, FigmaTextMuted)
            }
        };

        return new Grid
        {
            BackgroundColor = Colors.White,
            Children = { centerContent, bottomLoader }
        };
    }

    // ==========================================
    // 02. LOGIN SCREEN (Page 2/10) - Welcome Back !
    // ==========================================
    private View BuildLogin()
    {
        var vm = ViewModel.Auth;

        var backBtn = new Button
        {
            Text = "←",
            FontSize = 22,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            HeightRequest = 32,
            Command = new Command(() => ViewModel.NavigateTo("Splash"))
        };

        var brandRow = new HorizontalStackLayout
        {
            Spacing = 6,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                BrandPinIcon(18),
                Text("SaveIt", 17, true, FigmaBlue)
            }
        };

        var emailInput = new Entry
        {
            Text = vm.Email,
            Placeholder = "you@example.com",
            Keyboard = Keyboard.Email,
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        emailInput.TextChanged += (_, e) => vm.Email = e.NewTextValue;

        var passInput = new Entry
        {
            Text = vm.Password,
            Placeholder = "••••••••",
            IsPassword = true,
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        passInput.TextChanged += (_, e) => vm.Password = e.NewTextValue;

        var eyeToggle = new Button
        {
            Text = "👁",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextMuted),
            BackgroundColor = Colors.Transparent,
            Padding = 0,
            WidthRequest = 30
        };
        eyeToggle.Clicked += (_, _) => passInput.IsPassword = !passInput.IsPassword;

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                backBtn,
                brandRow,
                new VerticalStackLayout
                {
                    Spacing = 3,
                    Children =
                    {
                        Text("Welcome Back !", 24, true, FigmaTextDark),
                        Text("Log In to your account", 12, false, FigmaTextMuted)
                    }
                },
                FigmaFieldLabel("Email address"),
                FigmaInputBox("✉", emailInput),

                FigmaFieldLabel("Password"),
                FigmaInputBoxWithSuffix("🔒", passInput, eyeToggle),

                new Button
                {
                    Text = "Forgot password?",
                    FontSize = 12,
                    TextColor = Color.FromArgb(FigmaBlue),
                    BackgroundColor = Colors.Transparent,
                    HorizontalOptions = LayoutOptions.Start,
                    Padding = 0,
                    Command = vm.ForgotPasswordCommand
                },

                FigmaPrimaryButton("Log In", () => vm.LoginCommand.Execute(null)),

                new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
                    ColumnSpacing = 10,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        new BoxView { HeightRequest = 1, Color = Color.FromArgb(FigmaBorder), VerticalOptions = LayoutOptions.Center },
                        Text("or", 12, false, FigmaTextMuted),
                        new BoxView { HeightRequest = 1, Color = Color.FromArgb(FigmaBorder), VerticalOptions = LayoutOptions.Center }
                    }
                },

                FigmaOutlinedButton("🌐   Continue with Google", () => vm.GoogleSignInCommand.Execute(null)),

                new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 4,
                    Margin = new Thickness(0, 10, 0, 0),
                    Children =
                    {
                        Text("Don't have an account?", 12, false, FigmaTextMuted),
                        new Button
                        {
                            Text = "Sign Up",
                            FontSize = 12,
                            FontFamily = "OpenSansSemibold",
                            TextColor = Color.FromArgb(FigmaBlue),
                            BackgroundColor = Colors.Transparent,
                            Padding = 0,
                            Command = vm.ToggleModeCommand
                        }
                    }
                }
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 03. CREATE ACCOUNT SCREEN (Page 3/10)
    // ==========================================
    private View BuildRegistration()
    {
        var vm = ViewModel.Auth;

        var backBtn = new Button
        {
            Text = "←",
            FontSize = 22,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            HeightRequest = 32,
            Command = vm.ToggleModeCommand
        };

        var nameInput = new Entry
        {
            Placeholder = "Full name",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var emailInput = new Entry
        {
            Text = "invalid@email",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var phoneInput = new Entry
        {
            Placeholder = "Phone number",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var passInput = new Entry
        {
            Text = "Pass123",
            IsPassword = true,
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var eye1 = new Button { Text = "👁", FontSize = 13, TextColor = Color.FromArgb(FigmaTextMuted), BackgroundColor = Colors.Transparent, Padding = 0, WidthRequest = 30 };
        eye1.Clicked += (_, _) => passInput.IsPassword = !passInput.IsPassword;

        var strengthBar = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Auto)
            },
            ColumnSpacing = 6,
            Children =
            {
                new BoxView { HeightRequest = 4, Color = Color.FromArgb(FigmaBlue), CornerRadius = 2 },
                new BoxView { HeightRequest = 4, Color = Color.FromArgb(FigmaBlue), CornerRadius = 2 },
                new BoxView { HeightRequest = 4, Color = Color.FromArgb(FigmaBorder), CornerRadius = 2 },
                Text("Medium", 10, false, FigmaTextMuted)
            }
        };

        var confirmInput = new Entry
        {
            Placeholder = "Confirm password",
            IsPassword = true,
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var eye2 = new Button { Text = "👁", FontSize = 13, TextColor = Color.FromArgb(FigmaTextMuted), BackgroundColor = Colors.Transparent, Padding = 0, WidthRequest = 30 };
        eye2.Clicked += (_, _) => confirmInput.IsPassword = !confirmInput.IsPassword;

        var body = new VerticalStackLayout
        {
            Spacing = 13,
            Children =
            {
                backBtn,
                new VerticalStackLayout
                {
                    Spacing = 3,
                    Children =
                    {
                        Text("Create Account", 24, true, FigmaTextDark),
                        Text("Join SaveIt and help reunite people with their belongings", 12, false, FigmaTextMuted)
                    }
                },

                FigmaInputBox("👤", nameInput),

                new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        FigmaInputBoxWithError("✉", emailInput),
                        Text("Please enter a valid email address.", 11, false, FigmaRed)
                    }
                },

                FigmaInputBoxFocused("📞", phoneInput),

                new VerticalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        FigmaInputBoxWithSuffix("🔒", passInput, eye1),
                        strengthBar
                    }
                },

                new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        FigmaInputBoxWithErrorWithSuffix("🔒", confirmInput, eye2),
                        Text("Passwords do not match.", 11, false, FigmaRed)
                    }
                },

                new HorizontalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        new Border
                        {
                            WidthRequest = 18,
                            HeightRequest = 18,
                            Stroke = Color.FromArgb(FigmaBlue),
                            StrokeThickness = 1.5,
                            StrokeShape = new RoundRectangle { CornerRadius = 4 },
                            BackgroundColor = Colors.White
                        },
                        new FormattedString
                        {
                            Spans =
                            {
                                new Span { Text = "I agree to the ", TextColor = Color.FromArgb(FigmaTextDark), FontSize = 11 },
                                new Span { Text = "Terms of Service", TextColor = Color.FromArgb(FigmaBlue), FontAttributes = FontAttributes.Bold, FontSize = 11 },
                                new Span { Text = " and ", TextColor = Color.FromArgb(FigmaTextDark), FontSize = 11 },
                                new Span { Text = "Privacy Policy", TextColor = Color.FromArgb(FigmaBlue), FontAttributes = FontAttributes.Bold, FontSize = 11 }
                            }
                        }
                    }
                },

                FigmaButtonDisabled("Create Account", () => vm.RegisterCommand.Execute(null)),

                new HorizontalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 4,
                    Children =
                    {
                        Text("Already have an account?", 12, false, FigmaTextMuted),
                        new Button
                        {
                            Text = "Log In",
                            FontSize = 12,
                            FontFamily = "OpenSansSemibold",
                            TextColor = Color.FromArgb(FigmaBlue),
                            BackgroundColor = Colors.Transparent,
                            Padding = 0,
                            Command = vm.ToggleModeCommand
                        }
                    }
                }
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 04. HOME SCREEN (Page 4/10) - Exact Figma Layout
    // ==========================================
    private View BuildHome()
    {
        var vm = ViewModel.Home;

        var greetingRow = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            Children =
            {
                new VerticalStackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        Text("Good morning 👋", 11, false, FigmaTextMuted),
                        Text("Hello, Arielle!", 20, true, FigmaTextDark)
                    }
                },
                new Grid
                {
                    WidthRequest = 38,
                    HeightRequest = 38,
                    Children =
                    {
                        new Border
                        {
                            Stroke = Color.FromArgb(FigmaBorder),
                            StrokeThickness = 1,
                            StrokeShape = new RoundRectangle { CornerRadius = 19 },
                            BackgroundColor = Colors.White,
                            Content = Text("🔔", 16, false, FigmaTextDark)
                        },
                        new Ellipse
                        {
                            WidthRequest = 9,
                            HeightRequest = 9,
                            Fill = Color.FromArgb(FigmaRed),
                            HorizontalOptions = LayoutOptions.End,
                            VerticalOptions = LayoutOptions.Start,
                            Margin = new Thickness(0, 4, 4, 0)
                        }
                    }
                }
            }
        };

        var searchEntry = new Entry
        {
            Placeholder = "Search a lost or found item...",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            PlaceholderColor = Color.FromArgb(FigmaTextMuted),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        searchEntry.TextChanged += (_, e) => vm.SearchQuery = e.NewTextValue;

        var searchBox = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 0),
            HeightRequest = 46,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                ColumnSpacing = 10,
                Children =
                {
                    Text("🔍", 14, false, FigmaTextMuted),
                    searchEntry,
                    Text("☵", 15, false, FigmaBlue)
                }
            }
        };

        var actionCards = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 10
        };

        var reportLostCard = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaBlue),
            Stroke = Color.FromArgb(FigmaBlue),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 10),
            Content = new VerticalStackLayout
            {
                Spacing = 3,
                Children =
                {
                    Text("⚠ Report Lost", 14, true, "#FFFFFF"),
                    Text("Declare a missing item", 11, false, FigmaLightBlue)
                }
            }
        };
        var tapLost = new TapGestureRecognizer();
        tapLost.Tapped += (_, _) => vm.ReportLostCommand.Execute(null);
        reportLostCard.GestureRecognizers.Add(tapLost);
        actionCards.Add(reportLostCard, 0, 0);

        var reportFoundCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBlue),
            StrokeThickness = 1.5,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 10),
            Content = new VerticalStackLayout
            {
                Spacing = 3,
                Children =
                {
                    Text("✓ Report Found", 14, true, FigmaBlue),
                    Text("Declare a found item", 11, false, FigmaTextMuted)
                }
            }
        };
        var tapFound = new TapGestureRecognizer();
        tapFound.Tapped += (_, _) => vm.ReportFoundCommand.Execute(null);
        reportFoundCard.GestureRecognizers.Add(tapFound);
        actionCards.Add(reportFoundCard, 1, 0);

        var categoryPills = new HorizontalStackLayout { Spacing = 8 };
        foreach (var cat in new[] { "All", "Electronics", "Clothing", "Documents" })
        {
            var isSel = vm.SelectedCategory == cat;
            var pill = new Button
            {
                Text = cat,
                FontSize = 12,
                FontFamily = isSel ? "OpenSansSemibold" : "OpenSansRegular",
                TextColor = isSel ? Colors.White : Color.FromArgb(FigmaTextDark),
                BackgroundColor = isSel ? Color.FromArgb(FigmaBlue) : Colors.White,
                BorderColor = isSel ? Color.FromArgb(FigmaBlue) : Color.FromArgb(FigmaBorder),
                BorderWidth = 1,
                CornerRadius = 16,
                HeightRequest = 32,
                Padding = new Thickness(14, 0)
            };
            var currentCat = cat;
            pill.Clicked += (_, _) => vm.SelectCategoryCommand.Execute(currentCat);
            categoryPills.Children.Add(pill);
        }

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                greetingRow,
                searchBox,
                actionCards,

                new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                    Children =
                    {
                        Text("Recent Reports", 16, true, FigmaTextDark),
                        new Button
                        {
                            Text = "See all",
                            FontSize = 12,
                            TextColor = Color.FromArgb(FigmaBlue),
                            BackgroundColor = Colors.Transparent,
                            Padding = 0,
                            Command = new Command(() => vm.SelectFilterCommand.Execute("All"))
                        }
                    }
                },
                categoryPills
            }
        };

        foreach (var item in vm.Items)
        {
            var cur = item;
            var thumbColor = cur.Title.Contains("Wallet") ? "#222222" : (cur.Title.Contains("Jacket") ? "#2B4764" : "#1C1C1E");
            var thumbIcon = cur.Title.Contains("Wallet") ? "👛" : (cur.Title.Contains("Jacket") ? "🧥" : "🎧");

            var thumbnail = new Border
            {
                WidthRequest = 48,
                HeightRequest = 48,
                BackgroundColor = Color.FromArgb(thumbColor),
                StrokeShape = new RoundRectangle { CornerRadius = 10 },
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Content = Text(thumbIcon, 20, false, "#FFFFFF")
            };

            var itemCard = new Border
            {
                BackgroundColor = Colors.White,
                Stroke = Color.FromArgb(FigmaBorder),
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 12 },
                Padding = 12,
                Content = new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(52), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                    ColumnSpacing = 10,
                    Children =
                    {
                        thumbnail,
                        new VerticalStackLayout
                        {
                            Spacing = 3,
                            Children =
                            {
                                Text(cur.Title, 14, true, FigmaTextDark),
                                Text($"📍 {cur.Location}", 11, false, FigmaTextMuted),
                                new HorizontalStackLayout
                                {
                                    Spacing = 8,
                                    Children =
                                    {
                                        Text($"📅 {cur.DateDisplay}", 11, false, FigmaTextMuted),
                                        Text($"🏷 {cur.Category}", 11, false, FigmaTextMuted)
                                    }
                                }
                            }
                        },
                        StatusBadge(cur.TypeDisplay, cur.StatusColor, cur.StatusBgColor)
                    }
                }
            };

            var tap = new TapGestureRecognizer();
            tap.Tapped += (_, _) => vm.SelectItemCommand.Execute(cur);
            itemCard.GestureRecognizers.Add(tap);
            body.Children.Add(itemCard);
        }

        var matchesHeader = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            Margin = new Thickness(0, 6, 0, 0),
            Children =
            {
                Text("Possible Matches", 16, true, FigmaTextDark),
                new Button
                {
                    Text = "See all",
                    FontSize = 12,
                    TextColor = Color.FromArgb(FigmaBlue),
                    BackgroundColor = Colors.Transparent,
                    Padding = 0,
                    Command = vm.ViewMatchesCommand
                }
            }
        };
        body.Children.Add(matchesHeader);

        var matchScoreBox = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaBlue),
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            WidthRequest = 48,
            HeightRequest = 48,
            Padding = 2,
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                VerticalOptions = LayoutOptions.Center,
                Spacing = 0,
                Children =
                {
                    Text("3/4", 14, true, "#FFFFFF"),
                    Text("criteria", 9, false, "#DDEBFD")
                }
            }
        };

        var matchChevronBtn = new Border
        {
            WidthRequest = 32,
            HeightRequest = 32,
            BackgroundColor = Color.FromArgb(FigmaBlue),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            Content = Text(">", 14, true, "#FFFFFF")
        };

        var possibleMatchCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(52), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                ColumnSpacing = 10,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    matchScoreBox,
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            Text("Black wallet → Found Item", 13, true, FigmaTextDark),
                            Text("📍 Campus • Oct 5, 2026", 11, false, FigmaTextMuted),
                            Text("Category • description • place • date", 10, false, FigmaTextMuted)
                        }
                    },
                    matchChevronBtn
                }
            }
        };

        var tapMatch = new TapGestureRecognizer();
        tapMatch.Tapped += (_, _) => vm.ViewMatchesCommand.Execute(null);
        possibleMatchCard.GestureRecognizers.Add(tapMatch);
        body.Children.Add(possibleMatchCard);

        return Scroll(body);
    }

    // ==========================================
    // 05. REPORT AN ITEM (Page 5/10) - Exact Figma Layout
    // ==========================================
    private View BuildReport()
    {
        var vm = ViewModel.Report;

        var backBtn = new Button
        {
            Text = "←",
            FontSize = 22,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            HeightRequest = 32,
            Command = new Command(() => ViewModel.NavigateTo("Home"))
        };

        var titleRow = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(36), new ColumnDefinition(GridLength.Star), new ColumnDefinition(36) },
            Children =
            {
                backBtn,
                Text("Report an Item", 17, true, FigmaTextDark)
            }
        };

        var typeCapsule = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaLightBlue),
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 3,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 4,
                Children =
                {
                    new Button
                    {
                        Text = "🔍 Lost",
                        FontSize = 13,
                        FontFamily = vm.IsLost ? "OpenSansSemibold" : "OpenSansRegular",
                        TextColor = vm.IsLost ? Colors.White : Color.FromArgb(FigmaBlue),
                        BackgroundColor = vm.IsLost ? Color.FromArgb(FigmaBlue) : Colors.Transparent,
                        CornerRadius = 8,
                        HeightRequest = 38,
                        Command = new Command(() => { vm.SelectTypeCommand.Execute("Lost"); RenderScreen(); })
                    },
                    new Button
                    {
                        Text = "✓ Found",
                        FontSize = 13,
                        FontFamily = vm.IsFound ? "OpenSansSemibold" : "OpenSansRegular",
                        TextColor = vm.IsFound ? Colors.White : Color.FromArgb(FigmaBlue),
                        BackgroundColor = vm.IsFound ? Color.FromArgb(FigmaBlue) : Colors.Transparent,
                        CornerRadius = 8,
                        HeightRequest = 38,
                        Command = new Command(() => { vm.SelectTypeCommand.Execute("Found"); RenderScreen(); })
                    }
                }
            }
        };

        var nameInput = new Entry
        {
            Text = vm.ItemTitle,
            Placeholder = "e.g. Black leather wallet",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        nameInput.TextChanged += (_, e) => vm.ItemTitle = e.NewTextValue;

        var categoryPicker = new Picker
        {
            ItemsSource = vm.Categories,
            SelectedItem = vm.Category,
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        categoryPicker.SelectedIndexChanged += (_, _) =>
        {
            if (categoryPicker.SelectedItem is string cat)
                vm.Category = cat;
        };

        var descEditor = new Editor
        {
            Text = vm.Description,
            Placeholder = "Colour, brand, distinguishing marks...\nBe specific to improve matching.",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            PlaceholderColor = Color.FromArgb(FigmaTextMuted),
            BackgroundColor = Colors.Transparent,
            HeightRequest = 75
        };
        descEditor.TextChanged += (_, e) => vm.Description = e.NewTextValue;

        var placeInput = new Entry
        {
            Text = vm.Location,
            Placeholder = "e.g. PK 17",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        placeInput.TextChanged += (_, e) => vm.Location = e.NewTextValue;

        var datePicker = new DatePicker
        {
            Date = vm.Date,
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        datePicker.DateSelected += (_, e) => vm.Date = e.NewDate;

        var photoBox = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb("#BBD6FC"),
            StrokeThickness = 1.5,
            StrokeDashArray = new DoubleCollection { 4, 3 },
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = new Thickness(14, 16),
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 4,
                Children =
                {
                    Text("⬆", 20, true, FigmaBlue),
                    Text("Tap to add a photo", 12, true, FigmaBlue),
                    Text("JPG, PNG • max 5 MB", 10, false, FigmaTextMuted)
                }
            }
        };

        var infoPill = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaLightBlue),
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = new Thickness(10, 6),
            Content = Text("ℹ Category • description • place • date. No AI.", 11, false, FigmaBlue)
        };

        var body = new VerticalStackLayout
        {
            Spacing = 13,
            Children =
            {
                titleRow,
                typeCapsule,

                FigmaFieldLabel("Item name *"),
                FigmaInputBox("🏷", nameInput),

                FigmaFieldLabel("Category *"),
                FigmaInputBoxWithSuffix("📁", categoryPicker, Text("⌄", 14, false, FigmaTextMuted)),

                FigmaFieldLabel("Description *"),
                new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        new Border
                        {
                            BackgroundColor = Colors.White,
                            Stroke = Color.FromArgb(FigmaBorder),
                            StrokeThickness = 1,
                            StrokeShape = new RoundRectangle { CornerRadius = 10 },
                            Padding = 10,
                            Content = descEditor
                        },
                        Text("Used for matching - no AI involved.", 11, false, FigmaTextMuted)
                    }
                },

                FigmaFieldLabel("Place *"),
                FigmaInputBox("📍", placeInput),

                FigmaFieldLabel("Date *"),
                FigmaInputBoxWithSuffix("📅", datePicker, Text("⌄", 14, false, FigmaTextMuted)),

                FigmaFieldLabel("Photo (optional)"),
                photoBox,

                infoPill,

                FigmaPrimaryButton("Submit Report", () => vm.SubmitCommand.Execute(null))
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 06. ITEM DETAILS SCREEN (Page 6/10) - Exact Figma Layout
    // ==========================================
    private View BuildDetails()
    {
        var vm = ViewModel.ItemDetails;
        var item = vm.Item ?? new Item
        {
            Title = "Black wallet",
            Category = "Accessories",
            Location = "Campus",
            Date = new DateTime(2026, 10, 4),
            Type = ItemType.Lost,
            Description = "Black leather wallet with a small silver logo. Contains an ID card and two bank cards.",
            ReporterName = "Arielle T."
        };

        var backBtn = new Button
        {
            Text = "←",
            FontSize = 22,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            HeightRequest = 32,
            Command = vm.GoBackCommand
        };

        var headerRow = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(36), new ColumnDefinition(GridLength.Star), new ColumnDefinition(36) },
            Children =
            {
                backBtn,
                Text("Item Details", 16, true, FigmaTextDark),
                new Button
                {
                    Text = "⬆",
                    FontSize = 16,
                    TextColor = Color.FromArgb(FigmaTextDark),
                    BackgroundColor = Colors.Transparent,
                    Padding = 0
                }
            }
        };

        var photoContainer = new Border
        {
            BackgroundColor = Color.FromArgb("#F2F4F8"),
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 14 },
            HeightRequest = 180,
            Content = new Grid
            {
                Children =
                {
                    new Image
                    {
                        Source = "black_wallet.jpg",
                        Aspect = Aspect.AspectFit,
                        HeightRequest = 140,
                        HorizontalOptions = LayoutOptions.Center,
                        VerticalOptions = LayoutOptions.Center
                    },
                    new Border
                    {
                        BackgroundColor = Color.FromArgb(FigmaRed),
                        StrokeShape = new RoundRectangle { CornerRadius = 12 },
                        Padding = new Thickness(8, 3),
                        HorizontalOptions = LayoutOptions.Start,
                        VerticalOptions = LayoutOptions.Start,
                        Margin = new Thickness(12),
                        Content = Text("🔴 LOST", 10, true, "#FFFFFF")
                    },
                    new Border
                    {
                        WidthRequest = 32,
                        HeightRequest = 32,
                        BackgroundColor = Colors.White,
                        Stroke = Color.FromArgb(FigmaBorder),
                        StrokeThickness = 1,
                        StrokeShape = new RoundRectangle { CornerRadius = 16 },
                        HorizontalOptions = LayoutOptions.End,
                        VerticalOptions = LayoutOptions.Start,
                        Margin = new Thickness(12),
                        Content = Text("🔖", 14, false, FigmaTextDark)
                    }
                }
            }
        };

        var infoCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 14,
            Content = new VerticalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                        Children =
                        {
                            new VerticalStackLayout
                            {
                                Spacing = 2,
                                Children =
                                {
                                    Text(item.Title, 18, true, FigmaTextDark),
                                    Text("Reported lost • 2 days ago", 11, false, FigmaTextMuted)
                                }
                            },
                            new Border
                            {
                                BackgroundColor = Color.FromArgb(FigmaLightBlue),
                                StrokeShape = new RoundRectangle { CornerRadius = 12 },
                                Padding = new Thickness(10, 4),
                                Content = Text(item.Category, 11, true, FigmaBlue)
                            }
                        }
                    },
                    new BoxView { HeightRequest = 1, Color = Color.FromArgb(FigmaBorder) },

                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            Text("📍 LOCATION", 10, true, FigmaTextMuted),
                            Text(item.Location, 13, true, FigmaTextDark)
                        }
                    },

                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            Text("📅 DATE LOST", 10, true, FigmaTextMuted),
                            Text("October 4, 2026 • 10:30 AM", 13, true, FigmaTextDark)
                        }
                    },

                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            Text("DESCRIPTION", 10, true, FigmaTextMuted),
                            Text(item.Description, 12, false, FigmaTextDark)
                        }
                    }
                }
            }
        };

        var reporterCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(44), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                ColumnSpacing = 10,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 42,
                        HeightRequest = 42,
                        Stroke = Color.FromArgb(FigmaBorder),
                        StrokeThickness = 1,
                        StrokeShape = new RoundRectangle { CornerRadius = 21 },
                        Content = new Image
                        {
                            Source = "arielle_avatar.jpg",
                            Aspect = Aspect.AspectFill,
                            WidthRequest = 42,
                            HeightRequest = 42
                        }
                    },
                    new VerticalStackLayout
                    {
                        VerticalOptions = LayoutOptions.Center,
                        Spacing = 1,
                        Children =
                        {
                            Text("REPORTED BY", 9, true, FigmaTextMuted),
                            Text(item.ReporterName, 13, true, FigmaTextDark),
                            Text("• Contact through SaveIt", 10, false, FigmaBlue)
                        }
                    },
                    new Border
                    {
                        BackgroundColor = Color.FromArgb(FigmaLightBlue),
                        StrokeShape = new RoundRectangle { CornerRadius = 8 },
                        Padding = new Thickness(10, 4),
                        VerticalOptions = LayoutOptions.Center,
                        Content = Text("Profile", 11, true, FigmaBlue)
                    }
                }
            }
        };

        var matchesBanner = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 12,
            Content = new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                        Children =
                        {
                            Text("View possible matches", 13, true, FigmaTextDark),
                            Text("2 found", 12, true, FigmaBlue)
                        }
                    },
                    Text("Category, description, place and date. No AI.", 10, false, FigmaTextMuted)
                }
            }
        };
        var tapMatchBanner = new TapGestureRecognizer();
        tapMatchBanner.Tapped += (_, _) => vm.ViewMatchesCommand.Execute(null);
        matchesBanner.GestureRecognizers.Add(tapMatchBanner);

        var actionsGrid = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 10,
            Children =
            {
                FigmaOutlinedButton("♡  View matches", () => vm.ViewMatchesCommand.Execute(null)),
                FigmaPrimaryButton("📞  Contact securely", () => vm.ContactSecurelyCommand.Execute(null))
            }
        };

        var body = new VerticalStackLayout
        {
            Spacing = 12,
            Children =
            {
                headerRow,
                photoContainer,
                infoCard,
                reporterCard,
                matchesBanner,
                actionsGrid
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 07. POSSIBLE MATCHES SCREEN (Page 7/10) - Exact Figma Layout
    // ==========================================
    private View BuildMatches()
    {
        var vm = ViewModel.Matches;

        var backBtn = new Button
        {
            Text = "←",
            FontSize = 22,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            HeightRequest = 32,
            Command = new Command(() => ViewModel.NavigateTo("Home"))
        };

        var headerRow = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(36), new ColumnDefinition(GridLength.Star), new ColumnDefinition(36) },
            Children =
            {
                backBtn,
                new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 1,
                    Children =
                    {
                        Text("Possible Matches", 16, true, FigmaTextDark),
                        Text("For your report · Black wallet", 11, false, FigmaTextMuted)
                    }
                },
                Text("☵", 16, false, FigmaBlue)
            }
        };

        var yourReportCard = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaLightBlue),
            Stroke = Color.FromArgb("#CFE1FB"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 12,
            Content = new VerticalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
                        Children =
                        {
                            new Border
                            {
                                BackgroundColor = Color.FromArgb(FigmaBlue),
                                StrokeShape = new RoundRectangle { CornerRadius = 6 },
                                Padding = new Thickness(6, 2),
                                Content = Text("YOUR REPORT", 9, true, "#FFFFFF")
                            },
                            new Label
                            {
                                Text = "Lost · Oct 4, 2026",
                                FontSize = 10,
                                TextColor = Color.FromArgb(FigmaTextMuted),
                                HorizontalOptions = LayoutOptions.End
                            }
                        }
                    },
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(46), new ColumnDefinition(GridLength.Star) },
                        ColumnSpacing = 10,
                        Children =
                        {
                            new Border
                            {
                                WidthRequest = 42,
                                HeightRequest = 42,
                                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                                StrokeThickness = 0,
                                Content = new Image
                                {
                                    Source = "black_wallet.jpg",
                                    Aspect = Aspect.AspectFill,
                                    WidthRequest = 42,
                                    HeightRequest = 42
                                }
                            },
                            new VerticalStackLayout
                            {
                                Spacing = 2,
                                Children =
                                {
                                    Text("Black wallet", 13, true, FigmaTextDark),
                                    Text("📍 Campus", 11, false, FigmaTextMuted),
                                    Text("🏷 Accessories • Oct 4, 2026", 10, false, FigmaTextMuted)
                                }
                            }
                        }
                    }
                }
            }
        };

        var howItWorksCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 10,
            Content = new HorizontalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    Text("ℹ", 14, true, FigmaBlue),
                    new VerticalStackLayout
                    {
                        Spacing = 1,
                        Children =
                        {
                            Text("How matching works", 11, true, FigmaTextDark),
                            Text("Rules: category, description, place and date. No AI;\nownership must be confirmed.", 10, false, FigmaTextMuted)
                        }
                    }
                }
            }
        };

        var match1Card = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 12,
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                        Children =
                        {
                            Text("⭐ Strong match", 11, true, FigmaBlue),
                            new Border
                            {
                                BackgroundColor = Color.FromArgb(FigmaBlue),
                                StrokeShape = new RoundRectangle { CornerRadius = 6 },
                                Padding = new Thickness(8, 2),
                                Content = Text("Score 3/4", 10, true, "#FFFFFF")
                            }
                        }
                    },
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(46), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                        ColumnSpacing = 10,
                        Children =
                        {
                            new Border
                            {
                                WidthRequest = 42,
                                HeightRequest = 42,
                                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                                StrokeThickness = 0,
                                Content = new Image
                                {
                                    Source = "black_wallet.jpg",
                                    Aspect = Aspect.AspectFill,
                                    WidthRequest = 42,
                                    HeightRequest = 42
                                }
                            },
                            new VerticalStackLayout
                            {
                                Spacing = 2,
                                Children =
                                {
                                    Text("Black leather wallet", 13, true, FigmaTextDark),
                                    Text("📍 Campus ✓ same area", 11, false, FigmaBlue),
                                    Text("📅 Oct 5, 2026 ✓ 1 day later", 11, false, FigmaBlue)
                                }
                            },
                            StatusBadge("Found", FigmaGreen, FigmaGreenBg)
                        }
                    },
                    new HorizontalStackLayout
                    {
                        Spacing = 6,
                        Children =
                        {
                            CriteriaPill("Category", true),
                            CriteriaPill("Location", true),
                            CriteriaPill("Date", true),
                            CriteriaPill("Desc", false)
                        }
                    },
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
                        ColumnSpacing = 8,
                        Children =
                        {
                            FigmaPrimaryButton("Contact Reporter", () => ViewModel.NavigateTo("Conversation")),
                            FigmaOutlinedButton("View Details", () => ViewModel.NavigateTo("Details"))
                        }
                    }
                }
            }
        };

        var match2Card = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = 12,
            Content = new VerticalStackLayout
            {
                Spacing = 8,
                Children =
                {
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                        Children =
                        {
                            Text("⚠ Partial match", 11, true, Orange),
                            new Border
                            {
                                BackgroundColor = Color.FromArgb(Orange),
                                StrokeShape = new RoundRectangle { CornerRadius = 6 },
                                Padding = new Thickness(8, 2),
                                Content = Text("Score 2/4", 10, true, "#FFFFFF")
                            }
                        }
                    },
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(46), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                        ColumnSpacing = 10,
                        Children =
                        {
                            new Border
                            {
                                WidthRequest = 42,
                                HeightRequest = 42,
                                StrokeShape = new RoundRectangle { CornerRadius = 8 },
                                StrokeThickness = 0,
                                Content = new Image
                                {
                                    Source = "brown_wallet.jpg",
                                    Aspect = Aspect.AspectFill,
                                    WidthRequest = 42,
                                    HeightRequest = 42
                                }
                            },
                            new VerticalStackLayout
                            {
                                Spacing = 2,
                                Children =
                                {
                                    Text("Dark Brown Wallet", 13, true, FigmaTextDark),
                                    Text("📍 Bonamoussadi ~ nearby", 11, false, Orange),
                                    Text("📅 Oct 3, 2026 1 day earlier", 11, false, FigmaTextMuted)
                                }
                            },
                            StatusBadge("Found", FigmaGreen, FigmaGreenBg)
                        }
                    },
                    new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
                        ColumnSpacing = 8,
                        Children =
                        {
                            FigmaPrimaryButton("Contact Reporter", () => ViewModel.NavigateTo("Conversation")),
                            FigmaOutlinedButton("View Details", () => ViewModel.NavigateTo("Details"))
                        }
                    }
                }
            }
        };

        var body = new VerticalStackLayout
        {
            Spacing = 12,
            Children =
            {
                headerRow,
                yourReportCard,
                howItWorksCard,
                new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                    Children =
                    {
                        Text("2 possible matches", 13, true, FigmaTextDark),
                        Text("Score: 4 criteria", 11, false, FigmaBlue)
                    }
                },
                match1Card,
                match2Card
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 08. NOTIFICATIONS SCREEN (Page 8/10) - Exact Figma Layout
    // ==========================================
    private View BuildNotifications()
    {
        var vm = ViewModel.Notifications;

        var header = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Auto) },
            ColumnSpacing = 12,
            Children =
            {
                Text("Notifications", 22, true, FigmaTextDark),
                new Button { Text = "✓", FontSize = 16, TextColor = Color.FromArgb(FigmaBlue), BackgroundColor = Colors.Transparent, Padding = 0 },
                new Button { Text = "⚙", FontSize = 16, TextColor = Color.FromArgb(FigmaTextDark), BackgroundColor = Colors.Transparent, Padding = 0 }
            }
        };

        var tabsRow = new HorizontalStackLayout
        {
            Spacing = 14,
            Children =
            {
                Text("All", 13, true, FigmaBlue),
                Text("Matches", 13, false, FigmaTextMuted),
                Text("Messages", 13, false, FigmaTextMuted),
                Text("Reports", 13, false, FigmaTextMuted)
            }
        };

        var notif1 = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(42), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 10,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        BackgroundColor = Color.FromArgb(FigmaBlue),
                        StrokeShape = new RoundRectangle { CornerRadius = 19 },
                        Content = Text("♡", 16, true, "#FFFFFF")
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 4,
                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Auto) },
                                ColumnSpacing = 4,
                                Children =
                                {
                                    Text("New Match Found!", 13, true, FigmaTextDark),
                                    Text("2m ago", 10, false, FigmaTextMuted),
                                    new Ellipse { WidthRequest = 6, HeightRequest = 6, Fill = Color.FromArgb(FigmaBlue), VerticalOptions = LayoutOptions.Center }
                                }
                            },
                            Text("A found wallet may match your Campus report. Compare the details.", 11, false, FigmaTextMuted),
                            new HorizontalStackLayout
                            {
                                Spacing = 8,
                                Margin = new Thickness(0, 4, 0, 0),
                                Children =
                                {
                                    new Button
                                    {
                                        Text = "View Match",
                                        FontSize = 10,
                                        FontFamily = "OpenSansSemibold",
                                        TextColor = Colors.White,
                                        BackgroundColor = Color.FromArgb(FigmaBlue),
                                        CornerRadius = 12,
                                        HeightRequest = 26,
                                        Padding = new Thickness(10, 0),
                                        Command = new Command(() => ViewModel.NavigateTo("Matches"))
                                    },
                                    new Button
                                    {
                                        Text = "Dismiss",
                                        FontSize = 10,
                                        TextColor = Color.FromArgb(FigmaTextMuted),
                                        BackgroundColor = Colors.White,
                                        BorderColor = Color.FromArgb(FigmaBorder),
                                        BorderWidth = 1,
                                        CornerRadius = 12,
                                        HeightRequest = 26,
                                        Padding = new Thickness(10, 0)
                                    }
                                }
                            }
                        }
                    }
                }
            }
        };

        var notif2 = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(42), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 10,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        BackgroundColor = Color.FromArgb("#4A90E2"),
                        StrokeShape = new RoundRectangle { CornerRadius = 19 },
                        Content = Text("JD", 13, true, "#FFFFFF")
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 4,
                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Auto) },
                                ColumnSpacing = 4,
                                Children =
                                {
                                    Text("Jean D.", 13, true, FigmaTextDark),
                                    Text("15m ago", 10, false, FigmaTextMuted),
                                    new Ellipse { WidthRequest = 6, HeightRequest = 6, Fill = Color.FromArgb(FigmaBlue), VerticalOptions = LayoutOptions.Center }
                                }
                            },
                            Text("Hi! I found a black wallet near campus. Can you describe it?", 11, false, FigmaTextMuted),
                            new Border
                            {
                                BackgroundColor = Color.FromArgb(FigmaLightBlue),
                                StrokeShape = new RoundRectangle { CornerRadius = 6 },
                                Padding = new Thickness(6, 2),
                                HorizontalOptions = LayoutOptions.Start,
                                Content = Text("1 new message", 10, true, FigmaBlue)
                            }
                        }
                    }
                }
            }
        };
        var tapNotif2 = new TapGestureRecognizer();
        tapNotif2.Tapped += (_, _) => ViewModel.NavigateTo("Conversation");
        notif2.GestureRecognizers.Add(tapNotif2);

        var notif3 = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(42), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 10,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        BackgroundColor = Color.FromArgb(FigmaLightBlue),
                        StrokeShape = new RoundRectangle { CornerRadius = 19 },
                        Content = Text("📄", 15, false, FigmaBlue)
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                                Children =
                                {
                                    Text("Report Updated", 13, true, FigmaTextDark),
                                    Text("1h ago", 10, false, FigmaTextMuted)
                                }
                            },
                            Text("Your report \"Black wallet — Campus\" is active.", 11, false, FigmaTextMuted)
                        }
                    }
                }
            }
        };

        var notif4 = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(42), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 10,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        BackgroundColor = Colors.White,
                        Stroke = Color.FromArgb(FigmaBorder),
                        StrokeThickness = 1,
                        StrokeShape = new RoundRectangle { CornerRadius = 19 },
                        Content = Text("♡", 15, false, FigmaBlue)
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                                Children =
                                {
                                    Text("Possible Match", 13, true, FigmaTextDark),
                                    Text("Yesterday", 10, false, FigmaTextMuted)
                                }
                            },
                            Text("A found item shares 2 of 4 criteria with your keys report.", 11, false, FigmaTextMuted)
                        }
                    }
                }
            }
        };

        var notif5 = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(42), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 10,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        BackgroundColor = Color.FromArgb("#DDEAF9"),
                        StrokeShape = new RoundRectangle { CornerRadius = 19 },
                        Content = Text("TK", 13, true, FigmaBlue)
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                                Children =
                                {
                                    Text("Thomas K.", 13, true, FigmaTextDark),
                                    Text("Yesterday", 10, false, FigmaTextMuted)
                                }
                            },
                            Text("I left it at the lost & found desk of Galeries Bonamoussadi, you can pick it up anytime.", 11, false, FigmaTextMuted)
                        }
                    }
                }
            }
        };

        var notif6 = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 12,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(42), new ColumnDefinition(GridLength.Star) },
                ColumnSpacing = 10,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        BackgroundColor = Color.FromArgb(FigmaLightBlue),
                        StrokeShape = new RoundRectangle { CornerRadius = 19 },
                        Content = Text("🔔", 15, false, FigmaBlue)
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                                Children =
                                {
                                    Text("Reminder", 13, true, FigmaTextDark),
                                    Text("Yesterday", 10, false, FigmaTextMuted)
                                }
                            },
                            Text("Your report \"Silver Keys, Opera\" hasn't been updated in 7 days. Review it to keep it active.", 11, false, FigmaTextMuted)
                        }
                    }
                }
            }
        };

        var body = new VerticalStackLayout
        {
            Spacing = 12,
            Children =
            {
                header,
                tabsRow,
                Text("TODAY", 11, true, FigmaTextMuted),
                notif1,
                notif2,
                notif3,
                Text("YESTERDAY", 11, true, FigmaTextMuted),
                notif4,
                notif5,
                notif6
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 09. SECURE CONVERSATION (Page 9/10) - Exact Figma Layout
    // ==========================================
    private View BuildConversation()
    {
        var vm = ViewModel.Conversation;

        var backBtn = new Button
        {
            Text = "←",
            FontSize = 22,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            HeightRequest = 32,
            Command = vm.GoBackCommand
        };

        var avatar = new Border
        {
            WidthRequest = 32,
            HeightRequest = 32,
            BackgroundColor = Color.FromArgb(FigmaLightBlue),
            StrokeShape = new RoundRectangle { CornerRadius = 16 },
            Content = Text("M", 12, true, FigmaBlue)
        };

        var topBar = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(32), new ColumnDefinition(38), new ColumnDefinition(GridLength.Star), new ColumnDefinition(32) },
            ColumnSpacing = 6,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                backBtn,
                avatar,
                new VerticalStackLayout
                {
                    Spacing = 0,
                    VerticalOptions = LayoutOptions.Center,
                    Children =
                    {
                        Text("Jean D.", 14, true, FigmaTextDark),
                        Text("Active now", 10, false, FigmaGreen)
                    }
                },
                new Button { Text = "ℹ", FontSize = 16, TextColor = Color.FromArgb(FigmaBlue), BackgroundColor = Colors.Transparent, Padding = 0 }
            }
        };

        var itemContextCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = 8,
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(40), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                ColumnSpacing = 8,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 38,
                        HeightRequest = 38,
                        StrokeShape = new RoundRectangle { CornerRadius = 6 },
                        StrokeThickness = 0,
                        Content = new Image
                        {
                            Source = "black_wallet.jpg",
                            Aspect = Aspect.AspectFill,
                            WidthRequest = 38,
                            HeightRequest = 38
                        }
                    },
                    new VerticalStackLayout
                    {
                        Spacing = 1,
                        VerticalOptions = LayoutOptions.Center,
                        Children =
                        {
                            Text("Lost Item · Reference #4812", 9, true, FigmaBlue),
                            Text("Black leather wallet", 12, true, FigmaTextDark),
                            Text("Found near Campus · Oct 5, 2026", 10, false, FigmaTextMuted)
                        }
                    },
                    new Button
                    {
                        Text = "View",
                        FontSize = 10,
                        FontFamily = "OpenSansSemibold",
                        TextColor = Colors.White,
                        BackgroundColor = Color.FromArgb(FigmaBlue),
                        CornerRadius = 6,
                        HeightRequest = 26,
                        Padding = new Thickness(10, 0),
                        Command = new Command(() => ViewModel.NavigateTo("Details"))
                    }
                }
            }
        };

        var safetyBanner = new Border
        {
            BackgroundColor = Color.FromArgb("#FFF9E6"),
            Stroke = Color.FromArgb("#F6E2A3"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = 8,
            Content = new HorizontalStackLayout
            {
                Spacing = 6,
                Children =
                {
                    Text("🛡", 12, false, Orange),
                    Text("Meet in a public place. Use in-app contact only. Never\nshare personal details or payment info outside SaveIt.", 9, false, Color.FromArgb("#7C5300"))
                }
            }
        };

        var messagesList = new VerticalStackLayout
        {
            Spacing = 8,
            Children =
            {
                Text("Today", 10, true, FigmaTextMuted),

                // Msg 1 (Received)
                new HorizontalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        new Border { WidthRequest = 20, HeightRequest = 20, BackgroundColor = Color.FromArgb(FigmaLightBlue), StrokeShape = new RoundRectangle { CornerRadius = 10 }, Content = Text("M", 9, true, FigmaBlue) },
                        new VerticalStackLayout
                        {
                            Spacing = 2,
                            Children =
                            {
                                new Border
                                {
                                    BackgroundColor = Colors.White,
                                    Stroke = Color.FromArgb(FigmaBorder),
                                    StrokeThickness = 1,
                                    StrokeShape = new RoundRectangle { CornerRadius = 12 },
                                    Padding = new Thickness(10, 8),
                                    MaximumWidthRequest = 240,
                                    Content = Text("Hello! I found a black wallet near campus. Is yours black leather?", 12, false, FigmaTextDark)
                                },
                                Text("10:12", 9, false, FigmaTextMuted)
                            }
                        }
                    }
                },

                // Msg 2 (Sent)
                new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.End,
                    Spacing = 2,
                    Children =
                    {
                        new Border
                        {
                            BackgroundColor = Color.FromArgb(FigmaBlue),
                            StrokeShape = new RoundRectangle { CornerRadius = 12 },
                            Padding = new Thickness(10, 8),
                            MaximumWidthRequest = 240,
                            Content = Text("Yes, with a small silver logo. Thank you!", 12, false, "#FFFFFF")
                        },
                        Text("10:15 ✓✓", 9, false, FigmaTextMuted)
                    }
                },

                // Msg 3 (Received)
                new HorizontalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        new Border { WidthRequest = 20, HeightRequest = 20, BackgroundColor = Color.FromArgb(FigmaLightBlue), StrokeShape = new RoundRectangle { CornerRadius = 10 }, Content = Text("M", 9, true, FigmaBlue) },
                        new VerticalStackLayout
                        {
                            Spacing = 2,
                            Children =
                            {
                                new Border
                                {
                                    BackgroundColor = Colors.White,
                                    Stroke = Color.FromArgb(FigmaBorder),
                                    StrokeThickness = 1,
                                    StrokeShape = new RoundRectangle { CornerRadius = 12 },
                                    Padding = new Thickness(10, 8),
                                    MaximumWidthRequest = 240,
                                    Content = Text("That matches. Shall we meet at the campus gate at 5 PM?", 12, false, FigmaTextDark)
                                },
                                Text("10:18", 9, false, FigmaTextMuted)
                            }
                        }
                    }
                },

                // Msg 4 (Sent - in progress demo state)
                new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.End,
                    Spacing = 2,
                    Children =
                    {
                        new Border
                        {
                            BackgroundColor = Color.FromArgb(FigmaBlue),
                            StrokeShape = new RoundRectangle { CornerRadius = 12 },
                            Padding = new Thickness(10, 8),
                            MaximumWidthRequest = 240,
                            Content = Text("Perfect, 5 PM works for me!", 12, false, "#FFFFFF")
                        },
                        Text("Sending... ⏱", 9, false, FigmaTextMuted)
                    }
                },

                // Msg 5 (Sent - error demo state)
                new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.End,
                    Spacing = 2,
                    Children =
                    {
                        new Border
                        {
                            BackgroundColor = Color.FromArgb(FigmaRedBg),
                            Stroke = Color.FromArgb(FigmaRed),
                            StrokeThickness = 1,
                            StrokeShape = new RoundRectangle { CornerRadius = 12 },
                            Padding = new Thickness(10, 8),
                            MaximumWidthRequest = 240,
                            Content = Text("See you then!", 12, false, FigmaRed)
                        },
                        Text("Not delivered · Retry", 9, false, FigmaRed)
                    }
                }
            }
        };

        var messageInput = new Entry
        {
            Placeholder = "Write a message...",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            PlaceholderColor = Color.FromArgb(FigmaTextMuted),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        var sendBtn = new Border
        {
            WidthRequest = 34,
            HeightRequest = 34,
            BackgroundColor = Color.FromArgb(FigmaBlue),
            StrokeShape = new RoundRectangle { CornerRadius = 17 },
            Content = Text("➤", 12, true, "#FFFFFF")
        };
        var tapSend = new TapGestureRecognizer();
        tapSend.Tapped += (_, _) =>
        {
            if (!string.IsNullOrWhiteSpace(messageInput.Text))
            {
                vm.MessageInput = messageInput.Text;
                vm.SendMessageCommand.Execute(null);
                messageInput.Text = string.Empty;
            }
        };
        sendBtn.GestureRecognizers.Add(tapSend);

        var inputBar = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 20 },
            Padding = new Thickness(12, 4),
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                ColumnSpacing = 8,
                VerticalOptions = LayoutOptions.Center,
                Children =
                {
                    Text("📎", 16, false, FigmaTextMuted),
                    messageInput,
                    sendBtn
                }
            }
        };

        var body = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            },
            RowSpacing = 8,
            Padding = new Thickness(16, 10, 16, 12)
        };

        body.Add(topBar, 0, 0);
        body.Add(itemContextCard, 0, 1);
        body.Add(safetyBanner, 0, 2);
        body.Add(new ScrollView { Content = messagesList }, 0, 3);
        body.Add(inputBar, 0, 4);

        return body;
    }

    // ==========================================
    // 10. PROFILE SCREEN (Page 10/10) - Exact Figma Layout
    // ==========================================
    private View BuildProfile()
    {
        var vm = ViewModel.Profile;

        var blueBanner = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaBlue),
            StrokeThickness = 0,
            Padding = new Thickness(16, 24, 16, 20),
            Content = new VerticalStackLayout
            {
                HorizontalOptions = LayoutOptions.Center,
                Spacing = 6,
                Children =
                {
                    new Border
                    {
                        WidthRequest = 66,
                        HeightRequest = 66,
                        Stroke = Colors.White,
                        StrokeThickness = 2,
                        StrokeShape = new RoundRectangle { CornerRadius = 33 },
                        Content = new Image
                        {
                            Source = "arielle_avatar.jpg",
                            Aspect = Aspect.AspectFill,
                            WidthRequest = 66,
                            HeightRequest = 66
                        }
                    },
                    Text("Arielle T.", 18, true, "#FFFFFF"),
                    Text("arielle@gmail.com", 11, false, Color.FromArgb(FigmaLightBlue)),
                    new Border
                    {
                        BackgroundColor = Colors.Transparent,
                        Stroke = Colors.White,
                        StrokeThickness = 1,
                        StrokeShape = new RoundRectangle { CornerRadius = 14 },
                        Padding = new Thickness(12, 3),
                        HorizontalOptions = LayoutOptions.Center,
                        Content = Text("< Edit Profile", 10, true, "#FFFFFF")
                    }
                }
            }
        };

        var countersCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(0, 12),
            Margin = new Thickness(16, -16, 16, 0),
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
                Children =
                {
                    new VerticalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Spacing = 1,
                        Children =
                        {
                            Text("2", 18, true, FigmaTextDark),
                            Text("Lost", 11, false, FigmaTextMuted)
                        }
                    },
                    new VerticalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Spacing = 1,
                        Children =
                        {
                            Text("1", 18, true, FigmaTextDark),
                            Text("Found", 11, false, FigmaTextMuted)
                        }
                    },
                    new VerticalStackLayout
                    {
                        HorizontalOptions = LayoutOptions.Center,
                        Spacing = 1,
                        Children =
                        {
                            Text("1", 18, true, FigmaTextDark),
                            Text("Reunited", 11, false, FigmaTextMuted)
                        }
                    }
                }
            }
        };

        var myDeclarationsCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 6),
            Content = new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    FigmaProfileRow("🎒 My Lost Items", "2", Color.FromArgb(FigmaLightBlue), FigmaBlue, () => vm.ViewLostItemsCommand.Execute(null)),
                    new BoxView { HeightRequest = 1, Color = Color.FromArgb(FigmaBorder) },
                    FigmaProfileRow("📦 My Found Items", "1", Color.FromArgb(FigmaGreenBg), FigmaGreen, () => vm.ViewFoundItemsCommand.Execute(null))
                }
            }
        };

        var matchesCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 6),
            Content = FigmaProfileRow("⚡ My Matches", "2", Color.FromArgb("#FFF2E6"), Orange, () => vm.ViewMatchesCommand.Execute(null))
        };

        var settingsCard = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 12 },
            Padding = new Thickness(12, 6),
            Content = new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    FigmaSettingsRow("⚙ Notifications", () => vm.ViewNotificationsCommand.Execute(null)),
                    new BoxView { HeightRequest = 1, Color = Color.FromArgb(FigmaBorder) },
                    FigmaSettingsRow("🔒 Privacy & Security", () => DisplayAlert("Privacy & Security", "All data is end-to-end secured. Your contact details remain confidential.", "OK")),
                    new BoxView { HeightRequest = 1, Color = Color.FromArgb(FigmaBorder) },
                    FigmaSettingsRow("💬 Help & Support", () => DisplayAlert("Help & Support", "SaveIt facilitates safe lost & found reporting with strict 4-criteria matching.", "OK"))
                }
            }
        };

        var logoutBtn = new Button
        {
            Text = "Log Out",
            FontSize = 13,
            FontFamily = "OpenSansSemibold",
            TextColor = Color.FromArgb(FigmaRed),
            BackgroundColor = Colors.White,
            BorderColor = Color.FromArgb(FigmaRed),
            BorderWidth = 1,
            CornerRadius = 10,
            HeightRequest = 44,
            Command = vm.LogoutCommand
        };

        var body = new VerticalStackLayout
        {
            Spacing = 12,
            Children =
            {
                blueBanner,
                countersCard,
                new VerticalStackLayout
                {
                    Padding = new Thickness(16, 8, 16, 24),
                    Spacing = 12,
                    Children =
                    {
                        Text("MY DECLARATIONS", 10, true, FigmaTextMuted),
                        myDeclarationsCard,

                        Text("MATCHES", 10, true, FigmaTextMuted),
                        matchesCard,

                        Text("SETTINGS & HELP", 10, true, FigmaTextMuted),
                        settingsCard,

                        logoutBtn
                    }
                }
            }
        };

        return new ScrollView { Content = body };
    }

    // ==========================================
    // UI Helpers strictly implementing Figma Design System
    // ==========================================
    private static Border BrandPinIcon(double size = 18) => new()
    {
        WidthRequest = size,
        HeightRequest = size,
        StrokeThickness = 0,
        Content = Text("⌖", size, true, FigmaBlue)
    };

    private static Label FigmaFieldLabel(string text) => new()
    {
        Text = text,
        FontSize = 12,
        FontFamily = "OpenSansSemibold",
        TextColor = Color.FromArgb(FigmaTextDark)
    };

    private static Border FigmaInputBox(string icon, View field) => new()
    {
        BackgroundColor = Colors.White,
        Stroke = Color.FromArgb(FigmaBorder),
        StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = 10 },
        Padding = new Thickness(12, 0),
        HeightRequest = 46,
        Content = new HorizontalStackLayout
        {
            Spacing = 10,
            Children = { Text(icon, 14, false, FigmaTextMuted), field }
        }
    };

    private static Border FigmaInputBoxWithSuffix(string icon, View field, View suffix) => new()
    {
        BackgroundColor = Colors.White,
        Stroke = Color.FromArgb(FigmaBorder),
        StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = 10 },
        Padding = new Thickness(12, 0),
        HeightRequest = 46,
        Content = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            ColumnSpacing = 10,
            Children = { Text(icon, 14, false, FigmaTextMuted), field, suffix }
        }
    };

    private static Border FigmaInputBoxWithError(string icon, View field) => new()
    {
        BackgroundColor = Colors.White,
        Stroke = Color.FromArgb(FigmaRed),
        StrokeThickness = 1.5,
        StrokeShape = new RoundRectangle { CornerRadius = 10 },
        Padding = new Thickness(12, 0),
        HeightRequest = 46,
        Content = new HorizontalStackLayout
        {
            Spacing = 10,
            Children = { Text(icon, 14, false, FigmaRed), field }
        }
    };

    private static Border FigmaInputBoxWithErrorWithSuffix(string icon, View field, View suffix) => new()
    {
        BackgroundColor = Colors.White,
        Stroke = Color.FromArgb(FigmaRed),
        StrokeThickness = 1.5,
        StrokeShape = new RoundRectangle { CornerRadius = 10 },
        Padding = new Thickness(12, 0),
        HeightRequest = 46,
        Content = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            ColumnSpacing = 10,
            Children = { Text(icon, 14, false, FigmaRed), field, suffix }
        }
    };

    private static Border FigmaInputBoxFocused(string icon, View field) => new()
    {
        BackgroundColor = Colors.White,
        Stroke = Color.FromArgb(FigmaBlue),
        StrokeThickness = 2,
        StrokeShape = new RoundRectangle { CornerRadius = 10 },
        Padding = new Thickness(12, 0),
        HeightRequest = 46,
        Content = new HorizontalStackLayout
        {
            Spacing = 10,
            Children = { Text(icon, 14, false, FigmaBlue), field }
        }
    };

    private static Button FigmaPrimaryButton(string text, Action action)
    {
        var btn = new Button
        {
            Text = text,
            FontSize = 14,
            FontFamily = "OpenSansSemibold",
            TextColor = Colors.White,
            BackgroundColor = Color.FromArgb(FigmaBlue),
            CornerRadius = 10,
            HeightRequest = 46
        };
        btn.Clicked += (_, _) => action();
        return btn;
    }

    private static Button FigmaButtonDisabled(string text, Action action)
    {
        var btn = new Button
        {
            Text = text,
            FontSize = 14,
            FontFamily = "OpenSansSemibold",
            TextColor = Color.FromArgb("#5D7CA6"),
            BackgroundColor = Color.FromArgb("#D5E4F9"),
            CornerRadius = 10,
            HeightRequest = 46
        };
        btn.Clicked += (_, _) => action();
        return btn;
    }

    private static Button FigmaOutlinedButton(string text, Action action)
    {
        var btn = new Button
        {
            Text = text,
            FontSize = 13,
            FontFamily = "OpenSansSemibold",
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.White,
            BorderColor = Color.FromArgb(FigmaBorder),
            BorderWidth = 1,
            CornerRadius = 10,
            HeightRequest = 46
        };
        btn.Clicked += (_, _) => action();
        return btn;
    }

    private static View FigmaProfileRow(string title, string countBadge, Color badgeBg, string badgeFg, Action action)
    {
        var row = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Auto) },
            ColumnSpacing = 8,
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                Text(title, 13, false, FigmaTextDark),
                new Border
                {
                    BackgroundColor = badgeBg,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Padding = new Thickness(8, 2),
                    VerticalOptions = LayoutOptions.Center,
                    Content = Text(countBadge, 10, true, badgeFg)
                },
                Text("›", 16, true, FigmaTextMuted)
            }
        };
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => action();
        row.GestureRecognizers.Add(tap);
        return row;
    }

    private static View FigmaSettingsRow(string title, Action action)
    {
        var row = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
            HeightRequest = 40,
            VerticalOptions = LayoutOptions.Center,
            Children =
            {
                Text(title, 13, false, FigmaTextDark),
                Text("›", 16, true, FigmaTextMuted)
            }
        };
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => action();
        row.GestureRecognizers.Add(tap);
        return row;
    }

    private static View DetailRow(string label, string value) => new Grid
    {
        ColumnDefinitions = { new ColumnDefinition(100), new ColumnDefinition(GridLength.Star) },
        Children =
        {
            Text(label, 12, true, FigmaTextMuted),
            Text(value, 13, false, FigmaTextDark)
        }
    };

    private static View CriteriaPill(string label, bool isMatched) => new Border
    {
        BackgroundColor = Color.FromArgb(isMatched ? FigmaGreenBg : "#F3F5FA"),
        Stroke = Color.FromArgb(isMatched ? FigmaGreen : FigmaBorder),
        StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
        Padding = new Thickness(6, 2),
        Content = Text($"{(isMatched ? "✓" : "-")} {label}", 10, isMatched, isMatched ? FigmaGreen : FigmaTextMuted)
    };

    private static Button FilterButton(string label, bool isSelected, Action action)
    {
        var btn = new Button
        {
            Text = label,
            FontSize = 12,
            FontFamily = isSelected ? "OpenSansSemibold" : "OpenSansRegular",
            TextColor = isSelected ? Colors.White : Color.FromArgb(FigmaTextDark),
            BackgroundColor = isSelected ? Color.FromArgb(FigmaBlue) : Colors.White,
            BorderColor = isSelected ? Color.FromArgb(FigmaBlue) : Color.FromArgb(FigmaBorder),
            BorderWidth = 1,
            CornerRadius = 16,
            HeightRequest = 32,
            Padding = new Thickness(12, 0)
        };
        btn.Clicked += (_, _) => action();
        return btn;
    }

    private static View StatCard(string title, string count, string color, Action action)
    {
        var card = Card(new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 2,
            Children =
            {
                Text(count, 18, true, color),
                Text(title, 11, false, FigmaTextMuted)
            }
        });
        var tap = new TapGestureRecognizer();
        tap.Tapped += (_, _) => action();
        card.GestureRecognizers.Add(tap);
        return card;
    }

    private static View SettingLink(string title, Action action)
    {
        var btn = new Button
        {
            Text = $"{title}  ›",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 0),
            Padding = new Thickness(4, 6)
        };
        btn.Clicked += (_, _) => action();
        return btn;
    }

    private static Border EmptyState(string message) => Card(
        new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 8,
            Padding = 20,
            Children =
            {
                Text("∅", 28, false, FigmaTextMuted),
                Text(message, 12, false, FigmaTextMuted)
            }
        });
}
