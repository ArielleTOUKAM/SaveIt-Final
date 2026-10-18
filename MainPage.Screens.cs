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

        // Top Header with Back button and Brand Mark
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

        // Form fields
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

                // Separator "or"
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

        // Inputs
        var nameInput = new Entry
        {
            Placeholder = "Full name",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        // Field 2: Email in Error State (matching Figma mockup!)
        var emailInput = new Entry
        {
            Text = "invalid@email",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        // Field 3: Phone number in Active Focused State (border blue 2px)
        var phoneInput = new Entry
        {
            Placeholder = "Phone number",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };

        // Field 4: Password with strength bar
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

        // Password Strength Bar (2 blue segments, 1 gray, + "Medium" text)
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

        // Field 5: Confirm Password in Error State (matching Figma mockup!)
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

                // Full name
                FigmaInputBox("👤", nameInput),

                // Email in error state
                new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        FigmaInputBoxWithError("✉", emailInput),
                        Text("Please enter a valid email address.", 11, false, FigmaRed)
                    }
                },

                // Phone number in focused state (blue outline)
                FigmaInputBoxFocused("📞", phoneInput),

                // Password with strength
                new VerticalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        FigmaInputBoxWithSuffix("🔒", passInput, eye1),
                        strengthBar
                    }
                },

                // Confirm password in error state
                new VerticalStackLayout
                {
                    Spacing = 4,
                    Children =
                    {
                        FigmaInputBoxWithErrorWithSuffix("🔒", confirmInput, eye2),
                        Text("Passwords do not match.", 11, false, FigmaRed)
                    }
                },

                // Checkbox: Terms & Privacy
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

                // Create Account button (light blue disabled state as in mockup)
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

        // Top Greeting & Notification Bell
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
                        // Red notification dot
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

        // Search Box with filter sliders icon
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
                    Text("☵", 15, false, FigmaBlue) // Filter sliders symbol
                }
            }
        };

        // Side-by-side Action Cards (Report Lost vs Report Found)
        var actionCards = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 10
        };

        // Left: Report Lost (Solid Blue)
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

        // Right: Report Found (White with blue border)
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

        // Category filter chips: All, Electronics, Clothing, Documents
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

                // Section 1: Recent Reports
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

        // Render Recent Report Cards (with thumbnails, location, date, category tag, and status badge)
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

        // Section 2: Possible Matches (matching Figma's exact 3/4 criteria card!)
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

        // Figma Match Card: 3/4 criteria badge box, Title, Subline, Chevron button
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

        // Top Navigation: Back arrow + Centered Title
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

        // Type toggle: 🔍 Lost vs ✓ Found
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

        // Inputs
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

        // Photo Upload Box (Dashed border with upload icon)
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

        // Info pill: "ℹ Category • description • place • date. No AI."
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
    // 06. ITEM DETAILS SCREEN (Item, place, date & reporter)
    // ==========================================
    private View BuildDetails()
    {
        var vm = ViewModel.ItemDetails;
        var item = vm.Item ?? new Item { Title = "Item Details", Location = "Campus" };

        var backBtn = new Button
        {
            Text = "← Back",
            FontSize = 13,
            FontFamily = "OpenSansSemibold",
            TextColor = Color.FromArgb(FigmaBlue),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            Command = vm.GoBackCommand
        };

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                backBtn,
                new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                    Children =
                    {
                        Text(item.Title, 22, true, FigmaTextDark),
                        StatusBadge(item.TypeDisplay, item.StatusColor, item.StatusBgColor)
                    }
                },
                new Border
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
                            DetailRow("Category", item.Category),
                            DetailRow("Place", item.Location),
                            DetailRow("Date", item.DateDisplay),
                            DetailRow("Declared By", item.ReporterName),
                            DetailRow("Status", item.StatusDisplay)
                        }
                    }
                },
                Text("Description", 15, true, FigmaTextDark),
                new Border
                {
                    BackgroundColor = Colors.White,
                    Stroke = Color.FromArgb(FigmaBorder),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Padding = 12,
                    Content = Text(item.Description, 13, false, FigmaTextDark)
                },

                FigmaOutlinedButton($"View Matches ({vm.MatchesCount})", () => vm.ViewMatchesCommand.Execute(null)),
                FigmaPrimaryButton("Contact Reporter Securely", () => vm.ContactSecurelyCommand.Execute(null))
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 07. POSSIBLE MATCHES SCREEN (4 Criteria Comparison)
    // ==========================================
    private View BuildMatches()
    {
        var vm = ViewModel.Matches;

        var header = new VerticalStackLayout
        {
            Spacing = 3,
            Children =
            {
                Text("Possible Matches", 22, true, FigmaTextDark),
                Text("Evaluated strictly on 4 criteria: Category, Place, Date & Keywords (No AI)", 12, false, FigmaTextMuted)
            }
        };

        var body = new VerticalStackLayout { Spacing = 14, Children = { header } };

        if (vm.IsEmpty)
        {
            body.Children.Add(EmptyState("No possible matches detected yet for your declarations."));
        }
        else
        {
            foreach (var match in vm.Matches)
            {
                var m = match;
                var matchCard = new Border
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
                                    Text(m.CandidateItem.Title, 15, true, FigmaTextDark),
                                    StatusBadge(m.ScoreDisplay, FigmaBlue, FigmaLightBlue)
                                }
                            },
                            Text($"📍 {m.CandidateItem.Location}  •  📅 {m.CandidateItem.DateDisplay}", 11, false, FigmaTextMuted),
                            Text(m.CandidateItem.Description, 12, false, FigmaTextDark),

                            // 4 Criteria comparison pills
                            new HorizontalStackLayout
                            {
                                Spacing = 6,
                                Children =
                                {
                                    CriteriaPill("Category", m.CategoryMatch),
                                    CriteriaPill("Place", m.PlaceMatch),
                                    CriteriaPill("Date", m.DateMatch),
                                    CriteriaPill("Keywords", m.DescriptionMatch)
                                }
                            },

                            new Grid
                            {
                                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
                                ColumnSpacing = 8,
                                Children =
                                {
                                    FigmaOutlinedButton("View Details", () => vm.ViewDetailsCommand.Execute(m)),
                                    FigmaPrimaryButton("Contact", () => vm.ContactCommand.Execute(m))
                                }
                            }
                        }
                    }
                };

                body.Children.Add(matchCard);
            }
        }

        return Scroll(body);
    }

    // ==========================================
    // 08. NOTIFICATIONS SCREEN
    // ==========================================
    private View BuildNotifications()
    {
        var vm = ViewModel.Notifications;

        var filterPills = new HorizontalStackLayout
        {
            Spacing = 8,
            Children =
            {
                FilterButton("All", vm.Filter == "All", () => vm.SelectFilterCommand.Execute("All")),
                FilterButton($"Unread ({vm.UnreadCount})", vm.Filter == "Unread", () => vm.SelectFilterCommand.Execute("Unread"))
            }
        };

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                Text("Notifications", 22, true, FigmaTextDark),
                filterPills
            }
        };

        if (vm.IsEmpty)
        {
            body.Children.Add(EmptyState("No notifications to display right now."));
        }
        else
        {
            foreach (var notif in vm.Notifications)
            {
                var n = notif;
                var notifCard = new Border
                {
                    BackgroundColor = n.IsRead ? Colors.White : Color.FromArgb(FigmaLightBlue),
                    Stroke = Color.FromArgb(FigmaBorder),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 10 },
                    Padding = 12,
                    Content = new Grid
                    {
                        ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
                        ColumnSpacing = 12,
                        Children =
                        {
                            Text(n.Icon, 20, true, n.IsRead ? FigmaTextMuted : FigmaBlue),
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
                                            Text(n.Title, 13, !n.IsRead, FigmaTextDark),
                                            Text(n.TimeAgoDisplay, 10, false, FigmaTextMuted)
                                        }
                                    },
                                    Text(n.Message, 11, false, FigmaTextDark)
                                }
                            }
                        }
                    }
                };

                var tap = new TapGestureRecognizer();
                tap.Tapped += (_, _) => vm.SelectNotificationCommand.Execute(n);
                notifCard.GestureRecognizers.Add(tap);

                body.Children.Add(notifCard);
            }
        }

        return Scroll(body);
    }

    // ==========================================
    // 09. SECURE CONVERSATION (In-app messaging)
    // ==========================================
    private View BuildConversation()
    {
        var vm = ViewModel.Conversation;

        var backBtn = new Button
        {
            Text = "← Back",
            FontSize = 13,
            FontFamily = "OpenSansSemibold",
            TextColor = Color.FromArgb(FigmaBlue),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            Command = vm.GoBackCommand
        };

        var safetyBanner = new Border
        {
            BackgroundColor = Color.FromArgb("#FFF8E6"),
            Stroke = Color.FromArgb("#F9E2AF"),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 8 },
            Padding = 10,
            Content = new VerticalStackLayout
            {
                Spacing = 2,
                Children =
                {
                    Text("🛡 Secure in-app contact", 12, true, Orange),
                    Text("Your contact details remain confidential. Arrange a safe public handover.", 11, false, FigmaTextDark)
                }
            }
        };

        var messagesList = new VerticalStackLayout { Spacing = 10 };
        foreach (var msg in vm.Messages)
        {
            var isMe = msg.IsFromCurrentUser;
            var bubble = new Border
            {
                BackgroundColor = isMe ? Color.FromArgb(FigmaBlue) : Colors.White,
                Stroke = isMe ? Color.FromArgb(FigmaBlue) : Color.FromArgb(FigmaBorder),
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = 12 },
                Padding = new Thickness(12, 8),
                HorizontalOptions = isMe ? LayoutOptions.End : LayoutOptions.Start,
                MaximumWidthRequest = 270,
                Content = new VerticalStackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        Text(msg.Text, 13, false, isMe ? "#FFFFFF" : FigmaTextDark),
                        Text(msg.TimeDisplay, 10, false, isMe ? "#D8E3F5" : FigmaTextMuted)
                    }
                }
            };
            messagesList.Children.Add(bubble);
        }

        var messageEntry = new Entry
        {
            Text = vm.MessageInput,
            Placeholder = "Type a message...",
            FontSize = 13,
            TextColor = Color.FromArgb(FigmaTextDark),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        messageEntry.TextChanged += (_, e) => vm.MessageInput = e.NewTextValue;

        var sendBtn = new Button
        {
            Text = "Send",
            FontSize = 12,
            FontFamily = "OpenSansSemibold",
            TextColor = Colors.White,
            BackgroundColor = Color.FromArgb(FigmaBlue),
            CornerRadius = 8,
            HeightRequest = 38,
            Padding = new Thickness(12, 0)
        };
        sendBtn.Clicked += (_, _) =>
        {
            vm.SendMessageCommand.Execute(null);
            messageEntry.Text = string.Empty;
            RenderScreen();
        };

        var inputBar = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(FigmaBorder),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = 10 },
            Padding = new Thickness(10, 4),
            Content = new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                ColumnSpacing = 8,
                Children = { messageEntry, sendBtn }
            }
        };

        var layout = new Grid
        {
            RowDefinitions =
            {
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Auto),
                new RowDefinition(GridLength.Star),
                new RowDefinition(GridLength.Auto)
            },
            RowSpacing = 10,
            Padding = new Thickness(16, 12, 16, 14)
        };

        layout.Add(new HorizontalStackLayout
        {
            Spacing = 10,
            Children =
            {
                backBtn,
                Text(vm.OtherPartyName, 17, true, FigmaTextDark)
            }
        }, 0, 0);

        layout.Add(safetyBanner, 0, 1);
        layout.Add(new ScrollView { Content = messagesList }, 0, 2);
        layout.Add(inputBar, 0, 3);

        return layout;
    }

    // ==========================================
    // 10. PROFILE SCREEN
    // ==========================================
    private View BuildProfile()
    {
        var vm = ViewModel.Profile;
        var user = vm.UserProfile;

        var avatar = new Border
        {
            BackgroundColor = Color.FromArgb(FigmaBlue),
            StrokeShape = new RoundRectangle { CornerRadius = 30 },
            WidthRequest = 60,
            HeightRequest = 60,
            HorizontalOptions = LayoutOptions.Center,
            Content = Text(user.Initials, 22, true, "#FFFFFF")
        };

        var statsGrid = new Grid
        {
            ColumnDefinitions =
            {
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star),
                new ColumnDefinition(GridLength.Star)
            },
            ColumnSpacing = 8,
            Children =
            {
                StatCard("Lost", user.LostReportsCount.ToString(), Red, () => vm.ViewLostItemsCommand.Execute(null)),
                StatCard("Found", user.FoundReportsCount.ToString(), Green, () => vm.ViewFoundItemsCommand.Execute(null)),
                StatCard("Matches", user.ActiveMatchesCount.ToString(), FigmaBlue, () => vm.ViewMatchesCommand.Execute(null))
            }
        };

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                avatar,
                new VerticalStackLayout
                {
                    HorizontalOptions = LayoutOptions.Center,
                    Spacing = 2,
                    Children =
                    {
                        Text(user.FullName, 18, true, FigmaTextDark),
                        Text(user.Email, 12, false, FigmaTextMuted),
                        Text(user.Role, 11, false, FigmaTextMuted)
                    }
                },
                statsGrid,
                Text("Account & Settings", 15, true, FigmaTextDark),
                new Border
                {
                    BackgroundColor = Colors.White,
                    Stroke = Color.FromArgb(FigmaBorder),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = 12 },
                    Padding = 10,
                    Content = new VerticalStackLayout
                    {
                        Spacing = 6,
                        Children =
                        {
                            SettingLink("🔔 Notifications", () => vm.ViewNotificationsCommand.Execute(null)),
                            SettingLink("🔒 Privacy & Security", () => DisplayAlert("Privacy & Security", "All data is end-to-end secured. Your contact details are never disclosed to other users.", "OK")),
                            SettingLink("❓ Help & Support", () => DisplayAlert("Help & Support", "SaveIt facilitates safe lost & found reporting with strict 4-criteria matching.", "OK"))
                        }
                    }
                },
                FigmaOutlinedButton("Log Out", () => vm.LogoutCommand.Execute(null))
            }
        };

        return Scroll(body);
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
}
