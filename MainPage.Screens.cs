using Microsoft.Maui.Controls.Shapes;
using SaveIt.Models;

namespace SaveIt;

public partial class MainPage
{
    // ==========================================
    // 01. SPLASH SCREEN (Brand + session check)
    // ==========================================
    private View BuildSplash()
    {
        var content = new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            VerticalOptions = LayoutOptions.Center,
            Spacing = 12,
            Children =
            {
                Text("⌖", 54, true, Blue),
                Text("SaveIt", 28, true, Blue),
                Text("Lost. Found. Reunited.", 13, false, Muted),
                new ActivityIndicator
                {
                    IsRunning = true,
                    Color = Color.FromArgb(Blue),
                    Margin = new Thickness(0, 80, 0, 0)
                },
                Text("Checking session...", 11, false, Muted)
            }
        };
        return new Grid { BackgroundColor = Colors.White, Children = { content } };
    }

    // ==========================================
    // 02. LOGIN SCREEN (Sign in or recover access)
    // ==========================================
    private View BuildLogin()
    {
        var vm = ViewModel.Auth;

        var emailEntry = new Entry
        {
            Text = vm.Email,
            Placeholder = "you@example.com",
            Keyboard = Keyboard.Email,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        emailEntry.TextChanged += (_, e) => vm.Email = e.NewTextValue;

        var passEntry = new Entry
        {
            Text = vm.Password,
            Placeholder = "Password",
            IsPassword = true,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        passEntry.TextChanged += (_, e) => vm.Password = e.NewTextValue;

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                new HorizontalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        Text("⌖", 22, true, Blue),
                        Text("SaveIt", 18, true, Blue)
                    }
                },
                Text("Welcome Back!", 24, true, Ink),
                Text("Log in to report or track lost items", 12, false, Muted),

                FormLabel("Email address"),
                EntryField("✉", emailEntry),

                FormLabel("Password"),
                EntryField("🔒", passEntry),

                new Button
                {
                    Text = "Forgot password?",
                    FontSize = 12,
                    TextColor = Color.FromArgb(Blue),
                    BackgroundColor = Colors.Transparent,
                    HorizontalOptions = LayoutOptions.End,
                    Padding = 0,
                    Command = vm.ForgotPasswordCommand
                }
            }
        };

        if (!string.IsNullOrEmpty(vm.ErrorMessage))
        {
            body.Children.Add(ErrorBanner(vm.ErrorMessage));
        }

        body.Children.Add(ActionButton("Log In", () => vm.LoginCommand.Execute(null)));
        body.Children.Add(new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 4,
            Children =
            {
                Text("Don't have an account?", 12, false, Muted),
                new Button
                {
                    Text = "Sign Up",
                    FontSize = 12,
                    FontFamily = "OpenSansSemibold",
                    TextColor = Color.FromArgb(Blue),
                    BackgroundColor = Colors.Transparent,
                    Padding = 0,
                    Command = vm.ToggleModeCommand
                }
            }
        });

        return Scroll(body);
    }

    // ==========================================
    // 03. REGISTRATION SCREEN (Create account)
    // ==========================================
    private View BuildRegistration()
    {
        var vm = ViewModel.Auth;

        var nameEntry = new Entry
        {
            Text = vm.FullName,
            Placeholder = "Full Name",
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        nameEntry.TextChanged += (_, e) => vm.FullName = e.NewTextValue;

        var emailEntry = new Entry
        {
            Text = vm.Email,
            Placeholder = "you@example.com",
            Keyboard = Keyboard.Email,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        emailEntry.TextChanged += (_, e) => vm.Email = e.NewTextValue;

        var passEntry = new Entry
        {
            Text = vm.Password,
            Placeholder = "At least 6 characters",
            IsPassword = true,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        passEntry.TextChanged += (_, e) => vm.Password = e.NewTextValue;

        var confirmEntry = new Entry
        {
            Text = vm.ConfirmPassword,
            Placeholder = "Confirm password",
            IsPassword = true,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        confirmEntry.TextChanged += (_, e) => vm.ConfirmPassword = e.NewTextValue;

        var body = new VerticalStackLayout
        {
            Spacing = 13,
            Children =
            {
                Text("Create Account", 24, true, Ink),
                Text("Join SaveIt to reunite lost items securely", 12, false, Muted),

                FormLabel("Full Name"),
                EntryField("👤", nameEntry),

                FormLabel("Email address"),
                EntryField("✉", emailEntry),

                FormLabel("Password"),
                EntryField("🔒", passEntry),

                FormLabel("Confirm Password"),
                EntryField("🔒", confirmEntry)
            }
        };

        if (!string.IsNullOrEmpty(vm.ErrorMessage))
        {
            body.Children.Add(ErrorBanner(vm.ErrorMessage));
        }

        body.Children.Add(ActionButton("Sign Up", () => vm.RegisterCommand.Execute(null)));
        body.Children.Add(new HorizontalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 4,
            Children =
            {
                Text("Already have an account?", 12, false, Muted),
                new Button
                {
                    Text = "Log In",
                    FontSize = 12,
                    FontFamily = "OpenSansSemibold",
                    TextColor = Color.FromArgb(Blue),
                    BackgroundColor = Colors.Transparent,
                    Padding = 0,
                    Command = vm.ToggleModeCommand
                }
            }
        });

        return Scroll(body);
    }

    // ==========================================
    // 04. HOME SCREEN (Reports, search, filters)
    // ==========================================
    private View BuildHome()
    {
        var vm = ViewModel.Home;

        // Search Bar
        var searchInput = new Entry
        {
            Text = vm.SearchQuery,
            Placeholder = "Search lost or found items...",
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            PlaceholderColor = Color.FromArgb(Muted),
            BackgroundColor = Colors.Transparent,
            ClearButtonVisibility = ClearButtonVisibility.WhileEditing,
            HorizontalOptions = LayoutOptions.FillAndExpand
        };
        searchInput.TextChanged += (_, e) => vm.SearchQuery = e.NewTextValue;

        var searchBox = new Border
        {
            BackgroundColor = Colors.White,
            Stroke = Color.FromArgb(Line),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
            Padding = new Thickness(12, 0),
            HeightRequest = 46,
            Content = new HorizontalStackLayout
            {
                Spacing = 10,
                Children =
                {
                    Text("🔍", 14, false, Muted),
                    searchInput
                }
            }
        };

        // Filter Pills (All, Lost, Found)
        var filtersLayout = new HorizontalStackLayout { Spacing = 8 };
        foreach (var filter in new[] { "All", "Lost", "Found" })
        {
            var isSelected = vm.SelectedFilter.Equals(filter, StringComparison.OrdinalIgnoreCase);
            var pill = new Button
            {
                Text = filter,
                FontSize = 12,
                FontFamily = isSelected ? "OpenSansSemibold" : "OpenSansRegular",
                TextColor = isSelected ? Colors.White : Color.FromArgb(Ink),
                BackgroundColor = isSelected ? Color.FromArgb(Blue) : Colors.White,
                BorderColor = isSelected ? Color.FromArgb(Blue) : Color.FromArgb(Line),
                BorderWidth = 1,
                CornerRadius = 18,
                HeightRequest = 34,
                Padding = new Thickness(14, 0)
            };
            var f = filter;
            pill.Clicked += (_, _) => vm.SelectFilterCommand.Execute(f);
            filtersLayout.Children.Add(pill);
        }

        // Quick Actions: Report Lost / Report Found
        var quickActions = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 10
        };

        var lostBtn = ActionButton("+ Report Lost", () => vm.ReportLostCommand.Execute(null), true, Red);
        lostBtn.HeightRequest = 42;
        quickActions.Add(lostBtn, 0, 0);

        var foundBtn = ActionButton("+ Report Found", () => vm.ReportFoundCommand.Execute(null), true, Green);
        foundBtn.HeightRequest = 42;
        quickActions.Add(foundBtn, 1, 0);

        var body = new VerticalStackLayout
        {
            Spacing = 14,
            Children =
            {
                new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                    Children =
                    {
                        new VerticalStackLayout
                        {
                            Children =
                            {
                                Text("SaveIt", 22, true, Blue),
                                Text("Find, report & reunite", 12, false, Muted)
                            }
                        },
                        new Button
                        {
                            Text = "🔔",
                            FontSize = 18,
                            TextColor = Color.FromArgb(Ink),
                            BackgroundColor = Colors.White,
                            BorderColor = Color.FromArgb(Line),
                            BorderWidth = 1,
                            CornerRadius = 10,
                            WidthRequest = 40,
                            HeightRequest = 40,
                            Command = vm.OpenNotificationsCommand
                        }
                    }
                },
                searchBox,
                filtersLayout,
                quickActions
            }
        };

        // Possible Matches Banner if any
        if (vm.HasSuggestedMatches)
        {
            var matchBanner = Card(new Grid
            {
                ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                Children =
                {
                    new VerticalStackLayout
                    {
                        Spacing = 2,
                        Children =
                        {
                            Text($"♡ {vm.SuggestedMatchesCount} Potential Match(es)", 13, true, Blue),
                            Text("Automatic 4-criteria comparisons found", 11, false, Muted)
                        }
                    },
                    new Button
                    {
                        Text = "View →",
                        FontSize = 12,
                        FontFamily = "OpenSansSemibold",
                        TextColor = Color.FromArgb(Blue),
                        BackgroundColor = Colors.Transparent,
                        Command = vm.ViewMatchesCommand
                    }
                }
            }, PaleBlue);
            body.Children.Add(matchBanner);
        }

        // Section Title
        body.Children.Add(Text($"Recent Reports ({vm.Items.Count})", 16, true, Ink));

        // Items list
        if (vm.Items.Count == 0)
        {
            body.Children.Add(EmptyState("No items found matching your search."));
        }
        else
        {
            foreach (var item in vm.Items)
            {
                var current = item;
                var cardContent = new VerticalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        new Grid
                        {
                            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                            Children =
                            {
                                Text(current.Title, 15, true, Ink),
                                StatusBadge(current.TypeDisplay, current.StatusColor, current.StatusBgColor)
                            }
                        },
                        Text($"📍 {current.Location}  •  📅 {current.DateDisplay}", 11, false, Muted),
                        Text(current.Description, 12, false, Ink)
                    }
                };

                var itemCard = Card(cardContent);
                var tap = new TapGestureRecognizer();
                tap.Tapped += (_, _) => vm.SelectItemCommand.Execute(current);
                itemCard.GestureRecognizers.Add(tap);

                body.Children.Add(itemCard);
            }
        }

        return Scroll(body);
    }

    // ==========================================
    // 05. REPORT AN ITEM (Shared Lost/Found form)
    // ==========================================
    private View BuildReport()
    {
        var vm = ViewModel.Report;

        // Toggle Type: Lost or Found
        var typeToggle = new Grid
        {
            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Star) },
            ColumnSpacing = 8
        };

        var lostBtn = new Button
        {
            Text = "Lost Item",
            FontSize = 13,
            FontFamily = vm.IsLost ? "OpenSansSemibold" : "OpenSansRegular",
            TextColor = vm.IsLost ? Colors.White : Color.FromArgb(Ink),
            BackgroundColor = vm.IsLost ? Color.FromArgb(Red) : Colors.White,
            BorderColor = Color.FromArgb(Line),
            BorderWidth = 1,
            CornerRadius = 10,
            HeightRequest = 42
        };
        lostBtn.Clicked += (_, _) =>
        {
            vm.SelectTypeCommand.Execute("Lost");
            RenderScreen();
        };
        typeToggle.Add(lostBtn, 0, 0);

        var foundBtn = new Button
        {
            Text = "Found Item",
            FontSize = 13,
            FontFamily = vm.IsFound ? "OpenSansSemibold" : "OpenSansRegular",
            TextColor = vm.IsFound ? Colors.White : Color.FromArgb(Ink),
            BackgroundColor = vm.IsFound ? Color.FromArgb(Green) : Colors.White,
            BorderColor = Color.FromArgb(Line),
            BorderWidth = 1,
            CornerRadius = 10,
            HeightRequest = 42
        };
        foundBtn.Clicked += (_, _) =>
        {
            vm.SelectTypeCommand.Execute("Found");
            RenderScreen();
        };
        typeToggle.Add(foundBtn, 1, 0);

        var titleEntry = new Entry
        {
            Text = vm.ItemTitle,
            Placeholder = "e.g., Black Leather Wallet",
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        titleEntry.TextChanged += (_, e) => vm.ItemTitle = e.NewTextValue;

        var locationEntry = new Entry
        {
            Text = vm.Location,
            Placeholder = "e.g., Campus Library - 2nd floor",
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        locationEntry.TextChanged += (_, e) => vm.Location = e.NewTextValue;

        var descEditor = new Editor
        {
            Text = vm.Description,
            Placeholder = "Describe distinctive marks, brand, contents, color...",
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent,
            HeightRequest = 80
        };
        descEditor.TextChanged += (_, e) => vm.Description = e.NewTextValue;

        var categoryPicker = new Picker
        {
            ItemsSource = vm.Categories,
            SelectedItem = vm.Category,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        categoryPicker.SelectedIndexChanged += (_, _) =>
        {
            if (categoryPicker.SelectedItem is string cat)
                vm.Category = cat;
        };

        var datePicker = new DatePicker
        {
            Date = vm.Date,
            FontSize = 13,
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent
        };
        datePicker.DateSelected += (_, e) => vm.Date = e.NewDate;

        var body = new VerticalStackLayout
        {
            Spacing = 13,
            Children =
            {
                Text("Report an Item", 22, true, Ink),
                Text("Declare a lost or found object to start smart matching", 12, false, Muted),
                typeToggle,

                FormLabel("Item Title *"),
                EntryField("🏷", titleEntry),

                FormLabel("Category *"),
                EntryField("📁", categoryPicker),

                FormLabel("Place / Location *"),
                EntryField("📍", locationEntry),

                FormLabel("Date *"),
                EntryField("📅", datePicker),

                FormLabel("Description & Distinctive details *"),
                new Border
                {
                    BackgroundColor = Colors.White,
                    Stroke = Color.FromArgb(Line),
                    StrokeThickness = 1,
                    StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(9) },
                    Padding = 10,
                    Content = descEditor
                }
            }
        };

        if (!string.IsNullOrEmpty(vm.ErrorMessage))
        {
            body.Children.Add(ErrorBanner(vm.ErrorMessage));
        }

        body.Children.Add(ActionButton("Submit Report", () => vm.SubmitCommand.Execute(null), true, vm.IsLost ? Red : Green));
        body.Children.Add(ActionButton("Cancel", () => vm.CancelCommand.Execute(null), false));

        return Scroll(body);
    }

    // ==========================================
    // 06. ITEM DETAILS SCREEN (Item, place, date & reporter)
    // ==========================================
    private View BuildDetails()
    {
        var vm = ViewModel.ItemDetails;
        var item = vm.Item ?? new Item { Title = "Item Details", Location = "Unknown" };

        var backBtn = new Button
        {
            Text = "← Back",
            FontSize = 13,
            FontFamily = "OpenSansSemibold",
            TextColor = Color.FromArgb(Blue),
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
                        Text(item.Title, 22, true, Ink),
                        StatusBadge(item.TypeDisplay, item.StatusColor, item.StatusBgColor)
                    }
                },
                Card(new VerticalStackLayout
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
                }),
                Text("Description", 15, true, Ink),
                Card(Text(item.Description, 13, false, Ink)),

                ActionButton($"View Matches ({vm.MatchesCount})", () => vm.ViewMatchesCommand.Execute(null), false),
                ActionButton("Contact Reporter Securely", () => vm.ContactSecurelyCommand.Execute(null), true)
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // 07. POSSIBLE MATCHES (Compare the 4 criteria)
    // ==========================================
    private View BuildMatches()
    {
        var vm = ViewModel.Matches;

        var header = new VerticalStackLayout
        {
            Spacing = 3,
            Children =
            {
                Text("Possible Matches", 22, true, Ink),
                Text("Evaluated strictly on 4 criteria: Category, Place, Date & Keywords (No AI)", 12, false, Muted)
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
                var cardContent = new VerticalStackLayout
                {
                    Spacing = 8,
                    Children =
                    {
                        new Grid
                        {
                            ColumnDefinitions = { new ColumnDefinition(GridLength.Star), new ColumnDefinition(GridLength.Auto) },
                            Children =
                            {
                                Text(m.CandidateItem.Title, 16, true, Ink),
                                StatusBadge(m.ScoreDisplay, Blue, PaleBlue)
                            }
                        },
                        Text($"📍 {m.CandidateItem.Location}  •  📅 {m.CandidateItem.DateDisplay}", 11, false, Muted),
                        Text(m.CandidateItem.Description, 12, false, Ink),

                        // Criteria comparison row
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
                                ActionButton("View Details", () => vm.ViewDetailsCommand.Execute(m), false),
                                ActionButton("Contact", () => vm.ContactCommand.Execute(m), true)
                            }
                        }
                    }
                };

                body.Children.Add(Card(cardContent));
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
                Text("Notifications", 22, true, Ink),
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
                var notifCard = Card(new Grid
                {
                    ColumnDefinitions = { new ColumnDefinition(GridLength.Auto), new ColumnDefinition(GridLength.Star) },
                    ColumnSpacing = 12,
                    Children =
                    {
                        Text(n.Icon, 20, true, n.IsRead ? Muted : Blue),
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
                                        Text(n.Title, 14, !n.IsRead, Ink),
                                        Text(n.TimeAgoDisplay, 11, false, Muted)
                                    }
                                },
                                Text(n.Message, 12, false, Ink)
                            }
                        }
                    }
                }, n.IsRead ? "#FFFFFF" : PaleBlue);

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
            TextColor = Color.FromArgb(Blue),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.Start,
            Padding = 0,
            Command = vm.GoBackCommand
        };

        var safetyBanner = Card(new VerticalStackLayout
        {
            Spacing = 3,
            Children =
            {
                Text("🛡 Secure in-app contact", 12, true, Orange),
                Text("Your contact details remain confidential. Arrange a safe public handover.", 11, false, Ink)
            }
        }, "#FFF8E6");

        var messagesList = new VerticalStackLayout { Spacing = 10 };
        foreach (var msg in vm.Messages)
        {
            var isMe = msg.IsFromCurrentUser;
            var bubble = new Border
            {
                BackgroundColor = isMe ? Color.FromArgb(Blue) : Colors.White,
                Stroke = isMe ? Color.FromArgb(Blue) : Color.FromArgb(Line),
                StrokeThickness = 1,
                StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(12) },
                Padding = new Thickness(12, 8),
                HorizontalOptions = isMe ? LayoutOptions.End : LayoutOptions.Start,
                MaximumWidthRequest = 270,
                Content = new VerticalStackLayout
                {
                    Spacing = 2,
                    Children =
                    {
                        Text(msg.Text, 13, false, isMe ? "#FFFFFF" : Ink),
                        Text(msg.TimeDisplay, 10, false, isMe ? "#D8E3F5" : Muted)
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
            TextColor = Color.FromArgb(Ink),
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
            BackgroundColor = Color.FromArgb(Blue),
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
            Stroke = Color.FromArgb(Line),
            StrokeThickness = 1,
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(10) },
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
                Text(vm.OtherPartyName, 17, true, Ink)
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
            BackgroundColor = Color.FromArgb(Blue),
            StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(30) },
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
                StatCard("Matches", user.ActiveMatchesCount.ToString(), Blue, () => vm.ViewMatchesCommand.Execute(null))
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
                        Text(user.FullName, 18, true, Ink),
                        Text(user.Email, 12, false, Muted),
                        Text(user.Role, 11, false, Muted)
                    }
                },
                statsGrid,
                Text("Account & Settings", 15, true, Ink),
                Card(new VerticalStackLayout
                {
                    Spacing = 6,
                    Children =
                    {
                        SettingLink("🔔 Notifications", () => vm.ViewNotificationsCommand.Execute(null)),
                        SettingLink("🔒 Privacy & Security", () => DisplayAlert("Privacy & Security", "All data is end-to-end secured. Your contact details are never disclosed to other users.", "OK")),
                        SettingLink("❓ Help & Support", () => DisplayAlert("Help & Support", "SaveIt facilitates safe lost & found reporting with strict 4-criteria matching.", "OK"))
                    }
                }),
                ActionButton("Log Out", () => vm.LogoutCommand.Execute(null), false)
            }
        };

        return Scroll(body);
    }

    // ==========================================
    // Helpers and reusable UI components
    // ==========================================
    private static Border FormLabel(string text) => new()
    {
        StrokeThickness = 0,
        Padding = new Thickness(0, 4, 0, 0),
        Content = Text(text, 12, true, Ink)
    };

    private static Border EntryField(string icon, View field) => new()
    {
        BackgroundColor = Colors.White,
        Stroke = Color.FromArgb(Line),
        StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(9) },
        Padding = new Thickness(12, 0),
        HeightRequest = 46,
        Content = new HorizontalStackLayout
        {
            Spacing = 10,
            Children = { Text(icon, 14, false, Muted), field }
        }
    };

    private static Border ErrorBanner(string message) => Card(
        Text($"⚠️ {message}", 12, false, Red),
        "#FEECEE");

    private static Border EmptyState(string message) => Card(
        new VerticalStackLayout
        {
            HorizontalOptions = LayoutOptions.Center,
            Spacing = 8,
            Padding = 20,
            Children =
            {
                Text("∅", 28, false, Muted),
                Text(message, 12, false, Muted)
            }
        });

    private static View DetailRow(string label, string value) => new Grid
    {
        ColumnDefinitions = { new ColumnDefinition(100), new ColumnDefinition(GridLength.Star) },
        Children =
        {
            Text(label, 12, true, Muted),
            Text(value, 13, false, Ink)
        }
    };

    private static View CriteriaPill(string label, bool isMatched) => new Border
    {
        BackgroundColor = Color.FromArgb(isMatched ? "#EAF7ED" : "#F3F5FA"),
        Stroke = Color.FromArgb(isMatched ? Green : Line),
        StrokeThickness = 1,
        StrokeShape = new RoundRectangle { CornerRadius = new CornerRadius(6) },
        Padding = new Thickness(6, 2),
        Content = Text($"{(isMatched ? "✓" : "✗")} {label}", 10, isMatched, isMatched ? Green : Muted)
    };

    private static Button FilterButton(string label, bool isSelected, Action action)
    {
        var btn = new Button
        {
            Text = label,
            FontSize = 12,
            FontFamily = isSelected ? "OpenSansSemibold" : "OpenSansRegular",
            TextColor = isSelected ? Colors.White : Color.FromArgb(Ink),
            BackgroundColor = isSelected ? Color.FromArgb(Blue) : Colors.White,
            BorderColor = isSelected ? Color.FromArgb(Blue) : Color.FromArgb(Line),
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
                Text(title, 11, false, Muted)
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
            TextColor = Color.FromArgb(Ink),
            BackgroundColor = Colors.Transparent,
            HorizontalOptions = LayoutOptions.FillAndExpand,
            ContentLayout = new Button.ButtonContentLayout(Button.ButtonContentLayout.ImagePosition.Left, 0),
            Padding = new Thickness(4, 6)
        };
        btn.Clicked += (_, _) => action();
        return btn;
    }
}
