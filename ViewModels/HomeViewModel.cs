using System.Collections.ObjectModel;
using System.Windows.Input;
using SaveIt.Models;
using SaveIt.Services;

namespace SaveIt.ViewModels;

public class HomeViewModel : BaseViewModel
{
    private readonly IItemService _itemService;
    private readonly Action<string, object?> _navigateAction;

    private string _searchQuery = string.Empty;
    private string _selectedFilter = "All"; // "All", "Lost", "Found"
    private int _suggestedMatchesCount;
    private bool _hasSuggestedMatches;

    public HomeViewModel(IItemService itemService, Action<string, object?> navigateAction)
    {
        _itemService = itemService;
        _navigateAction = navigateAction;

        Items = [];

        SearchCommand = new Command(async () => await LoadItemsAsync());
        SelectFilterCommand = new Command<string>(async (filter) =>
        {
            if (filter == null) return;
            SelectedFilter = filter;
            await LoadItemsAsync();
        });
        SelectItemCommand = new Command<Item>((item) =>
        {
            if (item != null)
                _navigateAction("Details", item);
        });
        ReportLostCommand = new Command(() => _navigateAction("Report", ItemType.Lost));
        ReportFoundCommand = new Command(() => _navigateAction("Report", ItemType.Found));
        ViewMatchesCommand = new Command(() => _navigateAction("Matches", null));
        OpenNotificationsCommand = new Command(() => _navigateAction("Notifications", null));
    }

    public ObservableCollection<Item> Items { get; }

    public string SearchQuery
    {
        get => _searchQuery;
        set
        {
            if (SetProperty(ref _searchQuery, value))
            {
                _ = LoadItemsAsync();
            }
        }
    }

    public string SelectedFilter
    {
        get => _selectedFilter;
        set => SetProperty(ref _selectedFilter, value);
    }

    public int SuggestedMatchesCount
    {
        get => _suggestedMatchesCount;
        set
        {
            if (SetProperty(ref _suggestedMatchesCount, value))
            {
                HasSuggestedMatches = value > 0;
            }
        }
    }

    public bool HasSuggestedMatches
    {
        get => _hasSuggestedMatches;
        set => SetProperty(ref _hasSuggestedMatches, value);
    }

    public ICommand SearchCommand { get; }
    public ICommand SelectFilterCommand { get; }
    public ICommand SelectItemCommand { get; }
    public ICommand ReportLostCommand { get; }
    public ICommand ReportFoundCommand { get; }
    public ICommand ViewMatchesCommand { get; }
    public ICommand OpenNotificationsCommand { get; }

    public async Task LoadItemsAsync()
    {
        IsBusy = true;
        var list = await _itemService.GetItemsAsync(SelectedFilter, SearchQuery);
        Items.Clear();
        foreach (var item in list)
        {
            Items.Add(item);
        }

        var matches = await _itemService.GetAllMatchesAsync();
        SuggestedMatchesCount = matches.Count;
        IsBusy = false;
    }
}
