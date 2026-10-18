using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class ReportViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly Action<string, object?> _navigateAction;

    private ItemType _reportType = ItemType.Lost;
    private string _title = string.Empty;
    private string _category = "Wallet";
    private string _location = string.Empty;
    private DateTime _date = DateTime.Today;
    private string _description = string.Empty;
    private string? _errorMessage;

    public ReportViewModel(IItemService itemService, Action<string, object?> navigateAction)
    {
        _itemService = itemService;
        _navigateAction = navigateAction;

        Categories = ["Wallet", "Keys", "Phone / Tech", "Bags", "Documents", "Clothing", "Other"];

        SelectTypeCommand = new Command<string>((type) =>
        {
            if (Enum.TryParse<ItemType>(type, true, out var t))
            {
                ReportType = t;
            }
        });
        SubmitCommand = new Command(async () => await ExecuteSubmitAsync());
        CancelCommand = new Command(() => _navigateAction("Home", null));
    }

    public List<string> Categories { get; }

    public ItemType ReportType
    {
        get => _reportType;
        set => SetProperty(ref _reportType, value);
    }

    public bool IsLost => ReportType == ItemType.Lost;
    public bool IsFound => ReportType == ItemType.Found;

    public string ItemTitle
    {
        get => _title;
        set => SetProperty(ref _title, value);
    }

    public string Category
    {
        get => _category;
        set => SetProperty(ref _category, value);
    }

    public string Location
    {
        get => _location;
        set => SetProperty(ref _location, value);
    }

    public DateTime Date
    {
        get => _date;
        set => SetProperty(ref _date, value);
    }

    public string Description
    {
        get => _description;
        set => SetProperty(ref _description, value);
    }

    public string? ErrorMessage
    {
        get => _errorMessage;
        set => SetProperty(ref _errorMessage, value);
    }

    public ICommand SelectTypeCommand { get; }
    public ICommand SubmitCommand { get; }
    public ICommand CancelCommand { get; }

    public void SetInitialType(ItemType type)
    {
        ReportType = type;
        ErrorMessage = null;
    }

    private async Task ExecuteSubmitAsync()
    {
        ErrorMessage = null;
        if (string.IsNullOrWhiteSpace(ItemTitle))
        {
            ErrorMessage = "Please provide an item title.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Location))
        {
            ErrorMessage = "Please specify the place or location.";
            return;
        }

        if (string.IsNullOrWhiteSpace(Description))
        {
            ErrorMessage = "Please provide a brief description.";
            return;
        }

        IsBusy = true;
        var newItem = new Item
        {
            Title = ItemTitle.Trim(),
            Category = Category,
            Location = Location.Trim(),
            Date = Date,
            Type = ReportType,
            Description = Description.Trim(),
            ReporterName = "Arielle S.",
            ReporterId = "user_1"
        };

        var saved = await _itemService.AddItemAsync(newItem);
        IsBusy = false;

        // Reset form
        ItemTitle = string.Empty;
        Location = string.Empty;
        Description = string.Empty;

        // Successful submission -> Navigate to Item Details of saved item
        _navigateAction("Details", saved);
    }
}
